using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Gpio;
using Windows.UI;

// La plantilla de elemento del cuadro de diálogo de contenido está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace domopica
{
    public sealed partial class AlarmTriggered : ContentDialog
    {
        public static AlarmTriggered Current;
        private string device;
        public AlarmTriggered(string sender)
        {
            InitializeComponent();
            Current = this;
            device = sender;
            activatedBy.Text = "El sensor de la habitación \"" + sender + "\" ha activado la alarma.";
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (passwd.Password.ToString() == Globals.AlarmPIN)
            {
                Hide();
                Devices.IsAlarmRinging = false;
                passwd.Password = "";
            } 
            else
            {
                Hide();
                Globals.OpenDialog(device, "alarm");
            }
        }

    }
}
