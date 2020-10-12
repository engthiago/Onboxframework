using Autodesk.Revit.UI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Onbox.Revit.VDev.Async
{
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

    /// <summary>
    /// Provides a way to run asyncronous taks on Revit's main thread
    /// </summary>
    public class RevitExternalHandler : IRevitEventHandler
    {
        private readonly string name;
        private readonly ExternalEvent externalEvent;
        private readonly IDictionary<Task, FuncTask> queue = new ConcurrentDictionary<Task, FuncTask>();
        private readonly IRevitContext revitContext;
        private object contextResult;

        /// <summary>
        /// Constructor
        /// </summary>
        public RevitExternalHandler(RevitAsyncSettings settings, IRevitContext revitContext)
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

        /// <summary>
        /// Cancels all queue tasks
        /// </summary>
        public void CancelAll()
        {
            this.queue.Clear();
        }

        /// <summary>
        /// Gets the number of queue tasks
        /// </summary>
        public int GetQueueCount()
        {
            return this.queue.Count();
        }

        /// <summary>
        /// This method is called to handle the external event
        /// </summary>
        public void Execute(UIApplication app)
        {
            if (queue.Any())
            {
                var actionKey = queue.First();
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
                queue.Remove(actionKey.Key);
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
                queue.Remove(actionKey.Key);
                actionKey.Key.RunSynchronously();
                this.contextResult = null;
            }
        }

        /// <summary>
        /// The event's name
        /// </summary>
        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// The action will run by Revit as soon as its thread is available
        /// </summary>
        public async Task RunAsync(Action<UIApplication> action, CancellationTokenSource cancelSource = null)
        {
            if (revitContext.IsInRevitContext())
            {
                var app = revitContext.GetUIApplication();
                action?.Invoke(app);
                return;
            }

            if (cancelSource == null)
            {
                cancelSource = new CancellationTokenSource();
            }

            var task = new Task(DummyAction, cancelSource.Token);
            await AddTask(action, task, cancelSource);
        }

        private async Task AddTask(Action<UIApplication> action, Task task, CancellationTokenSource tokenSource)
        {
            if (tokenSource.IsCancellationRequested)
            {
                return;
            }

            queue.Add(task, new FuncTask { Action = action, Cancellation = tokenSource });
            externalEvent.Raise();
            await task;
        }

        /// <summary>
        /// The action will run by Revit as soon as its thread is available
        /// </summary>
        public async Task<T> RunAsync<T>(Func<UIApplication, T> action, CancellationTokenSource cancelSource = null)
        {
            if (revitContext.IsInRevitContext())
            {
                var app = revitContext.GetUIApplication();
                var result = action.Invoke(app);
                return result;
            }

            if (cancelSource == null)
            {
                cancelSource = new CancellationTokenSource();
            }

            var task = new Task<object>(DummyFunc, cancelSource.Token);
            return await AddTask(action, task, cancelSource);
        }

        private async Task<T> AddTask<T>(Func<UIApplication, T> action, Task<object> task, CancellationTokenSource tokenSource)
        {
            if (tokenSource.IsCancellationRequested)
            {
                return default;
            }

            var funcTask = new FuncTask { Action = action, Cancellation = tokenSource, DelegateType = DelegateType.Func };
            queue.Add(task, funcTask);
            externalEvent.Raise();

            var asyncResult = await task;
            return (T)asyncResult;
        }

        private void DummyAction()
        {
        }

        private object DummyFunc()
        {
            return this.contextResult;
        }
    }

    /// <summary>
    /// Data class to hold preferences for Revit async tasks
    /// </summary>
    public class RevitAsyncSettings
    {
        /// <summary>
        /// The name of the handler for Revit async tasks
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// If the handler will be journable
        /// </summary>
        public bool IsJournalable { get; set; }
    }
}