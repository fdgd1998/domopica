using ABElectronics_Win10IOT_Libraries;
using domopica.DevicePages;
using Mfrc522Lib;
using Sensors.Dht;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Windows.Devices.Enumeration;
using Windows.Devices.Gpio;
using Windows.Devices.I2c;
using Windows.UI.Xaml;

namespace domopica
{
    public static class Devices
    {
        private static GpioController gpio = GpioController.GetDefault();
        public static IOPi Bus1 { get; } = new IOPi(0x20);
        private static GpioPin temp_outdoor = gpio.OpenPin(17, GpioSharingMode.Exclusive); // Outdoor
        private static GpioPin temp_livingroom = gpio.OpenPin(18, GpioSharingMode.Exclusive); // Living room
        private static GpioPin temp_bedroom = gpio.OpenPin(27, GpioSharingMode.Exclusive); // Bedroom

        public static Dictionary<string, Light> lightsList = new Dictionary<string, Light>()
        {
            { "living_room_light", new Light(Bus1, 1) },
            { "bedroom_light", new Light(Bus1, 2) },
            { "bathroom_light", new Light(Bus1, 3) },
            { "garage_light", new Light(Bus1, 4) },
            { "stairs_light", new Light(Bus1, 5) },
            { "entrance_light", new Light(Bus1, 6) },
            { "kitchen_light", new Light(Bus1, 7) },
            { "terrace_light", new Light(Bus1, 8) }
        };

        private static GpioPin Reset { get; } = gpio.OpenPin(19);
        private static GpioPin Buzzer { get; } = gpio.OpenPin(13);
        public static GpioPin LivingRoomCurtain { get; } = gpio.OpenPin(20);
        public static GpioPin BedroomCurtain { get; } = gpio.OpenPin(21);
        public static GpioPin BathroomCurtain { get; } = gpio.OpenPin(26);
        public static GpioPin GarageDoor { get; } = gpio.OpenPin(16);
        public static GpioPin EntranceRSw1 { get; } = gpio.OpenPin(24);
        public static GpioPin TerraceRSw2 { get; } = gpio.OpenPin(5);
        public static GpioPin GarageRSw3 { get; } = gpio.OpenPin(23);
        public static GpioPin LightSensor { get; } = gpio.OpenPin(6);

        public static Dictionary<string, Curtain> curtainsList = new Dictionary<string, Curtain>();
        public static Dictionary<string, Door> doorsList = new Dictionary<string, Door>();

        public async static void ConfigureGpio()
        {
            try
            {
                await Bus1.Connect();
            }
            catch (Exception)
            {
                Debug.WriteLine("No se ha podido conectar con el dispositivo I2C 0x20.");
            }

            Bus1.SetPortDirection(0, 0x00);
            Bus1.SetPortDirection(1, 0x00);
            Bus1.WritePort(0, 0x00);
            Bus1.WritePort(1, 0x00);
            Reset.SetDriveMode(GpioPinDriveMode.Output);
            Reset.Write(GpioPinValue.High);
            Buzzer.SetDriveMode(GpioPinDriveMode.Output);
            Buzzer.Write(GpioPinValue.Low);
            EntranceRSw1.SetDriveMode(GpioPinDriveMode.Input);
            TerraceRSw2.SetDriveMode(GpioPinDriveMode.Input);
            GarageRSw3.SetDriveMode(GpioPinDriveMode.Input);
            BedroomCurtain.SetDriveMode(GpioPinDriveMode.Input);
            LivingRoomCurtain.SetDriveMode(GpioPinDriveMode.Input);
            BathroomCurtain.SetDriveMode(GpioPinDriveMode.Input);
            GarageDoor.SetDriveMode(GpioPinDriveMode.Input);
            LightSensor.SetDriveMode(GpioPinDriveMode.Input);
        }


        public static Mfrc522 RFIDReader = new Mfrc522();

        public static void ResetDevices()
        {
            Reset.Write(GpioPinValue.Low);
            Thread.Sleep(100);
            Reset.Write(GpioPinValue.High);
            Debug.WriteLine("Info: device reboot success.");
        }

