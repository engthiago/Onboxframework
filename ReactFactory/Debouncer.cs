using System;
using System.Threading.Tasks;

namespace Onbox.ReactFactory.V5
{
    public class Debouncer
    {
        private string taskId;

        internal void Debounce(Action action, int delay = 500)
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
