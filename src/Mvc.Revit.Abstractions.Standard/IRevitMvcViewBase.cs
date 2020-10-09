using Onbox.Mvc.Abstractions.V7;
using Onbox.Mvc.V7;

namespace Onbox.Mvc.Revit.Abstractions.V7
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