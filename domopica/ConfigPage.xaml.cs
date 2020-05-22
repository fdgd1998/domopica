using domopica.ConfigPages;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace domopica
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class ConfigPage : Page
    {
        public static ConfigPage Current;
        public ConfigPage()
        {
            InitializeComponent();
            Current = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ConfigFrame.Navigate(typeof(MainConfigPage));
            SetIcons();
            MainPage.Current.StyleControls();
        }

        public void SetIcons()
        {
            bool aux = Globals.UiColor == "UiWhite" ? true : false;
            reseticon.Source = aux ? new BitmapImage(new Uri("ms-appx:///Assets/Icons/reboot-black.png")) : new BitmapImage(new Uri("ms-appx:///Assets/Icons/reboot-white.png"));
            paletteicon.Source = aux ? new BitmapImage(new Uri("ms-appx:///Assets/Icons/palette-black.png")) : new BitmapImage(new Uri("ms-appx:///Assets/Icons/palette.png"));
            datetimeicon.Source = aux ? new BitmapImage(new Uri("ms-appx:///Assets/Icons/clock-black.png")) : new BitmapImage(new Uri("ms-appx:///Assets/Icons/clock.png"));
            lighticon.Source = aux ? new BitmapImage(new Uri("ms-appx:///Assets/Icons/light-black.png")) : new BitmapImage(new Uri("ms-appx:///Assets/Icons/light.png"));
            dooricon.Source = aux ? new BitmapImage(new Uri("ms-appx:///Assets/Icons/alarm-black.png")) : new BitmapImage(new Uri("ms-appx:///Assets/Icons/alarm.png"));
            thermometericon.Source = aux ? new BitmapImage(new Uri("ms-appx:///Assets/Icons/thermometer-black.png")) : new BitmapImage(new Uri("ms-appx:///Assets/Icons/thermometer.png"));
            automatizationicon.Source = aux ? new BitmapImage(new Uri("ms-appx:///Assets/Icons/32px/task-planning-black.png")) : new BitmapImage(new Uri("ms-appx:///Assets/Icons/32px/task-planning-white.png"));
        }

        private void GoToPage_Click(object sender, RoutedEventArgs e)
        {
            string page = (sender as Button).Name;
            switch (page)
            {
                case "GoToColorPage":
                    ConfigFrame.Navigate(typeof(ColorPage), null, new SuppressNavigationTransitionInfo());
                    break;
                case "GoToAlarmPage":
                    ConfigFrame.Navigate(typeof(AlarmPage), null, new SuppressNavigationTransitionInfo());
                    break;
                case "GoToAmbientLightPage":
                    ConfigFrame.Navigate(typeof(AmbientLightPage), null, new SuppressNavigationTransitionInfo());
                    break;
                case "GoToTemperaturePage":
                    ConfigFrame.Navigate(typeof(TemperaturePage), null, new SuppressNavigationTransitionInfo());
                    break;
                case "GoToDateTimePage":
                    ConfigFrame.Navigate(typeof(DateTimePage), null, new SuppressNavigationTransitionInfo());
                    break;
                case "GoToResetPage":
                    ConfigFrame.Navigate(typeof(ResetPage), null, new SuppressNavigationTransitionInfo());
                    break;
                case "GoToAutomatizationPage":
                    ConfigFrame.Navigate(typeof(AutomatizationPage), null, new SuppressNavigationTransitionInfo());
                    break;
            }
        }
    }
}
