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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace domopica
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainConfigPage : Page
    {
        public MainConfigPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Globals.IsTransparencyEnabled(this, ConfigPage.Current.sidebar, MainPage.Current.navbar);
            if (Globals.UiColor == "UiWhite")
            {
                if (Globals.UiTransparency) mainicon.Source = new BitmapImage(new Uri("ms-appx:///Assets/Icons/settings-black.png"));
                else mainicon.Source = new BitmapImage(new Uri("ms-appx:///Assets/Icons/settings.png"));
            }
            else mainicon.Source = new BitmapImage(new Uri("ms-appx:///Assets/Icons/settings.png"));

            ConfigPage.Current.SetIcons();
        }
    }
}
