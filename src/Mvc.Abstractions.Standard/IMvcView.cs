using System;
using System.Threading.Tasks;

namespace Onbox.Mvc.Abstractions.VDev
{
    /// <summary>
    /// Provides base functionality for WPF MVC Pattern
    /// </summary>
    public interface IMvcView : IMvcComponent
    {
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
    public interface IMvcViewModal
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
    public interface IMvcViewModeless
    {
        /// <summary>
        /// Opens the window and returns without waiting for the newly opened window to close
        /// </summary>
        void Show();
    }
}