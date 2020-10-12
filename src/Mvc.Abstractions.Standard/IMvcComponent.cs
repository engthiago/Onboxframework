using System;
using System.Threading.Tasks;

namespace Onbox.Mvc.Abstractions.VDev
{
    /// <summary>
    /// A MVC component contract
    /// </summary>
    public interface IMvcComponent
    {
        string Name { get; set; }
        /// <summary>
        /// Runs an async method on View Initialization
        /// </summary>
        /// <param name="func">The method to run</param>
        /// <param name="error">If an exception is raised</param>
        /// <param name="complete">After completing the method</param>
        void RunOnInitFunc(Func<Task> func, Action<string> error = null, Action complete = null);
    }
}