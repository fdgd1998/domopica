using Windows.Devices.I2c;

namespace domopica
{
    public class Curtain
    {
        private bool Opened { get; set; } = true;
        public bool IsWorking { get; set; } = false;
        private I2cDevice device;
        public bool IsOpened
        {
            get
            {
                return Opened;
            }

            set
            {
                if (Opened != value)
                {
                    Opened = value;
                    if (Opened) Close();
                    else Open();
                }
            }
        }

        public Curtain(I2cDevice device)
        {
            this.device = device;
            Opened = false;
        }

        private void Open()
        {
            device.Write(new byte[] { 0 });
            IsWorking = true;
        }

        private void Close()
        {
            device.Write(new byte[] { 1 });
            IsWorking = true;
        }
    }
}
