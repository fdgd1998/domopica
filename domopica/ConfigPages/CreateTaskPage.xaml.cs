using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace domopica.ConfigPages
{
    public sealed partial class CreateTaskPage : Page
    {
        private string actionToPerform;
        public CreateTaskPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Globals.IsTransparencyEnabled(this);
            base.OnNavigatedTo(e);
            string[] data = (string[])e.Parameter;
            actionToPerform = data[0];

            if (data.Length > 1)
            {
                actionType.Text = "Editar regla";
                taskName.Text = data[1];
                createBtn.Content = "Editar";
                taskName.IsEnabled = false;
                LoadCurrentData(data[1]);
            }
            else
            {
                actionType.Text = "Crear nueva regla";
            }
        }

        private void LoadCurrentData(string ruleName)
        {
            LoadValues(lightStartActions, ruleName, "Light", "start");
            LoadValues(curtainStartActions, ruleName, "Curtain", "start");
            LoadValues(doorStartActions, ruleName, "Door", "start");
            LoadValues(alarmStartActions, ruleName, "Alarm", "start");

            LoadValues(lightEndActions, ruleName, "Light", "end");
            LoadValues(curtainEndActions, ruleName, "Curtain", "end");
            LoadValues(doorEndActions, ruleName, "Door", "end");
            LoadValues(alarmEndActions, ruleName, "Alarm", "end");

            startTime.SelectedTime = Globals.automatizatedTasks[ruleName].SelectedStartTime;
            endTime.SelectedTime = Globals.automatizatedTasks[ruleName].SelectedEndTime;
        }

        private void LoadValues(StackPanel panel, string name, string deviceClass, string actionExecution)
        {
            Debug.WriteLine("[i] Options loading started!");
            string tag;
            string[] action;
            foreach (object item in panel.Children)
            {
                if (item is StackPanel)
                {
                    tag = ((item as StackPanel).Children[0] as CheckBox).Tag.ToString();
                    Debug.WriteLine("Contains "+ tag +": "+Globals.automatizatedTasks[name].Contains(tag, deviceClass, actionExecution));
                    if (Globals.automatizatedTasks[name].Contains(tag, deviceClass, actionExecution)) 
                    {
                        action = Globals.automatizatedTasks[name].GetAction(tag, deviceClass, actionExecution);
                        Debug.WriteLine("actionData: " + action[0] + ", " + action[1]);

                        if (deviceClass == "Light" || deviceClass == "Alarm")
                        {
                            ((item as StackPanel).Children[0] as CheckBox).IsChecked = true;
                            if (action[1] == "on")
                            {
                                ((item as StackPanel).Children[1] as ToggleSwitch).IsOn = true;
                            }
                            else
                            {
                                ((item as StackPanel).Children[1] as ToggleSwitch).IsOn = false;
                            }
                                   
                        }
                        else
                        {
                            ((item as StackPanel).Children[0] as CheckBox).IsChecked = true;
                            if (action[1] == "open")
                            {
                                ((item as StackPanel).Children[1] as ToggleSwitch).IsOn = true;
                            }
                            else
                            {
                                ((item as StackPanel).Children[1] as ToggleSwitch).IsOn = false;
                            }
                        }
                    }
                }
            }
        }

        private void SetActions(StackPanel panel, string name, string deviceClass, string actionExecution)
        {
            string value;
            string tag;
            foreach (object item in panel.Children)
            {
                if (item is StackPanel)
                {
                    tag = ((item as StackPanel).Children[0] as CheckBox).Tag.ToString();
                    if (deviceClass == "Light" || deviceClass == "Alarm")
                    {
                        value = ((item as StackPanel).Children[1] as ToggleSwitch).IsOn ? "on" : "off";
                    }
                    else
                    {
                        value = ((item as StackPanel).Children[1] as ToggleSwitch).IsOn ? "open" : "close";
                    }

                    if (actionToPerform == "create")
                    {
                        if (((item as StackPanel).Children[0] as CheckBox).IsChecked == true)
                        {
                            Globals.automatizatedTasks[name].AddAction(tag, value, deviceClass, actionExecution);
                            Debug.WriteLine("tag is " + tag);
                            Debug.WriteLine("value is " + value);
                        }
                    }
                    else
                    {
                        if (((item as StackPanel).Children[0] as CheckBox).IsChecked == true)
                        {
                            Globals.automatizatedTasks[name].ModifyAction(tag, value, deviceClass, actionExecution);
                        }
                        else
                        {
                            if (Globals.automatizatedTasks[name].Contains(tag, deviceClass, actionExecution))
                            {
                                Globals.automatizatedTasks[name].RemoveAction(tag, deviceClass, actionExecution);
                            }
                        }
                    }

                    /*if (((item as StackPanel).Children[0] as CheckBox).IsChecked == true)
                    {
                        if (actionToPerform == "create")
                        {
                            Globals.automatizatedTasks[name].AddAction(tag, value, deviceClass, actionExecution);
                            Debug.WriteLine("tag is " + tag);
                            Debug.WriteLine("value is " + value);
                        }
                        else
                        {
                            Globals.automatizatedTasks[name].ModifyAction(tag, value, deviceClass, actionExecution);
                        }
                    }
                    else
                    {
                        if (actionToPerform == "edit")
                        {
                            if (Globals.automatizatedTasks[name].Contains(tag, deviceClass, actionExecution))
                            {
                                Globals.automatizatedTasks[name].RemoveAction(tag, deviceClass, actionExecution);
                            }
                        }
                    }*/
                }
                /*if (item is ToggleSwitch)
                {
                    Globals.automatizatedTasks[name].StartActivateAlarm = (item as ToggleSwitch).IsOn ? true : false;
                }*/
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (taskName.Text == "")
            {
                taskName.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            }
            else
            {
                if (startTime.SelectedTime == null)
                {
                    startTime.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                }
                else if (endTime.SelectedTime == null)
                {
                    endTime.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                }
                else
                {
                    TimeSpan start = startTime.Time;
                    TimeSpan end = endTime.Time;
                    string name = taskName.Text;
                    
                    if (actionToPerform == "create")
                    {
                        Globals.automatizatedTasks.Add(name, new AutomatizationTask(start.Hours, start.Minutes, end.Hours, end.Minutes));
                    }
                    else
                    {
                        Globals.automatizatedTasks[name].TaskEnabled = false;
                    }

                    Globals.automatizatedTasks[name].SelectedStartTime = (TimeSpan)startTime.SelectedTime;
                    Globals.automatizatedTasks[name].SelectedEndTime = (TimeSpan)endTime.SelectedTime;

                    SetActions(lightStartActions, name, "Light", "start");
                    SetActions(curtainStartActions, name, "Curtain", "start");
                    SetActions(doorStartActions, name, "Door", "start");
                    SetActions(alarmStartActions, name, "Alarm", "start");

                    SetActions(lightEndActions, name, "Light", "end");
                    SetActions(curtainEndActions, name, "Curtain", "end");
                    SetActions(doorEndActions, name, "Door", "end");
                    SetActions(alarmEndActions, name, "Alarm", "end");

                    if (actionToPerform == "edit")
                    {
                        Globals.automatizatedTasks[name].TaskEnabled = true;
                    }

                    ConfigPage.Current.ConfigFrame.Navigate(typeof(AutomatizationPage), null, new SuppressNavigationTransitionInfo());
                }
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            ConfigPage.Current.ConfigFrame.Navigate(typeof(AutomatizationPage), null, new SuppressNavigationTransitionInfo());
        }
    }
}
