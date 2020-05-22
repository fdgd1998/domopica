using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public sealed partial class DateTimePage : Page
    {
        public DateTimePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SetDateTimeModel();
            SetCheckBoxOptions();
            StyleControls();
            Globals.IsTransparencyEnabled(this);
        }

        private void StyleControls()
        {
            if (Globals.UiColor == "UiWhite" && !Globals.UiTransparency)
            {
                yearchkbx.Style = (Style)Application.Current.Resources["CheckBoxTransparent"];
                weekdaychkbx.Style = (Style)Application.Current.Resources["CheckBoxTransparent"];
                secondschkbx.Style = (Style)Application.Current.Resources["CheckBoxTransparent"];
            }
            else if (Globals.UiColor == "UiWhite" && Globals.UiTransparency)
            {
                yearchkbx.Style = (Style)Application.Current.Resources["CheckBoxDark"];
                weekdaychkbx.Style = (Style)Application.Current.Resources["CheckBoxDark"];
                secondschkbx.Style = (Style)Application.Current.Resources["CheckBoxDark"];
            }
            else 
            {
                yearchkbx.Style = (Style)Application.Current.Resources["CheckBoxTransparent"];
                weekdaychkbx.Style = (Style)Application.Current.Resources["CheckBoxTransparent"];
                secondschkbx.Style = (Style)Application.Current.Resources["CheckBoxTransparent"];
            }
            
        }

        private void SetDateTimeModel()
        {
            string[] dateTimeItems;
            if (Globals.ShowFullYear)
            {
                dateTimeItems = new string[]
                {
                    "dd/MM/yyyy HH:mm",
                    "dd MMMM yyyy HH:mm"
                };
            }
            else
            {
                dateTimeItems = new string[]
                {
                    "dd/MM/yy HH:mm",
                };
            }

            datetimeformat.Items.Clear();
            for (int i = 0; i < dateTimeItems.Length; i++)
            {
                datetimeformat.Items.Add(dateTimeItems[i]);
            }
            //Debug.Write("Info: selected index: " + Globals.selectedDateOption);
            if (!Globals.ShowFullYear) datetimeformat.SelectedIndex = 0;
            else datetimeformat.SelectedIndex = Globals.SelectedDateOption;
            
        }

        private void SetCheckBoxOptions()
        {
            if (Globals.ShowDayWeek) weekdaychkbx.IsChecked = true;
            else weekdaychkbx.IsChecked = false;

            if (Globals.ShowFullYear) yearchkbx.IsChecked = true;
            else yearchkbx.IsChecked = false;

            if (Globals.ShowSeconds) secondschkbx.IsChecked = true;
            else secondschkbx.IsChecked = false;
        }

        private void Datetimechkbx_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            string name = (sender as CheckBox).Name;
            switch (name)
            {
                case "weekdaychkbx":
                    Globals.ShowDayWeek = chk.IsChecked == true ? true : false;
                    break;
                case "yearchkbx":
                    Globals.ShowFullYear = chk.IsChecked == true ? true : false;
                    break;
                case "secondschkbx":
                    Globals.ShowSeconds = chk.IsChecked == true ? true : false;
                    break;
            }
            SetDateTimeModel();
            SetDateTimeFormat();
        }

        private string AddDayWeekSeconds(string format)
        {
            if (Globals.ShowDayWeek) format = "dddd, " + format;
            if (Globals.ShowSeconds) format += ":ss";
            return format;
        }

        private void SetDateTimeFormat()
        {
            ComboBox cbox = datetimeformat;
            if (cbox.SelectedValue != null)
            {
                string format = cbox.SelectedValue.ToString();
                format = AddDayWeekSeconds(format);
                Globals.DateTimeFormat = format;
                Globals.SelectedDateOption = cbox.SelectedIndex;
                //Debug.Write("Info: selected index: " + Globals.selectedDateOption);
            }
        }

        private void Datetimeformat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetDateTimeFormat();
        }
    }
}
