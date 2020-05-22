using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace domopica
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>

    public sealed partial class HomePage : Page
    {
        public static HomePage Current;
        public HomePage()
        {
            InitializeComponent();
            Current = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Background = Globals.TranslucentUiColor;
            SetHomeStatus();
        }

        public void SetHomeStatus()
        {
            Debug.WriteLine("IsAlarmActivated: " + Devices.IsAlarmActivated);
            Debug.WriteLine("IsLightSensorEnabled: " + Devices.IsLightSensorEnabled);
            string dir = "ms-appx:///Assets/Icons/";
            if (Globals.UiColor == "UiWhite")
            {
                RequestedTheme = ElementTheme.Light;
                alarmStatusIcon.Source = Devices.IsAlarmActivated ?
                    new BitmapImage(new Uri(dir + "success-dark.png"))
                    : new BitmapImage(new Uri(dir + "cancel-dark.png"));
                lightSensorStatusIcon.Source = Devices.IsLightSensorEnabled ?
                    new BitmapImage(new Uri(dir + "success-dark.png"))
                    : new BitmapImage(new Uri(dir + "cancel-dark.png"));
            }
            else
            {
                RequestedTheme = ElementTheme.Dark;
                alarmStatusIcon.Source = Devices.IsAlarmActivated ?
                    new BitmapImage(new Uri(dir + "success.png"))
                    : new BitmapImage(new Uri(dir + "cancel.png"));
                lightSensorStatusIcon.Source = Devices.IsLightSensorEnabled ?
                    new BitmapImage(new Uri(dir + "success.png"))
                    : new BitmapImage(new Uri(dir + "cancel.png"));
            }

            alarmStatusText.Text = Devices.IsAlarmActivated ? "La alarma está activada" : "La alarma está desactivada";
            lightSensorStatusText.Text = Devices.IsLightSensorEnabled ? "El sensor de luz ambiente está activado" : "El sensor de luz ambiente está desactivado";
        }
    }
}
