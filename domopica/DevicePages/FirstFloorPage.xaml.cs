﻿using System;
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
using Windows.UI;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace domopica.DevicePages
{
    public sealed partial class FirstFloorPage : Page
    {
        public static FirstFloorPage Current;
        public FirstFloorPage()
        {
            InitializeComponent();
            Current = this; 
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SetLightButtonStatus();
            SetCurtainButtonStatus();
            DevicePage.SetFloorScheme(Current);
            DevicePage.Current.pageText.Text = "Primera planta";
            Background = Globals.UiTransparency ? new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)) : new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        }

            public void SetLightButtonStatus()
        {
            foreach (KeyValuePair<string, Light> item in Devices.lightsList)
            {
                if (FindName(item.Key) is ToggleButton btn)
                {
                    btn.IsChecked = item.Value.LightOn ? true : false;
                    DevicePage.SetButtonOptions(btn, "Light");
                }
            }
        }

        private void SetCurtainButtonStatus()
        {
            foreach (KeyValuePair<string, Curtain> item in Devices.curtainsList)
            {
                if (FindName(item.Key) is ToggleButton btn)
                {
                    btn.IsChecked = item.Value.IsOpened ? true : false;
                    btn.IsEnabled = item.Value.IsWorking ? false : true;
                    DevicePage.SetButtonOptions(btn, "Curtain");
                }
            }
        }

        private void LightButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton btn = sender as ToggleButton;
            string name = btn.Name;
            Devices.ChangeLightState(name);
            DevicePage.SetButtonOptions(btn, "Light");
        }

        private void CurtainButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton btn = sender as ToggleButton;
            btn.IsEnabled = false;
            string name = btn.Name;
            Devices.ChangeCurtainState(name);
            DevicePage.SetButtonOptions(btn, "Curtain");
        }
    }
}
