using System;
using System.Threading;
using System.Threading.Tasks;

namespace TZDriver.Models.Tools.Utilities
{
    public class TimeOut
    {
        private readonly CancellationTokenSource canceller = new CancellationTokenSource();
        private int v1;
        private bool v2;

        public TimeOut(int v1, bool v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public TimeOut(int timeoutSeconds, Action timesup)
        {
            if (timeoutSeconds == System.Threading.Timeout.Infinite)
            {
                return; // nothing to do
            }

            if (timeoutSeconds < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(timeoutSeconds));
            }

            Task.Delay(TimeSpan.FromSeconds(timeoutSeconds), this.canceller.Token).ContinueWith(
                t =>
                {
                    if (!t.IsCanceled)
                    {
                        timesup();
                    }
                });
        }

        public void Cancel()
        {
            this.canceller.Cancel();
        }

    }
}
