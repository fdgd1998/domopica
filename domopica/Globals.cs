using domopica.DevicePages;
using Mfrc522Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Windows.Devices.Gpio;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace domopica
{
    static class Globals
    {
        public static Dictionary<string, AutomatizationTask> automatizatedTasks = new Dictionary<string, AutomatizationTask>();
        /*{
            {"autoTask1", new AutomatizationTask(13,16,13,17)},
            {"autoTask2", new AutomatizationTask()},
            {"autoTask3", new AutomatizationTask()}
        };*/

        public static bool ShowDayWeek { get; set; } = true;
        public static bool ShowFullYear { get; set; } = true;
        public static bool ShowSeconds { get; set; } = true;
        public static string DateTimeFormat { get; set; } = "dddd, dd/MM/yyyy HH:mm:ss";

        public static bool UiTransparency { get; set; } = true;
        public static string UiColor { get; set; } = "UiDarkGray";
        public static SolidColorBrush TranslucentUiColor { get; set; } = new SolidColorBrush(Color.FromArgb(127, 43, 43, 43));
        public static SolidColorBrush SolidUiColor { get; set; } = new SolidColorBrush(Color.FromArgb(255, 43, 43, 43));
        public static SolidColorBrush FrameBackground { get; set; } = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
       
        public static double[,] dht11_values = new double[3, 2];

        public static string AlarmPIN { get; set; } = "1234";
        public static string RFIDUid { get; set; } = "d93bb389";

        public static string FrecuencyInterval { get; set; } = "1 minuto";
        public static string TempUnit { get; set; } = "celsius";

        public static string WallpaperFolder { get; set; } = "ms-appx:///Assets/Wallpapers/";
        public static string SourceImage { get; set; } = "wallpaper-1.jpg";
        public static BitmapImage BackgroundImage { get; set; } = new BitmapImage(new Uri(WallpaperFolder + SourceImage));

        public static int TempTimerInterval { get; set; } = 60000;
        public static int SelectedDateOption { get; set; } = 0;

        public static async void CloseOpenedDialog()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
            () =>
            {
                var dialogs = VisualTreeHelper.GetOpenPopups(Window.Current);

                foreach (var dialog in dialogs)
                    if (dialog.Child is AlarmCountDown || dialog.Child is AlarmTriggered)
                        (dialog.Child as ContentDialog).Hide();
            });
        }

        public static async void OpenDialog(string sender, string type)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
            async () =>
            {
                var dialogs = VisualTreeHelper.GetOpenPopups(Window.Current);
                int encountered = 0;
                foreach (var dialog in dialogs)
                    if (dialog.Child is AlarmCountDown || dialog.Child is AlarmTriggered)
                        encountered++;

                if (encountered == 0)
                {
                    ContentDialog contentDialog = null;
                    switch (type)
                    {
                        case "countdown":
                            Devices.AlarmTriggered = true;
                            contentDialog = new AlarmCountDown(sender);
                            break;
                        case "alarm":
                            Devices.IsAlarmRinging = true;
                            contentDialog = new AlarmTriggered(sender);
                            break;
                    }

                    if (Devices.AlarmCountDownFirstTime == true) Devices.AlarmCountDownFirstTime = false;
                    await contentDialog.ShowAsync();
                }
            });
        }

        public static void SetCheckBoxStyle(StackPanel panel)
        {
            foreach (CheckBox item in panel.Children)
            {
                if (UiColor == "UiWhite" && !UiTransparency) item.Style = (Style)Application.Current.Resources["CheckBoxTransparent"];
                else if (UiColor == "UiWhite" && UiTransparency) item.Style = (Style)Application.Current.Resources["CheckBoxDark"];
                else item.Style = (Style)Application.Current.Resources["CheckBoxTransparent"];
            }
        }

        public static void IsTransparencyEnabled(Page page)
        {
            page.Background = UiTransparency ? TranslucentUiColor : new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            SetUiTheme(page);
        }

        public static void IsTransparencyEnabled(Page page, StackPanel panel, Grid navbar)
        {

            page.Background = UiTransparency ? TranslucentUiColor : new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            panel.Background = UiTransparency ? TranslucentUiColor : SolidUiColor;
            navbar.Background = UiTransparency ? TranslucentUiColor : SolidUiColor;
            SetUiTheme(page, panel, navbar);
        }

        public static void SetUiTheme(Page page, StackPanel panel, Grid navbar)
        {
            if (UiColor == "UiWhite")
            {
                page.RequestedTheme = UiTransparency ? ElementTheme.Light : ElementTheme.Dark;
                panel.RequestedTheme = ElementTheme.Light;
                navbar.RequestedTheme = ElementTheme.Light;
            }
            else
            {
                page.RequestedTheme = ElementTheme.Dark;
                panel.RequestedTheme = ElementTheme.Dark;
                navbar.RequestedTheme = ElementTheme.Dark;
            }
        }

        public static void SetUiTheme(Page page)
        {
            if (UiColor == "UiWhite")
            {
                page.RequestedTheme = UiTransparency ? ElementTheme.Light : ElementTheme.Dark;
            }
            else
            {
                page.RequestedTheme = ElementTheme.Dark;
            }
        }

        public static double TempToFarenheit(double celsius)
        {
            return 32 + 1.8 * celsius;
        }
    }
}
