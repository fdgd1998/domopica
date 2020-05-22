using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class AlarmPage : Page
    {
        public static AlarmPage Current;
        public AlarmPage()
        {
            InitializeComponent();
            Current = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Globals.IsTransparencyEnabled(this);
            SetControls();
        }

        private void SetControls()
        {
            alarm.IsOn = Devices.IsAlarmActivated ? true : false;
            Globals.IsTransparencyEnabled(this, ConfigPage.Current.sidebar, MainPage.Current.navbar);
            EnablePinChange();
        }

        private void EnablePinChange()
        {
            currentPIN.IsEnabled = alarm.IsOn ? true : false;
            newPIN.IsEnabled = alarm.IsOn ? true : false;
            newPIN2.IsEnabled = alarm.IsOn ? true : false;
            changePIN.IsEnabled = alarm.IsOn ? true : false;
        }

        private void AlarmBtn_Toggled(object sender, RoutedEventArgs e)
        {

            if (alarm.IsOn) Devices.IsAlarmActivated = true;
            else Devices.IsAlarmActivated = false;
            AlarmBtnState();
        }

        public void AlarmBtnState()
        {
            alarm.IsOn = Devices.IsAlarmActivated ? true : false;
            EnablePinChange();
        }

        private void changePIN_Click(object sender, RoutedEventArgs e)
        {
            string currentPasswd = currentPIN.Password;
            string newPasswd = newPIN.Password;
            string newPasswd2 = newPIN2.Password;

            if (currentPasswd != Globals.AlarmPIN)
            {
                currentPIN.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            } 
            else
            {
                if (newPasswd != newPasswd2)
                {
                    newPIN.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                    newPIN2.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                } 
                else
                {
                    Globals.AlarmPIN = newPasswd;
                    string image = Globals.UiColor == "UiWhite" ? "ms-appx:///Assets/Icons/tick-black.png" : "ms-appx:///Assets/Icons/tick.png";
                    currentPIN.Password = "";
                    newPIN.Password = "";
                    newPIN2.Password = "";
                    changeSuccess.Source = new BitmapImage(new Uri(image));
                    currentPIN.BorderBrush = new SolidColorBrush(Color.FromArgb(115, 230, 230, 230));
                    newPIN.BorderBrush = new SolidColorBrush(Color.FromArgb(115, 230, 230, 230));
                    newPIN2.BorderBrush = new SolidColorBrush(Color.FromArgb(115, 230, 230, 230));
                };
            }
        }
    }
}
