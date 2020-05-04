using System;
using System.Threading.Tasks;
using System.Timers;

namespace Onbox.Core.V5.ReactFactory
{
    /// <summary>
    /// Runs an action sequential times every specified interval of time. It is possible to specify maximum runs as well
    /// </summary>
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

        /// <summary>
        /// Stops firing actions and cleans resources
        /// </summary>
        public void Stop()
        {
            this.timer.Enabled = false;
            this.timer.Stop();
            this.timer.Dispose();
        }
    }
}
