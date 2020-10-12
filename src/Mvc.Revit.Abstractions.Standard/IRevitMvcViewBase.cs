using Onbox.Mvc.Abstractions.VDev;
using Onbox.Mvc.VDev;

namespace Onbox.Mvc.Revit.Abstractions.VDev
{
    /// <summary>
    /// Provides specific Revit functionaliy to <see cref="IMvcView"/> like set Revit as parent window and Title Bar visibility
    /// </summary>
    public interface IRevitMvcViewBase : IMvcView
    {
        /// <summary>
        /// Tells how to display this windows's Title Bar, Minimize, and Maximize buttons
        /// </summary>
        void SetTitleVisibility(TitleVisibility titleVisibility);
    }
}