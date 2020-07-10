using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Onbox.Revit.V7
{
    public interface IRevitEventHandler : IExternalEventHandler
    {
        Task RunAsync(Action<UIApplication> action);
        Task RunAsync(Action<UIApplication> action, CancellationTokenSource cancelSource);
    }

    public class ActionTask
    {
        public Action<UIApplication> Action { get; set; }
        public CancellationTokenSource Cancellation { get; set; }
    }


    public class ExternalHandler : IRevitEventHandler
    {
        private readonly string name;
        private readonly ExternalEvent externalEvent;
        private readonly IDictionary<Task, ActionTask> actions = new Dictionary<Task, ActionTask>();
        private readonly IRevitContext revitContext;

        public ExternalHandler(RevitEventSettings settings, IRevitContext revitContext)
        {
            this.name = settings.Name;
            this.externalEvent = ExternalEvent.CreateJournalable(this);
            this.revitContext = revitContext;
        }

        public void Execute(UIApplication app)
        {
            if (actions.Any())
            {
                var actionKey = actions.First();
                var taskKey = actionKey.Key;
                if (!taskKey.IsCanceled)
                {
                    actionKey.Value.Action?.Invoke(app);
                    actionKey.Key.RunSynchronously();
                }
                actions.Remove(actionKey.Key);
            }
        }

        public string GetName()
        {
            return name;
        }

        public async Task RunAsync(Action<UIApplication> action)
        {
            await RunAsync(action, new CancellationTokenSource());
        }

        public async Task RunAsync(Action<UIApplication> action, CancellationTokenSource cancelSource)
        {
            if (revitContext.IsInRevitContext())
            {
                var app = revitContext.GetUIApplication();
                action?.Invoke(app);
                return;
            }

            var task = new Task(DummyMethod, cancelSource.Token);
            await AddTask(action, task, cancelSource);
        }

        private async Task AddTask(Action<UIApplication> action, Task task, CancellationTokenSource tokenSource)
        {
            if (tokenSource.IsCancellationRequested)
            {
                return;
            }

            actions.Add(task, new ActionTask { Action = action, Cancellation = tokenSource });
            externalEvent.Raise();
            await task;
        }

        private void DummyMethod()
        {
        }
    }

    public class RevitEventSettings
    {
        public string Name { get; set; }
    }
}
