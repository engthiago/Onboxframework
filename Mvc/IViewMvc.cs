using System;
using System.Threading.Tasks;

namespace Onbox.Mvc.V7
{
    /// <summary>
    /// Provides base functionality for WPF MVC Pattern
    /// </summary>
    public interface IViewMvc
    {
        /// <summary>
        /// Runs an async method on View Initialization
        /// </summary>
        /// <param name="func">The method to run</param>
        /// <param name="error">If an exception is raised</param>
        /// <param name="complete">After completing the method</param>
        void RunOnInitFunc(Func<Task> func, Action<string> error = null, Action complete = null);
        /// <summary>
        /// Set another window as the Owner of this Window
        /// </summary>
        /// <param name="owner">The parent Window</param>
        void SetOwner(object owner);
        /// <summary>
        /// Sets the Title of this Window
        /// </summary>
        /// <param name="title"></param>
        void SetTitle(string title);
    }

    /// <summary>
    /// Provides Modal dialog feature
    /// </summary>
    public interface IViewMvcModal : IViewMvc
    {
        /// <summary>
        /// Opens a window and returns only when the newly opened window is closed
        /// </summary>
        /// <returns></returns>
        bool? ShowDialog();
    }

    /// <summary>
    /// Provides Modeless dialog feature
    /// </summary>
    public interface IViewMvcModeless : IViewMvc
    {
        /// <summary>
        /// Opens the window and returns without waiting for the newly opened window to close
        /// </summary>
        void Show();
    }
}