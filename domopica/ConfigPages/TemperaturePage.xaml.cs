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

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace domopica
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class TemperaturePage : Page
    {
        public TemperaturePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Globals.IsTransparencyEnabled(this);
            StyleControls();
            SetControls();
        }

        private void SetControls()
        {
            string[] frecuencyvalues = new string[]
            {
                "1 minuto",
                "2 minutos",
                "10 minutos"
            };

            frecuency.Items.Clear();

            for (int i = 0; i < frecuencyvalues.Length; i++)
            {
                frecuency.Items.Add(frecuencyvalues[i]);
            }

            switch (Globals.FrecuencyInterval)
            {
                case "1 minuto":
                    frecuency.SelectedIndex = 0;
                    break;
                case "2 minutos":
                    frecuency.SelectedIndex = 1;
                    break;
                case "10 minutos":
                    frecuency.SelectedIndex = 2;
                    break;
            }

            switch (Globals.TempUnit)
            {
                case "celsius":
                    celsius.IsChecked = true;
                    break;
                case "farenheit":
                    farenheit.IsChecked = true;
                    break;
            }
        }

        private void StyleControls()
        {
            if (Globals.UiColor == "UiWhite" && !Globals.UiTransparency)
            {
                celsius.Style = (Style)Application.Current.Resources["RadioBtnWhite"];
                farenheit.Style = (Style)Application.Current.Resources["RadioBtnWhite"];
            }  
            else if (Globals.UiColor == "UiWhite" && Globals.UiTransparency)
            {
                celsius.Style = (Style)Application.Current.Resources["RadioBtnBlack"];
                farenheit.Style = (Style)Application.Current.Resources["RadioBtnBlack"];
            }
            else
            {
                celsius.Style = (Style)Application.Current.Resources["RadioBtnWhite"];
                farenheit.Style = (Style)Application.Current.Resources["RadioBtnWhite"];
            }
        }

        private void SetTempUnit_Click(object sender, RoutedEventArgs e)
        {
            RadioButton btn = sender as RadioButton;
            string name = btn.Name;
            switch (name)
            {
                case "celsius":
                    Globals.TempUnit = "celsius";
                    break;
                case "farenheit":
                    Globals.TempUnit = "farenheit";
                    break;
            }
        }

        private void FrecuencyIntervalChanged_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedvalue = (sender as ComboBox).SelectedValue.ToString();
            switch (selectedvalue)
            {
                case "1 minuto":
                    Globals.TempTimerInterval = 60000;
                    Globals.FrecuencyInterval = "1 minuto";
                    break;
                case "2 minutos":
                    Globals.TempTimerInterval = 120000;
                    Globals.FrecuencyInterval = "2 minutos";
                    break;
                case "10 minutos":
                    Globals.TempTimerInterval = 600000;
                    Globals.FrecuencyInterval = "10 minutos";
                    break;
            }
        }
    }
}
