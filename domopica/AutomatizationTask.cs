using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
//using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Windows.UI.Xaml.Controls;

namespace domopica
{
    class AutomatizationTask
    {
        private DateTime nowTime;
        private DateTime startTime;
        private DateTime endTime;
        public TimeSpan SelectedStartTime { get; set; }
        public TimeSpan SelectedEndTime { get; set; }
        private int startHour;
        private int startMinute;
        private int endHour;
        private int endMinute;
        private Timer startTimer;
        private Timer endTimer;

        private Dictionary<string, string> startLightAction = new Dictionary<string, string>();
        private Dictionary<string, string> startCurtainAction = new Dictionary<string, string>();
        private Dictionary<string, string> startDoorAction = new Dictionary<string, string>();
        private string StartActivateAlarm { get; set; }

        private Dictionary<string, string> endLightAction = new Dictionary<string, string>();
        private Dictionary<string, string> endCurtainAction = new Dictionary<string, string>();
        private Dictionary<string, string> endDoorAction = new Dictionary<string, string>();
        private string EndActivateAlarm { get; set; }

        public string[] GetAction(string name, string type, string execute)
        {
            string value = "";
            switch (type)
            {
                case "Light":
                    if (execute == "start") value = startLightAction[name];
                    else value = endLightAction[name];
                    break;
                case "Curtain":
                    if (execute == "start") value = startCurtainAction[name];
                    else value = endCurtainAction[name];
                    break;
                case "Door":
                    if (execute == "start") value = startDoorAction[name];
                    else value = endDoorAction[name];
                    break;
                case "Alarm":
                    if (execute == "start") value = StartActivateAlarm;
                    else value = EndActivateAlarm;
                    break;
            }
            Debug.WriteLine("GetAction: "+name+","+value);
            return new string[] {name, value};
        }

        public bool Contains (string key, string deviceClass, string executeTask)
        {
            bool containsKey = false;
            try
            {
                string[] data = GetAction(key, deviceClass, executeTask);
                if (data[0] != null && data[1] != null)
                {
                    if (data[0] == key) containsKey = true;
                }
            } 
            catch (KeyNotFoundException e)
            {
                Debug.WriteLine("key not found!");
            }
            
            
            return containsKey;
        }

        public void ModifyAction(string name, string value, string type, string execute)
        {
            switch (type)
            {
                case "Light":
                    if (execute == "start") startLightAction[name] = value;
                    else endLightAction[name] = value;
                    break;
                case "Curtain":
                    if (execute == "start") startCurtainAction[name] = value;
                    else endCurtainAction[name] = value;
                    break;
                case "Door":
                    if (execute == "start") startDoorAction[name] = value;
                    else endDoorAction[name] = value;
                    break;
                case "Alarm":
                    if (execute == "start") StartActivateAlarm = value;
                    else EndActivateAlarm = value;
                    break;
            }
        }

        public void AddAction(string name, string value, string type, string execute)
        {
            switch(type)
            {
                case "Light":
                    if (execute == "start") startLightAction.Add(name, value);
                    else endLightAction.Add(name, value);
                    break;
                case "Curtain":
                    if (execute == "start") startCurtainAction.Add(name, value);
                    else endCurtainAction.Add(name, value);
                    break;
                case "Door":
                    if (execute == "start") startDoorAction.Add(name, value);
                    else endDoorAction.Add(name, value);
                    break;
                case "Alarm":
                    if (execute == "start") StartActivateAlarm = value;
                    else EndActivateAlarm = value;
                    break;
            }
        }

        public void RemoveAction(string name, string type, string execute)
        {
            switch (type)
            {
                case "Light":
                    if (execute == "start") startLightAction.Remove(name);
                    else endLightAction.Remove(name);
                    break;
                case "Curtain":
                    if (execute == "start") startCurtainAction.Remove(name);
                    else endCurtainAction.Remove(name);
                    break;
                case "Door":
                    if (execute == "start") startDoorAction.Remove(name);
                    else endDoorAction.Remove(name);
                    break;
                case "Alarm":
                    if (execute == "start") StartActivateAlarm = "off";
                    else EndActivateAlarm = "off";
                    break;
            }
        }

