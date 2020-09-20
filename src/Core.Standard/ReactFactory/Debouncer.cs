using System;
using System.Threading.Tasks;

namespace Onbox.Core.V7.ReactFactory
{
    /// <summary>
    /// Onbox Debouncer runs an action after a particular time span has passed without another action is fired
    /// </summary>
    public class Debouncer
    {
        private string taskId;

        /// <summary>
        /// Debounces an action
        /// </summary>
        public void Debounce(Action action, int delay = 500)
        {
            var taskId = Guid.NewGuid().ToString();
            this.taskId = taskId;

            Task.Factory.StartNew(async () =>
            {
                await Task.Delay(delay);

                if (this.taskId == taskId)
                {
                    action?.Invoke();
                }
            });
        }
    }
}