        public static async void InitI2CDevices()
        {
            string aqs = I2cDevice.GetDeviceSelector();

            var dis = await DeviceInformation.FindAllAsync(aqs);

            var arduino1 = new I2cConnectionSettings(0x40);
            var arduino2 = new I2cConnectionSettings(0x41);
            var arduino3 = new I2cConnectionSettings(0x42);
            var arduino4 = new I2cConnectionSettings(0x43);

            arduino1.BusSpeed = I2cBusSpeed.FastMode;
            arduino2.BusSpeed = I2cBusSpeed.FastMode;
            arduino3.BusSpeed = I2cBusSpeed.FastMode;
            arduino4.BusSpeed = I2cBusSpeed.FastMode;

            var device1 = await I2cDevice.FromIdAsync(dis[0].Id, arduino1);
            var device2 = await I2cDevice.FromIdAsync(dis[0].Id, arduino2);
            var device3 = await I2cDevice.FromIdAsync(dis[0].Id, arduino3);
            var device4 = await I2cDevice.FromIdAsync(dis[0].Id, arduino4);

            curtainsList.Add("living_room_curtain", new Curtain(device1));
            curtainsList.Add("bedroom_curtain", new Curtain(device2));
            curtainsList.Add("bathroom_curtain", new Curtain(device3));
            doorsList.Add("garage_door", new Door(device4));
        }

        private static bool alarmStatus = false;

        private static DispatcherTimer alarmLed = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(2)
        };

