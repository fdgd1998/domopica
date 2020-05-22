using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
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

namespace domopica.ConfigPages
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class ResetPage : Page
    {
        public ResetPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Globals.IsTransparencyEnabled(this);
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            Devices.ResetDevices();
            foreach (KeyValuePair<string, Curtain> item in Devices.curtainsList)
            {
                Devices.curtainsList[item.Key].IsWorking = false;
                Devices.curtainsList[item.Key].IsOpened = false;
            }
                

            foreach (KeyValuePair<string, Door> item in Devices.doorsList)
            {
                Devices.doorsList[item.Key].IsWorking = false;
                Devices.doorsList[item.Key].IsOpened = false;
            }
            string image = Globals.UiColor == "UiWhite" ? "ms-appx:///Assets/Icons/tick-black.png" : "ms-appx:///Assets/Icons/tick.png";
            resetSuccess.Source = new BitmapImage(new Uri(image));
        }
    }
}
