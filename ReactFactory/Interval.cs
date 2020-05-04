using System;
using System.Threading.Tasks;
using System.Timers;

namespace Onbox.ReactFactory.V5
{
    public class Interval
    {
        private Timer timer;
        private readonly Action<int> actionInterval;
        private readonly int? maxRuns;
        private int currentRun;
        private readonly Action action;

        internal Interval(Action<int> action, int interval, int? maxRuns = null)
        {
            this.actionInterval = action;
            this.maxRuns = maxRuns;
            Task.Factory.StartNew(() =>
            {
                timer = new Timer(interval);
                timer.Elapsed += this.ActionInterval;
                timer.Enabled = true;
                timer.AutoReset = true;
            });
        }

        private void ActionInterval(object sender, ElapsedEventArgs e)
        {
            ++currentRun;
            if (maxRuns != null && currentRun > maxRuns)
            {
                this.Stop();
            }
            else
            {
                actionInterval?.Invoke(currentRun);
            }
        }

        internal Interval(Action action, int interval, int? maxRuns = null)
        {
            this.action = action;
            this.maxRuns = maxRuns;
            Task.Factory.StartNew(() =>
            {
                timer = new Timer(interval);
                timer.Elapsed += this.Action;
                timer.Enabled = true;
                timer.AutoReset = true;
            });
        }

        private void Action(object sender, ElapsedEventArgs e)
        {
            ++currentRun;
            if (maxRuns != null && currentRun > maxRuns)
            {
                this.Stop();
            }
            else
            {
                action.Invoke();
            }
        }

        public void Stop()
        {
            this.timer.Enabled = false;
            this.timer.Stop();
            this.timer.Dispose();
        }
    }
}
