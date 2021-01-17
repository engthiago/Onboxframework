using Onbox.Mvc.Abstractions.VDev;
using Onbox.Mvc.Revit.Abstractions.VDev;
using Onbox.Mvc.Revit.VDev;
using Onbox.Revit.Abstractions.VDev;

namespace MvcSimpleCommand.Views
{
    /// <summary>
    /// A contract a view designed to have Revit as parent window
    /// </summary>
    public interface IMvcSampleView : IRevitMvcViewBase, IMvcViewModal
    {
    }

    /// <summary>
    /// A view designed to have Revit as parent window
    /// </summary>
    public partial class MvcSampleView : RevitMvcViewBase, IMvcSampleView
    {
        public MvcSampleView(IRevitAppData appData) : base(appData)
        {
            InitializeComponent();
        }
    }
}