        private static DispatcherTimer alarmCountDown = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(0.9)
        };

        private static void AlarmLedFlash(object sender, object e)
        {
            Bus1.WritePin(10, true);
            Thread.Sleep(100);
            Bus1.WritePin(10, false);
        }

        public static void InitTimers()
        {
            alarmLed.Tick += new EventHandler<object>(AlarmLedFlash);
            alarmCountDown.Tick += new EventHandler<object>(AlarmCountDownSound);
        }

        public static void InitListeners()
        {
            EntranceRSw1.ValueChanged += (sender, args) =>
            {
                if (IsAlarmActivated && EntranceRSw1.Read() == GpioPinValue.Low)
                    TriggerAlarm("terraza");
            };

            TerraceRSw2.ValueChanged += (sender, args) =>
            {
                if (IsAlarmActivated && TerraceRSw2.Read() == GpioPinValue.Low)
                    TriggerAlarm("entrada");
            };

            GarageRSw3.ValueChanged += (sender, args) =>
            {
                if (IsAlarmActivated && GarageRSw3.Read() == GpioPinValue.Low)
                    TriggerAlarm("garaje");
            };

            LightSensor.ValueChanged += (sender, args) => { AutoLights(); };

            LivingRoomCurtain.ValueChanged += (sender, args) =>
            {
                if (LivingRoomCurtain.Read() == GpioPinValue.Low && GroundFloorPage.Current != null)
                {
                    Debug.WriteLine("living_room_curatain state changed");
                    DevicePage.Current.EnableCurtainButton("living_room_curtain");
                }
            };

            BedroomCurtain.ValueChanged += (sender, args) =>
            {
                if (BedroomCurtain.Read() == GpioPinValue.Low && FirstFloorPage.Current != null)
                {
                    Debug.WriteLine("bedroom_curatain state changed");
                    DevicePage.Current.EnableCurtainButton("bedroom_curtain");
                }
            };

            BathroomCurtain.ValueChanged += (sender, args) =>
            {

                if (BathroomCurtain.Read() == GpioPinValue.Low && FirstFloorPage.Current != null)
                {
                    Debug.WriteLine("bathroom_curatain state changed");
                    DevicePage.Current.EnableCurtainButton("bathroom_curtain");
                }   
            };

            GarageDoor.ValueChanged += (sender, args) =>
            {
                if (GarageDoor.Read() == GpioPinValue.Low && GroundFloorPage.Current != null)
                {
                    Debug.WriteLine("garage_door state changed");
                    DevicePage.Current.EnableDoorButton("garage_door");
                }   
            };
        }

        public static bool IsLightSensorEnabled { get; set; } = false;
        public static List<string> lightSensorDevices = new List<string>();

        public async static void AutoLights()
        {
            if (IsLightSensorEnabled)
            {
                if (LightSensor.Read() == GpioPinValue.High)
                {
                    foreach (string item in lightSensorDevices)
                        lightsList[item].LightOn = true;
                }
                else
                {
                    foreach (string item in lightSensorDevices)
                        lightsList[item].LightOn = false;
                }

                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    if (GroundFloorPage.Current != null) GroundFloorPage.Current.SetLightButtonStatus();
                    if (FirstFloorPage.Current != null) FirstFloorPage.Current.SetLightButtonStatus();
                });
            }
        }

        private static bool AlarmRinging { get; set; } = false;
        private static bool alarmTriggered = false;

        public static bool AlarmCountDownFirstTime { get; set; } = true;
        public static int AlarmCountDownLeft { get; set; } = 10;

        public static bool IsAlarmActivated
        {
            get
            {
                return alarmStatus;
            }

            set
            {
                if (alarmStatus != value)
                {
                    alarmStatus = value;
                    if (alarmStatus) alarmLed.Start();
                    else alarmLed.Stop();
                }
            }
        }

        public static bool IsAlarmRinging
        {
            get
            {
                return AlarmRinging;
            }
            set
            {
                AlarmRinging = value;
                if (AlarmRinging)
                {
                    AlarmTriggered = false;
                    Buzzer.Write(GpioPinValue.High);
                }
                else Buzzer.Write(GpioPinValue.Low);
            }
        }

        public static bool IsAlarmCountDownEnabled
        {
            get
            {
                return AlarmTriggered;
            }

            set
            {
                AlarmTriggered = value;
                CountDownEnable();
            }
        }

        private static async void CountDownEnable()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    if (AlarmTriggered) alarmCountDown.Start();
                    else
                    {
                        alarmCountDown.Stop();
                        AlarmCountDownLeft = 10;
                        AlarmCountDownFirstTime = true;
                    }
                });
        }

        private static void AlarmCountDownSound(object sender, object e)
        {
            if (AlarmCountDownLeft > 0)
            {
                AlarmCountDownLeft--;
                Buzzer.Write(GpioPinValue.High);
                Thread.Sleep(100);
                Buzzer.Write(GpioPinValue.Low);
            }
            else
            {
                IsAlarmCountDownEnabled = false;
                string device = AlarmCountDown.Current.device;
                AlarmCountDown.Current.Hide();
                IsAlarmRinging = true;
                // Buzzer.Write(GpioPinValue.High);
                Globals.OpenDialog(device, "alarm");
            }
        }

        public static void TriggerAlarm(string room)
        {
            AlarmTriggered = true;
            IsAlarmCountDownEnabled = true;
            Globals.OpenDialog(room, "countdown");
        }

        public static bool AlarmTriggered { 
            get
            {
                return alarmTriggered;
            }
            set
            {
                alarmTriggered = value;
                IsAlarmCountDownRunning();
            } 
        }

        private static async void IsAlarmCountDownRunning()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    if (alarmTriggered) alarmCountDown.Start();
                    else alarmCountDown.Stop();
                });
        }

        /*private static bool isBuzzerRinging = false;
        private static bool IsBuzzerRinging
        {
            get
            {
                return isBuzzerRinging;
            }

            set
            {
                isBuzzerRinging = value;
                Buzzer.Write(GpioPinValue.Low);
            }
        }*/

        public static Dht11[] temp_devices =
        {
            new Dht11(temp_outdoor, GpioPinDriveMode.Input),
            new Dht11(temp_livingroom, GpioPinDriveMode.Input),
            new Dht11(temp_bedroom, GpioPinDriveMode.Input)
        };

        public static void RFIDError()
        {
            Buzzer.Write(GpioPinValue.High);
            Thread.Sleep(100);
            Buzzer.Write(GpioPinValue.Low);
            Thread.Sleep(50);
            Buzzer.Write(GpioPinValue.High);
            Thread.Sleep(100);
            Buzzer.Write(GpioPinValue.Low);
        }

        public static void RFIDSuccess()
        {
            Buzzer.Write(GpioPinValue.High);
            Thread.Sleep(100);
            Buzzer.Write(GpioPinValue.Low);
        }

        public static void ChangeLightState(string name)
        {
            Light light = lightsList[name];
            light.LightOn = light.LightOn ? false : true;

        }

        public static void ChangeCurtainState(string name)
        {
            Curtain c = curtainsList[name];
            c.IsOpened = c.IsOpened ? false : true;

        }

        public static void ChangeDoorState(string name)
        {
            Door d = doorsList[name];
            d.IsOpened = d.IsOpened ? false : true;

        }
    }
}
