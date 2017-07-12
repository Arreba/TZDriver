using System;
using Windows.UI.Xaml;

namespace TZDriver.Models.Tools.Utilities
{
    public class IntervalTimer
    {
        DispatcherTimer timer;
        public bool isTimerRunning { get; set; } = false;

        public IntervalTimer(int intervalInMinutes)
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, intervalInMinutes, 0);
        }

        public void StartTimer(Action action)
        {
            timer.Tick += (s, e) => action();
            timer.Start();
            isTimerRunning = true;
        }

        public void StopTimer()
        {
            timer.Stop();
            isTimerRunning = false;
        }
    }
}
