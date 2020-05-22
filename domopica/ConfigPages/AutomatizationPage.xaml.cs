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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace domopica.ConfigPages
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class AutomatizationPage : Page
    {
        public AutomatizationPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Globals.IsTransparencyEnabled(this);
            createTaskBtn.IsEnabled = IsTaskListFull() ? false : true;
            if (Globals.automatizatedTasks.Count > 0)
            {
                foreach (KeyValuePair<string, AutomatizationTask> item in Globals.automatizatedTasks)
                {
                    StackPanel mainStackPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Height = 50,
                        Width = 712,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Name = item.Key
                        
                    };

                    StackPanel btnStackPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Height = 50,
                        Width = 212,
                        HorizontalAlignment = HorizontalAlignment.Right
                    };
                    TextBlock textblock = new TextBlock 
                    { 
                        Text = item.Key,
                        Width = 500
                    };
                    Button removeBtn = new Button {
                        Content = "Eliminar",
                        Margin = new Thickness(0,0,10,0),
                        Tag = item.Key
                    };
                    removeBtn.Click += RemoveBtn_Click;
                    Button editBtn = new Button { 
                        Content = "Editar",
                        Tag = item.Key
                    };
                    editBtn.Click += EditBtn_Click;
                    mainStackPanel.Children.Add(textblock);
                    btnStackPanel.Children.Add(removeBtn);
                    btnStackPanel.Children.Add(editBtn);
                    mainStackPanel.Children.Add(btnStackPanel);
                    tasks.Children.Add(mainStackPanel);
                }
            }
            else
            {
                TextBlock textblock = new TextBlock { Text = "No se han creado reglas."};
                tasks.Children.Add(textblock);
            }
        }

        private bool IsTaskListFull()
        {
            return Globals.automatizatedTasks.Count == 5 ? true : false;
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            string[] data = new string[] { "edit", (sender as Button).Tag.ToString() };
            ConfigPage.Current.ConfigFrame.Navigate(typeof(CreateTaskPage), data, new SuppressNavigationTransitionInfo());
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            string tag = (sender as Button).Tag.ToString();
            Globals.automatizatedTasks.Remove(tag);
            Debug.WriteLine(Globals.automatizatedTasks.Count);
            foreach (StackPanel item in tasks.Children)
            {
                if (item.Name == tag) tasks.Children.Remove(item);
            }
            if (tasks.Children.Count == 0)
            {
                TextBlock textblock = new TextBlock { Text = "No se han creado reglas." };
                tasks.Children.Add(textblock);
            }
            createTaskBtn.IsEnabled = IsTaskListFull() ? false : true;
        }

        private void createTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            string[] data = new string[] { "create" };
            ConfigPage.Current.ConfigFrame.Navigate(typeof(CreateTaskPage), data, new SuppressNavigationTransitionInfo());
        }
    }
}
