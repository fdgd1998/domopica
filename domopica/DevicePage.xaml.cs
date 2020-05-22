using domopica.DevicePages;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace domopica
{
    public sealed partial class DevicePage : Page
    {
        public static DevicePage Current;
        public DevicePage()
        {
            InitializeComponent();
            Current = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Globals.IsTransparencyEnabled(this);
            deviceFrame.Navigate(typeof(GroundFloorPage), null, new SuppressNavigationTransitionInfo());
        }

        private void GoToPage_Click(object sender, RoutedEventArgs e)
        {
            string page = (sender as Button).Name;
            switch (page)
            {
                case "GoToGroundFloorPage":
                    deviceFrame.Navigate(typeof(GroundFloorPage), null, new SuppressNavigationTransitionInfo());
                    break;
                case "GoToFirstFloorPage":
                    deviceFrame.Navigate(typeof(FirstFloorPage), null, new SuppressNavigationTransitionInfo());
                    break;
            }
        }

        public static void SetFloorScheme(Page page)
        {
            string baseURL = "ms-appx:///Assets/Floor/";
            string schemeWhite, schemeBlack;

            if (page is FirstFloorPage)
            {
                schemeWhite = "floor1-White.png";
                schemeBlack = "floor1-black.png";
                if (Globals.UiColor == "UiWhite")
                {
                    if (Globals.UiTransparency) FirstFloorPage.Current.scheme.Source = new BitmapImage(new Uri(baseURL + schemeBlack));
                    else FirstFloorPage.Current.scheme.Source = new BitmapImage(new Uri(baseURL + schemeWhite));
                }
                else
                    FirstFloorPage.Current.scheme.Source = new BitmapImage(new Uri(baseURL + schemeWhite));
            }
            else
            {
                schemeWhite = "floor0-White.png";
                schemeBlack = "floor0-black.png";
                if (Globals.UiColor == "UiWhite")
                {
                    if (Globals.UiTransparency) GroundFloorPage.Current.scheme.Source = new BitmapImage(new Uri(baseURL + schemeBlack));
                    else GroundFloorPage.Current.scheme.Source = new BitmapImage(new Uri(baseURL + schemeWhite));
                }
                else
                    GroundFloorPage.Current.scheme.Source = new BitmapImage(new Uri(baseURL + schemeWhite));
            }
        }

        public static void SetButtonOptions(ToggleButton btn, string type)
        {
            string folder = "ms-appx:///Assets/Icons/32px/";
            string lightOnIcon, lightOffIcon, doorOpenIcon, doorCloseIcon, curtainOpenIcon, curtainCloseIcon;

            if (Globals.UiTransparency)
            {
                lightOnIcon = Globals.UiColor != "UiWhite" ? "light-off-white.png" : "light-off-black.png";
                lightOffIcon = Globals.UiColor != "UiWhite" ? "light-on-white.png" : "light-on-black.png";
                doorOpenIcon = Globals.UiColor != "UiWhite" ? "garage-door-open-white.png" : "garage-door-open-black.png";
                doorCloseIcon = Globals.UiColor != "UiWhite" ? "garage-door-close-white.png" : "garage-door-close-black.png";
                curtainOpenIcon = Globals.UiColor != "UiWhite" ? "curtain-open-white.png" : "curtain-open-black.png";
                curtainCloseIcon = Globals.UiColor != "UiWhite" ? "curtain-close-white.png" : "curtain-close-black.png";
            }
            else
            {
                lightOnIcon = "light-off-white.png";
                lightOffIcon = "light-on-white.png";
                doorOpenIcon = "garage-door-open-white.png";
                doorCloseIcon = "garage-door-close-white.png";
                curtainOpenIcon = "curtain-open-white.png";
                curtainCloseIcon = "curtain-close-white.png";
            }

            if (Globals.UiTransparency && Globals.UiColor == "UiWhite")
            {
                btn.Style = (Style)Application.Current.Resources["DeviceToggleBtnLight"];
            } else
            {
                btn.Style = (Style)Application.Current.Resources["DeviceToggleBtn"];
            }

            foreach (var thing in ((StackPanel)btn.Content).Children)
            {
                if (thing is Image)
                {
                    switch (type)
                    {
                        case "Light":
                            (thing as Image).Source = new BitmapImage(new Uri(folder + ((bool)btn.IsChecked ? lightOnIcon : lightOffIcon)));
                            break;
                        case "Door":
                            (thing as Image).Source = new BitmapImage(new Uri(folder + ((bool)btn.IsChecked ? doorCloseIcon : doorOpenIcon)));
                            break;
                        case "Curtain":
                            (thing as Image).Source = new BitmapImage(new Uri(folder + ((bool)btn.IsChecked ? curtainCloseIcon : curtainOpenIcon)));
                            break;
                    }
                }
                else if (thing is TextBlock)
                {
                    switch (type)
                    {
                        case "Light":
                            (thing as TextBlock).Text = (bool)btn.IsChecked ? "Apagar\nluces" : "Encender\nluces";
                            break;
                        case "Door":
                            (thing as TextBlock).Text = (bool)btn.IsChecked ? "Cerrar\npuerta" : "Abrir\npuerta";
                            break;
                        case "Curtain":
                            (thing as TextBlock).Text = (bool)btn.IsChecked ? "Cerrar\npersianas" : "Abrir\npersianas";
                            break;
                    }
                    if (Globals.UiTransparency && Globals.UiColor == "UiWhite")
                    {
                        (thing as TextBlock).RequestedTheme = ElementTheme.Light;
                    }
                }
            }
        }

        public async void EnableCurtainButton(string btnName)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
            () =>
            {
                Devices.curtainsList[btnName].IsWorking = false;
                switch (btnName)
                {
                    case "living_room_curtain":
                        GroundFloorPage.Current.living_room_curtain.IsEnabled = true;
                        break;
                    case "bedroom_curtain":
                        FirstFloorPage.Current.bedroom_curtain.IsEnabled = true;
                        break;
                    case "bathroom_curtain":
                        FirstFloorPage.Current.bathroom_curtain.IsEnabled = true;
                        break;
                    default:
                        break;
                }
            });
        }

        public async void EnableDoorButton(string btnName)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
            () =>
            {
                Devices.doorsList[btnName].IsWorking = false;
                switch (btnName)
                {
                    case "garage_door":
                        GroundFloorPage.Current.garage_door.IsEnabled = true;
                        break;

                }
            });
        }
    }
}