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
    public sealed partial class AmbientLightPage : Page
    {
        public AmbientLightPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SetButtonStatus();
            EnableButtons();
            Globals.SetCheckBoxStyle(gridButtons);
            Globals.IsTransparencyEnabled(this, ConfigPage.Current.sidebar, MainPage.Current.navbar);
        }

        private void EnableButtons()
        {
            foreach (CheckBox item in gridButtons.Children)
                item.IsEnabled = Devices.IsLightSensorEnabled ? true : false;
            saveDevices.IsEnabled = Devices.IsLightSensorEnabled ? true : false;
        }

        private void SetButtonStatus()
        {
            lightSensor.IsOn = Devices.IsLightSensorEnabled ? true : false;
            foreach (string name in Devices.lightSensorDevices)
            {
                foreach(var item in gridButtons.Children)
                {
                    if ((item as CheckBox).Name == name)
                        (item as CheckBox).IsChecked = true;
                }
            }
        }

        private void SaveDevices_Click(object sender, RoutedEventArgs e)
        {
            Devices.lightSensorDevices.Clear();
            foreach (CheckBox thing in gridButtons.Children)
            {
                CheckBox btn = thing as CheckBox;
                if ((bool)btn.IsChecked)
                    Devices.lightSensorDevices.Add(btn.Name);
            }
            string image = Globals.UiColor == "UiWhite" ? "ms-appx:///Assets/Icons/tick-black.png" : "ms-appx:///Assets/Icons/tick.png";
            changeSuccess.Source = new BitmapImage(new Uri(image));
            Devices.AutoLights();
        }

        private void LightSensor_Toggled(object sender, RoutedEventArgs e)
        {
            Devices.IsLightSensorEnabled = (sender as ToggleSwitch).IsOn ? true : false;
            EnableButtons();
            Devices.AutoLights();
        }
    }
}