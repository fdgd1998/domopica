using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using ABElectronics_Win10IOT_Libraries;

namespace domopica
{
    public class Light
    {
        private byte ledPin;
        private IOPi bus;
        private bool ledOn = false;
        public bool LightOn { 
            get { return ledOn; } 
            set
            {
                if (value != ledOn)
                {
                    ledOn = value;
                    if (ledOn) PowerOn();
                    else PowerOff();
                }
            }
        }

        public Light(IOPi bus, byte pin)
        {
            ledPin = pin;
            this.bus = bus;
            //ledPin.Write(GpioPinValue.Low);
            ledOn = false;
        }

        private void PowerOn()
        {
            bus.WritePin(ledPin, true);
            //ledPin.Write(GpioPinValue.High);
        }

        private void PowerOff()
        {
            bus.WritePin(ledPin, false);
            //ledPin.Write(GpioPinValue.Low);
        }
    }
}
