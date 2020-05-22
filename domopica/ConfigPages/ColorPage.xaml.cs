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
    public sealed partial class ColorPage : Page
    {
        public ColorPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Globals.UiTransparency) transparentchkbx.IsChecked = true;
            else transparentchkbx.IsChecked = false;
            if (Globals.UiColor == "UiWhite" && Globals.UiTransparency) RequestedTheme = ElementTheme.Light;
            StyleControls();
            wallpaperpreview.Source = Globals.BackgroundImage;
            LoadSelectedColor();
            LoadSelectedWallpaper();
            Globals.IsTransparencyEnabled(this);
        }

        private void StyleControls()
        {
            if (Globals.UiColor == "UiWhite" && !Globals.UiTransparency) transparentchkbx.Style = (Style)Application.Current.Resources["CheckBoxTransparent"];
            else if (Globals.UiColor == "UiWhite" && Globals.UiTransparency) transparentchkbx.Style = (Style)Application.Current.Resources["CheckBoxDark"];
            else transparentchkbx.Style = (Style)Application.Current.Resources["CheckBoxTransparent"];
            MainPage.Current.StyleControls(); 
        }

        private void LoadSelectedColor ()
        {
            switch (Globals.UiColor)
            {
                case "UiRed":
                    UiRed.IsChecked = true;
                    break;
                case "UiPink":
                    UiPink.IsChecked = true;
                    break;
                case "UiGreen":
                    UiGreen.IsChecked = true;
                    break;
                case "UiBlue":
                    UiBlue.IsChecked = true;
                    break;
                case "UiDarkBlue":
                    UiDarkBlue.IsChecked = true;
                    break;
                case "UiDarkGray":
                    UiDarkGray.IsChecked = true;
                    break;
                case "UiOrange":
                    UiOrange.IsChecked = true;
                    break;
                case "UiPurple":
                    UiPurple.IsChecked = true;
                    break;
                case "UiWhite":
                    UiWhite.IsChecked = true;
                    break;
                case "UiBlack":
                    UiBlack.IsChecked = true;
                    break;
            }
        }

        private void LoadSelectedWallpaper()
        {
            switch (Globals.SourceImage)
            {
                case "wallpaper-1.jpg":
                    wallpaper1.IsChecked = true;
                    break;
                case "wallpaper-2.jpg":
                    wallpaper2.IsChecked = true;
                    break;
                case "wallpaper-3.jpg":
                    wallpaper3.IsChecked = true;
                    break;
                case "wallpaper-4.jpg":
                    wallpaper4.IsChecked = true;
                    break;
                case "wallpaper-5.jpg":
                    wallpaper5.IsChecked = true;
                    break;
                case "wallpaper-6.jpg":
                    wallpaper6.IsChecked = true;
                    break;
                case "wallpaper-7.jpg":
                    wallpaper7.IsChecked = true;
                    break;
                case "wallpaper-8.jpg":
                    wallpaper8.IsChecked = true;
                    break;
            }
        }

        private void SetTransparency_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (chk.IsChecked == true) Globals.UiTransparency = true;
            else Globals.UiTransparency = false;
            ChangeColors();
            StyleControls();
        }

        private void ColorRadioBtn_Click(object sender, RoutedEventArgs e)
        {
            RadioButton btn = sender as RadioButton;
            Globals.UiColor = btn.Name;
            ChangeColors();
            StyleControls();
            ConfigPage.Current.SetIcons();
        }

        private void ChangeColors()
        {
            switch (Globals.UiColor)
            {
                case "UiRed":
                    Globals.TranslucentUiColor = new SolidColorBrush(Color.FromArgb(127, 176, 0, 0));
                    Globals.SolidUiColor = new SolidColorBrush(Color.FromArgb(255, 176, 0, 0));
                    break;
                case "UiOrange":
                    Globals.TranslucentUiColor = new SolidColorBrush(Color.FromArgb(127, 234, 85, 0));
                    Globals.SolidUiColor = new SolidColorBrush(Color.FromArgb(255, 234, 85, 0));
                    break;
                case "UiPink":
                    Globals.TranslucentUiColor = new SolidColorBrush(Color.FromArgb(127, 234, 0, 213));
                    Globals.SolidUiColor = new SolidColorBrush(Color.FromArgb(255, 234, 0, 213));
                    break;
                case "UiGreen":
                    Globals.TranslucentUiColor = new SolidColorBrush(Color.FromArgb(127, 48, 106, 14));
                    Globals.SolidUiColor = new SolidColorBrush(Color.FromArgb(255, 48, 106, 14));
                    break;
                case "UiBlue":
                    Globals.TranslucentUiColor = new SolidColorBrush(Color.FromArgb(127, 0, 109, 182));
                    Globals.SolidUiColor = new SolidColorBrush(Color.FromArgb(255, 0, 109, 182));
                    break;
                case "UiPurple":
                    Globals.TranslucentUiColor = new SolidColorBrush(Color.FromArgb(127, 99, 47, 120));
                    Globals.SolidUiColor = new SolidColorBrush(Color.FromArgb(255, 99, 47, 120));
                    break;
                case "UiWhite":
                    Globals.TranslucentUiColor = new SolidColorBrush(Color.FromArgb(127, 255, 255, 255));
                    Globals.SolidUiColor = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    break;
                case "UiBlack":
                    Globals.TranslucentUiColor = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
                    Globals.SolidUiColor = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    break;
                case "UiDarkGray":
                    Globals.TranslucentUiColor = new SolidColorBrush(Color.FromArgb(127, 43, 43, 43));
                    Globals.SolidUiColor = new SolidColorBrush(Color.FromArgb(255, 43, 43, 43));
                    break;
                case "UiDarkBlue":
                    Globals.TranslucentUiColor = new SolidColorBrush(Color.FromArgb(127, 0, 55, 135));
                    Globals.SolidUiColor = new SolidColorBrush(Color.FromArgb(255, 0, 55, 135));
                    break;
            }
            Globals.IsTransparencyEnabled(this, ConfigPage.Current.sidebar, MainPage.Current.navbar);
        }

        private void WallpaperRadioBtn_Click(object sender, RoutedEventArgs e)
        {
            RadioButton btn = sender as RadioButton;
            string name = btn.Name;

            switch (name)
            {
                case "wallpaper1":
                    Globals.SourceImage = "wallpaper-1.jpg";
                    break;
                case "wallpaper2":
                    Globals.SourceImage = "wallpaper-2.jpg";
                    break;
                case "wallpaper3":
                    Globals.SourceImage = "wallpaper-3.jpg";
                    break;
                case "wallpaper4":
                    Globals.SourceImage = "wallpaper-4.jpg";
                    break;
                case "wallpaper5":
                    Globals.SourceImage = "wallpaper-5.jpg";
                    break;
                case "wallpaper6":
                    Globals.SourceImage = "wallpaper-6.jpg";
                    break;
                case "wallpaper7":
                    Globals.SourceImage = "wallpaper-7.jpg";
                    break;
                case "wallpaper8":
                    Globals.SourceImage = "wallpaper-8.jpg";
                    break;
            }
            Globals.BackgroundImage = new BitmapImage(new Uri(Globals.WallpaperFolder + Globals.SourceImage));
            MainPage.Current.wallpaper.Source = Globals.BackgroundImage;
            wallpaperpreview.Source = Globals.BackgroundImage;
        }
    }
}
