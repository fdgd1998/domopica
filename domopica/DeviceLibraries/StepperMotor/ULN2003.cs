using IOTMotorDrivers.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using ABElectronics_Win10IOT_Libraries;

namespace IOTMotorDrivers
{
    public class ULN2003 : IDisposable
    {
        // GPIO pins
        private byte IN1pin, IN2pin, IN3pin, IN4pin;
        // Step Sequence
        private List<bool[]> Step_Seqs { get; set; }

        public bool IsInitialized { get; set; } = false;

        public bool IsSupported { get; set; } = true;

        private IOPi Bus { get; set; }

        private SafeBreak SafeBreakMe;

        /// <summary>
        /// Create an instance of the ULN2003 driver (currently only supports 28BYJ step motor)
        /// </summary>
        public ULN2003(IOPi bus, byte pin1, byte pin2, byte pin3, byte pin4, ULN2003Enums motorType = ULN2003Enums.STEP_28BYJ)
        {
            if (motorType == ULN2003Enums.Other) IsSupported = false;
            else
            {
                Bus = bus;
                IN1pin = pin1;
                IN2pin = pin2;
                IN3pin = pin3;
                IN4pin = pin4;
                    Bus.SetPinDirection(IN1pin, false);
                    Bus.SetPinDirection(IN2pin, false);
                    Bus.SetPinDirection(IN3pin, false);
                    Bus.SetPinDirection(IN4pin, false);
                    //IN1 = gpio.OpenPin(IN1pin);
                    //IN2 = gpio.OpenPin(IN2pin);
                    //IN3 = gpio.OpenPin(IN3pin);
                    //IN4 = gpio.OpenPin(IN4pin);

                    //IN1.SetDriveMode(GpioPinDriveMode.Output);
                    //IN2.SetDriveMode(GpioPinDriveMode.Output);
                    //IN3.SetDriveMode(GpioPinDriveMode.Output);
                    //IN4.SetDriveMode(GpioPinDriveMode.Output);

                    SetGPIOWrite(false, false, false, false);

                    SetStepSequences(motorType);

                    SafeBreakMe = new SafeBreak();
                    SafeBreakMe.PropertyChanged += SafeBreakMe_PropertyChanged;

                    IsInitialized = true;
                
            }
        }

        private void SafeBreakMe_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var breakMe = sender as SafeBreak;
            if (e.PropertyName == "IsActive" & !breakMe.IsActive & !breakMe.IsDisposed)
            {
                // Only dispose if the Active is false
                //if (breakMe.Dispose)
                //{
                //    SafeBreakMe.IsDisposed = true;
                //    SetGPIOWrite(-1, -1, -1, -1);
                //}
                //else 
                    SetGPIOWrite(false, false, false, false);
            }
        }

        private void SetGPIOWrite(bool in1, bool in2, bool in3, bool in4)
        {
            if (!in1) Bus.WritePin(IN1pin, false);
            if (in1) Bus.WritePin(IN1pin, true);
            //if (in1 == -1) IN1.Dispose();

            if (!in2) Bus.WritePin(IN2pin, false);
            if (in2) Bus.WritePin(IN2pin, true);
            //if (in2 == -1) IN2.Dispose();

            if (!in3) Bus.WritePin(IN3pin, false);
            if (in3) Bus.WritePin(IN3pin, true);
            //if (in3 == -1) IN3.Dispose();


            if (!in4) Bus.WritePin(IN4pin, false);
            if (in4) Bus.WritePin(IN4pin, true);
            //if (in4 == -1) IN4.Dispose();
        }

        private void SetStepSequences(ULN2003Enums motorType)
        {
            if (motorType == ULN2003Enums.STEP_28BYJ)
            {
                // Each sequence is a step for the step motor 
                Step_Seqs = new List<bool[]>()
                {
                    new bool[] { true, false, false, false },
                    new bool[] { true, true, false, false },
                    new bool[] { false, true, false, false },
                    new bool[] { false, true, true, false },
                    new bool[] { false, false, true, false },
                    new bool[] { false, false, true, true },
                    new bool[] { false, false, false, true },
                    new bool[] { true, false, false, true }
                };
            }
        }

        private async Task StartSequence(int Steps, int Delay, List<bool[]> Local_Step_Seqs, IProgress<ULN2003Progress> progress)
        {
            int seqCounter = 0;
            for (int step = 0; step <= Steps; step++)
            {
                if (seqCounter < Local_Step_Seqs.Count)
                {
                    var step_Seq = Local_Step_Seqs[seqCounter];
                    SetGPIOWrite(step_Seq[0], step_Seq[1], step_Seq[2], step_Seq[3]);
                    await Task.Delay(Delay);
                    if (progress != null)
                        progress.Report(new ULN2003Progress { IsActive = true, CurrentStep = step, CurrentSequence = seqCounter, StepsSize = Steps });
                    seqCounter++;
                }

                if (seqCounter == Local_Step_Seqs.Count) seqCounter = 0;

                if (SafeBreakMe.Break)
                {
                    SafeBreakMe.Break = false;
                    break;
                }
            }
        }

        /// <summary>
        /// start the step motor
        /// </summary>
        /// <param name="revolutions">each revolution executes 4096 steps and hence the lowest is 0.00041 for 1 step</param>
        /// <param name="isClockwise">choose the direction between clockwise and counter clockwise</param>
        /// <param name="delay">time required for executing the steps to move the motor</param>
        /// <param name="progress">get real time updates regarding the steps</param>
        public async Task Start(/*double revolutions,*/ bool isClockwise = false,
            int delay = 100, IProgress<ULN2003Progress> progress = null)
        {
            if (IsSupported & IsInitialized)
            {
                SafeBreakMe.IsActive = true;
                var revsPercent = Convert.ToInt32(512);
                // 1 revolution = 4096 steps
                var Steps = revsPercent < 1 ? 512 : revsPercent;
                var Local_Step_Seqs = new List<bool[]>(Step_Seqs);
                if (!isClockwise) Local_Step_Seqs.Reverse();
                var Delay = delay > 1000 || delay < 1 ? 1 : delay;

                await StartSequence(Steps, Delay, Local_Step_Seqs, progress);

                if (progress != null)
                    progress.Report(new ULN2003Progress { IsActive = false });
                SafeBreakMe.IsActive = false;
            }
        }

        /// <summary>
        /// stop the step motor
        /// </summary>
        /// <param name="safeStop">stop the step motor in a safe way</param>
        public void Stop(bool safeStop = true)
        {
            if (IsSupported & IsInitialized)
            {
                if (safeStop)
                {
                    if (SafeBreakMe.IsActive) SafeBreakMe.Break = true;
                }
                else SafeBreakMe.IsActive = false;
            }
        }

        public void Dispose()
        {
            SafeBreakMe.Dispose = true;
            SafeBreakMe.Break = true;
            if (!SafeBreakMe.IsActive) SafeBreakMe.IsActive = false;
        }
    }
}
