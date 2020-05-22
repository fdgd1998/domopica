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
    public sealed partial class HelpPage : Page
    {
        public static HelpPage Current;
        public HelpPage()
        {
            InitializeComponent();
            Current = this;
            HelpFrame.Navigate(typeof(MainHelpPage));
            SetIcons();
        }

        public void SetIcons()
        {
            bool aux = Globals.UiColor == "UiWhite" ? true : false;
            paletteicon.Source = aux ? new BitmapImage(new Uri("ms-appx:///Assets/Icons/palette-black.png")) : new BitmapImage(new Uri("ms-appx:///Assets/Icons/palette.png"));
            infoicon.Source = aux ? new BitmapImage(new Uri("ms-appx:///Assets/Icons/info-black.png")) : new BitmapImage(new Uri("ms-appx:///Assets/Icons/info.png"));
            lighticon.Source = aux ? new BitmapImage(new Uri("ms-appx:///Assets/Icons/light-black.png")) : new BitmapImage(new Uri("ms-appx:///Assets/Icons/light.png"));
            dooricon.Source = aux ? new BitmapImage(new Uri("ms-appx:///Assets/Icons/door-black.png")) : new BitmapImage(new Uri("ms-appx:///Assets/Icons/door.png"));
            thermometericon.Source = aux ? new BitmapImage(new Uri("ms-appx:///Assets/Icons/thermometer-black.png")) : new BitmapImage(new Uri("ms-appx:///Assets/Icons/thermometer.png"));
            uiicon.Source = aux ? new BitmapImage(new Uri("ms-appx:///Assets/Icons/ui-black.png")) : new BitmapImage(new Uri("ms-appx:///Assets/Icons/ui.png"));
        }

        private void GoToPage_Click(object sender, RoutedEventArgs e)
        {
            string page = (sender as Button).Name;
            switch (page)
            {
                case "GoToAboutPage":
                    HelpFrame.Navigate(typeof(AboutPage), null, new SuppressNavigationTransitionInfo());
                    break;
                case "GoToColorHelpPage":
                    HelpFrame.Navigate(typeof(ColorHelpPage), null, new SuppressNavigationTransitionInfo());
                    break;
                case "GoToUiHelpPage":
                    HelpFrame.Navigate(typeof(UiHelpPage), null, new SuppressNavigationTransitionInfo());
                    break;
                case "GoToLightHelpPage":
                    HelpFrame.Navigate(typeof(LightHelpPage), null, new SuppressNavigationTransitionInfo());
                    break;
                case "GoToDoorBlindHelpPage":
                    HelpFrame.Navigate(typeof(DoorBlindHelpPage), null, new SuppressNavigationTransitionInfo());
                    break;
                case "GoToTemperatureHelpPage":
                    HelpFrame.Navigate(typeof(TemperatureHelpPage), null, new SuppressNavigationTransitionInfo());
                    break;
            }
        }
    }
}