        private bool taskEnabled = true;
        public bool TaskEnabled {
            get
            {
                return taskEnabled;
            }
            set
            {
                taskEnabled = value;
                if (taskEnabled)
                {
                    startTimer.Start();
                    endTimer.Start();
                }
                else
                {
                    startTimer.Stop();
                    endTimer.Stop();
                }
            } 
        }

        public AutomatizationTask()
        {
        }

        public AutomatizationTask(int startHour, int startMinute, int endHour, int endMinute)
        {
            nowTime = DateTime.Now;
            this.startHour = startHour;
            this.startMinute = startMinute;
            this.endHour = endHour;
            this.endMinute = endMinute;
            StartTimer(startHour, startMinute);
            EndTimer(endHour, endMinute);
        }

        private void StartTimer(int hour, int minute)
        {
            startTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, hour, minute, 0, 0);
            if (DateTime.Now > startTime)
            {
                startTime = startTime.AddDays(1);
                SelectedStartTime.Add(new TimeSpan(1, 0, 0, 0));
                Debug.WriteLine("next start execution at "+startTime);
            }

            double tickTime = (double)(startTime - DateTime.Now).TotalMilliseconds;
            startTimer = new Timer(tickTime);
            startTimer.Elapsed += new ElapsedEventHandler(StartTask);
            startTimer.Start();
        }

        private void EndTimer(int hour, int minute)
        {
            endTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, hour, minute, 0, 0);
            if (DateTime.Now > endTime)
            {
                endTime = endTime.AddDays(1);
                SelectedEndTime.Add(new TimeSpan(1, 0, 0, 0));
                Debug.WriteLine("next end execution at " + endTime);
            }

            double tickTime = (endTime - DateTime.Now).TotalMilliseconds;
            endTimer = new Timer(tickTime);
            endTimer.Elapsed += new ElapsedEventHandler(EndTask);
            endTimer.Start();
        }

        private async void StartTask(object sender, ElapsedEventArgs e)
        {
            startTimer.Stop();
            Debug.WriteLine("task started!");
            foreach (KeyValuePair<string, string> item in startLightAction)
            {
                Devices.lightsList[item.Key].LightOn = (item.Value == "on") ? true : false;
            }
            foreach (KeyValuePair<string, string> item in startCurtainAction)
            {
                Devices.curtainsList[item.Key].IsOpened = (item.Value == "open") ? true : false;
            }
            foreach (KeyValuePair<string, string> item in startDoorAction)
            {
                Devices.doorsList[item.Key].IsOpened = (item.Value == "open") ? true : false;
            }
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    Devices.IsAlarmActivated = StartActivateAlarm == "on" ? true : false;
                    Debug.WriteLine("StartActivateAlarm: " + StartActivateAlarm);
                });
            StartTimer(startHour, startMinute);
        }

        private async void EndTask(object sender, ElapsedEventArgs e)
        {
            endTimer.Stop();
            Debug.WriteLine("task finished!");
            foreach (KeyValuePair<string, string> item in endLightAction)
            {
                Devices.lightsList[item.Key].LightOn = (item.Value == "on") ? true : false;
            }
            foreach (KeyValuePair<string, string> item in endCurtainAction)
            {
                Devices.curtainsList[item.Key].IsOpened = (item.Value == "open") ? true : false;
            }
            foreach (KeyValuePair<string, string> item in endDoorAction)
            {
                Devices.doorsList[item.Key].IsOpened = (item.Value == "open") ? true : false;
            }
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    Devices.IsAlarmActivated = EndActivateAlarm == "on" ? true : false;
                    Debug.WriteLine("EndActivateAlarm: " + EndActivateAlarm);
                });
            EndTimer(endHour, endMinute);
        }
    }
}
