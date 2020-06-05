using Onbox.Mvc.V7;

namespace Onbox.Mvc.Revit.V7
{
    /// <summary>
    /// Provides specific Revit functionaliy to <see cref="IViewMvc"/> like set Revit as parent window and Title Bar visibility
    /// </summary>
    public interface IRevitViewMvcBase : IViewMvc
    {
        /// <summary>
        /// Tells how to display this windows's Title Bar, Minimize, and Maximize buttons
        /// </summary>
        void SetTitleVisibility(TitleVisibility titleVisibility);
    }
}