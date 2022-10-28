using Onbox.Mvc.Revit.Abstractions.VDev;
using Onbox.Mvc.VDev;
using Onbox.Revit.Abstractions.VDev;

namespace Onbox.Mvc.Revit.VDev
{
    public class RevitViewOptions
    {
        public TitleVisibility TitleVisibility { get; set; } = TitleVisibility.HideMinimizeAndMaximize;
    }

    /// <summary>
    /// Provides specific Revit functionaliy to <see cref="MvcViewBase"/> like set Revit as parent window and Title Bar visibility
    /// </summary>
    public abstract class RevitMvcViewBase : MvcViewBase, IRevitMvcViewBase
    {
        private TitleVisibility titleVisibility;
        private readonly RevitViewAttacher viewAttacher;

        /// <summary>
        /// Provides specific Revit functionaliy to <see cref="MvcViewBase"/> like set Revit as parent window and Title Bar visibility
        /// </summary>
        /// <param name="revitUIApp">Some information of the current instance of Revit UI App</param>
        public RevitMvcViewBase(IRevitAppData revitUIApp, RevitViewOptions viewOptions = null)
        {
            if (viewOptions == null)
            {
                viewOptions = new RevitViewOptions();
            }

            this.titleVisibility = viewOptions.TitleVisibility;
            this.viewAttacher = new RevitViewAttacher(this, revitUIApp.GetRevitWindowHandle(), this.titleVisibility);
            this.viewAttacher.Attach();
        }

        /// <summary>
        /// This Needs to be called right after the view constructor and before the view is loaded or initiliazed. Otherwise this will be ignored
        /// </summary>
        public void SetTitleVisibility(TitleVisibility titleVisibility)
        {
            this.titleVisibility = titleVisibility;
            this.viewAttacher.SetTitleVisibility(this.titleVisibility);
        }
    }
}