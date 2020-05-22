using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento del cuadro de diálogo de contenido está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace domopica
{
    public sealed partial class AlarmCountDown : ContentDialog
    {
        public static AlarmCountDown Current;
        public string device;
        public AlarmCountDown(string sender)
        {
            InitializeComponent();
            Current = this;
            device = sender;
            activatedBy.Text = "El sensor de la habitación \"" + device + "\" ha detectado una intrusión. La alarma se activará en 10 segundos.";
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (passwd.Password.ToString() == Globals.AlarmPIN)
            {
                Hide();
                Devices.AlarmTriggered = false;
                passwd.Password = "";
            }
            else
            {
                Hide();
                Globals.OpenDialog(device, "countdown");
            }
        }
    }
}
