using Sensors.Dht;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace domopica
{
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;
        private bool justOpened = true;
        public MainPage()
        {
            InitializeComponent();
            Current = this;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Devices.InitI2CDevices();
            Devices.InitListeners();
            Devices.ConfigureGpio();

            ThreadStart readingTempsMethod = new ThreadStart(UpdateTempValuesThread);
            Thread readingTemps = new Thread(readingTempsMethod);
            readingTemps.Start();

            ThreadStart RFIDTagReading = new ThreadStart(InitRFIDReaderThread);
            Thread readTag = new Thread(RFIDTagReading);
            readTag.Start();

            DispatcherTimer dateTime = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            dateTime.Tick += new EventHandler<object>(Tick);
            dateTime.Start();

            Devices.InitTimers();
            wallpaper.Source = Globals.BackgroundImage;
            navbar.Background = Globals.TranslucentUiColor;
            MainFrame.Navigate(typeof(HomePage));

            Thread.Sleep(2000);
            Devices.ResetDevices();
        }

        private async void InitRFIDReaderThread()
        {
            await Devices.RFIDReader.InitIO();
            while (true)
            {
                if (Devices.RFIDReader.IsTagPresent())
                {
                    var uid = Devices.RFIDReader.ReadUid().ToString();
                    string uidString = uid;
                    if (Globals.RFIDUid == uid)
                    {
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                        () =>
                        {
                            if (Devices.IsAlarmActivated)
                            {
                                if (Devices.AlarmTriggered)
                                {
                                    Globals.CloseOpenedDialog();
                                    Devices.IsAlarmCountDownEnabled = false;
                                }
                                else if (Devices.IsAlarmRinging)
                                {
                                    Globals.CloseOpenedDialog();
                                    Devices.IsAlarmRinging = false;
                                }
                                else
                                {
                                    Devices.IsAlarmActivated = false;
                                    Devices.RFIDSuccess();
                                }
                            }
                            else
                            {
                                Devices.IsAlarmActivated = true;
                                Devices.RFIDSuccess();
                            }
                            if (AlarmPage.Current != null) AlarmPage.Current.AlarmBtnState();
                            HomePage.Current.SetHomeStatus();
                        });
                    }
                    else
                    {
                        Devices.RFIDError();
                        if (Devices.IsAlarmRinging) Devices.IsAlarmRinging = true; ;
                    }

                    Devices.RFIDReader.HaltTag();
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }
        }

        private async void UpdateTempValuesThread()
        {
            int times = 0;
            while (true)
            {
                for (int i = 0; i < Devices.temp_devices.Length; i++)
                {
                    DhtReading reading = await Devices.temp_devices[i].GetReadingAsync().AsTask();
                    if (reading.Temperature != 0 && reading.Humidity != 0)
                    {
                        Globals.dht11_values[i, 0] = reading.Temperature;
                        Globals.dht11_values[i, 1] = reading.Humidity;
                    }
                }

                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                   () => { ShowTempDataOnUI(); });

                if (justOpened && times <= 3)
                {
                    times++;
                    if (times == 3) justOpened = false;
                    Thread.Sleep(8000);
                }
                else
                {
                    Thread.Sleep(Globals.TempTimerInterval);
                }
            }
        }

        private void ShowTempDataOnUI()
        {
            string unit = Globals.TempUnit == "celsius" ? " ºC" : " ºF";
            HomePage.Current.temp1.Text = "Temperatura: " + (Globals.TempUnit == "farenheit" ? Globals.TempToFarenheit(Globals.dht11_values[0, 0]) : Globals.dht11_values[0, 0]) + unit;
            HomePage.Current.hum1.Text = "Humedad: " + Globals.dht11_values[0, 1] + " %";
            HomePage.Current.temp2.Text = "Temperatura: " + (Globals.TempUnit == "farenheit" ? Globals.TempToFarenheit(Globals.dht11_values[1, 0]) : Globals.dht11_values[1, 0]) + unit;
            HomePage.Current.hum2.Text = "Humedad: " + Globals.dht11_values[1, 1] + " %";
            HomePage.Current.temp3.Text = "Temperatura: " + (Globals.TempUnit == "farenheit" ? Globals.TempToFarenheit(Globals.dht11_values[2, 0]) : Globals.dht11_values[2, 0]) + unit;
            HomePage.Current.hum3.Text = "Humedad: " + Globals.dht11_values[2, 1] + " %";
            Debug.WriteLine("DHT11: información actualizada.");
        }

        public void StyleControls()
        {
            if (Globals.UiColor == "UiWhite")
            {
                GoToHomePage.Style = (Style)Application.Current.Resources["GoToHomePageBtnBlack"];
                GoToDevicePage.Style = (Style)Application.Current.Resources["GoToDevicesPageBtnBlack"];
                GoToConfigPage.Style = (Style)Application.Current.Resources["GoToConfigPageBtnBlack"];
            }
            else
            {
                GoToHomePage.Style = (Style)Application.Current.Resources["GoToHomePageBtn"];
                GoToDevicePage.Style = (Style)Application.Current.Resources["GoToDevicesPageBtn"];
                GoToConfigPage.Style = (Style)Application.Current.Resources["GoToConfigPageBtn"];
            }
        }

        public void Tick(object sender, object e)
        {
            time.Text = DateTime.Now.ToString(Globals.DateTimeFormat, CultureInfo.CreateSpecificCulture("es-ES"));
        }

        private void GoToPage_Click(object sender, RoutedEventArgs e)
        {
            string page = (sender as Button).Name;
            switch (page)
            {
                case "GoToHomePage":
                    MainFrame.Navigate(typeof(HomePage), null, new SuppressNavigationTransitionInfo());
                    PageName.Text = "";
                    navbar.Background = Globals.TranslucentUiColor;
                    ShowTempDataOnUI();
                    break;
                case "GoToDevicePage":
                    MainFrame.Navigate(typeof(DevicePage), null, new SuppressNavigationTransitionInfo());
                    navbar.Background = Globals.UiTransparency ? Globals.TranslucentUiColor : Globals.SolidUiColor;
                    PageName.Text = "Dispositivos";
                    break;
                case "GoToConfigPage":
                    MainFrame.Navigate(typeof(ConfigPage), null, new SuppressNavigationTransitionInfo());
                    PageName.Text = "Configuración";
                    break;
            }
        }
    }
}
