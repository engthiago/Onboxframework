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
        Task<T> RunAsync<T>(Func<UIApplication, T> action);
    }

    internal enum DelegateType
    {
        Action = 0,
        Func = 1,
    }

    internal class FuncTask
    {
        internal Delegate Action { get; set; }

        internal DelegateType DelegateType { get; set; }

        internal CancellationTokenSource Cancellation { get; set; }
    }

    public class ExternalHandler : IRevitEventHandler
    {
        private readonly string name;
        private readonly ExternalEvent externalEvent;
        private readonly IDictionary<Task, FuncTask> actions = new Dictionary<Task, FuncTask>();
        private readonly IRevitContext revitContext;
        private object contextResult;

        public ExternalHandler(RevitEventSettings settings, IRevitContext revitContext)
        {
            this.revitContext = revitContext;
            this.name = settings.Name;
            if (settings.IsJournalable)
            {
                this.externalEvent = ExternalEvent.CreateJournalable(this);
            }
            else
            {
                this.externalEvent = ExternalEvent.Create(this);
            }
        }

        public void Execute(UIApplication app)
        {
            if (actions.Any())
            {
                var actionKey = actions.First();
                var taskKey = actionKey.Key;

                if (actionKey.Value.DelegateType == DelegateType.Action)
                {
                    RunAction(app, actionKey, taskKey);
                }
                else
                {
                    RunFunc(app, actionKey, taskKey);
                }
            }
        }

        private void RunAction(UIApplication app, KeyValuePair<Task, FuncTask> actionKey, Task taskKey)
        {
            try
            {
                if (!taskKey.IsCanceled)
                {
                    actionKey.Value.Action?.DynamicInvoke(app);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                actions.Remove(actionKey.Key);
                actionKey.Key.RunSynchronously();
            }
        }

        private void RunFunc(UIApplication app, KeyValuePair<Task, FuncTask> actionKey, Task taskKey)
        {
            try
            {
                if (!taskKey.IsCanceled)
                {
                    this.contextResult = actionKey.Value.Action?.DynamicInvoke(app);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                actions.Remove(actionKey.Key);
                actionKey.Key.RunSynchronously();
                this.contextResult = null;
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

            var task = new Task(DummyAction, cancelSource.Token);
            await AddTask(action, task, cancelSource);
        }

        public async Task<T> RunAsync<T>(Func<UIApplication, T> action)
        {
            if (revitContext.IsInRevitContext())
            {
                var app = revitContext.GetUIApplication();
                var result = action.Invoke(app);
                return result;
            }

            var task = new Task<object>(DummyFunc);
            var funcTask = new FuncTask { Action = action, Cancellation = new CancellationTokenSource(), DelegateType = DelegateType.Func };
            actions.Add(task, funcTask);
            externalEvent.Raise();

            var asyncResult = await task;
            return (T)asyncResult;
        }

        private async Task AddTask(Action<UIApplication> action, Task task, CancellationTokenSource tokenSource)
        {
            if (tokenSource.IsCancellationRequested)
            {
                return;
            }

            actions.Add(task, new FuncTask { Action = action, Cancellation = tokenSource });
            externalEvent.Raise();
            await task;
        }

        private void DummyAction()
        {
        }

        private object DummyFunc()
        {
            return this.contextResult;
        }
    }

    public class RevitEventSettings
    {
        public string Name { get; set; }
        public bool IsJournalable { get; set; }
    }
}
