using Autodesk.Revit.UI;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Onbox.Revit.VDev.Async
{
    /// <summary>
    /// Provides a way to run asyncronous taks on Revit's main thread
    /// </summary>
    public interface IRevitEventHandler : IExternalEventHandler
    {
        /// <summary>
        /// The action will run by Revit as soon as its thread is available
        /// </summary>
        Task RunAsync(Action<UIApplication> action, CancellationTokenSource cancelSource = null);

        /// <summary>
        /// The action will run by Revit as soon as its thread is available
        /// </summary>
        Task<T> RunAsync<T>(Func<UIApplication, T> action, CancellationTokenSource cancellationToken = null);

        /// <summary>
        /// Cancels all queue tasks
        /// </summary>
        void CancelAll();

        /// <summary>
        /// Gets the number of queue tasks
        /// </summary>
        int GetQueueCount();
    }
}