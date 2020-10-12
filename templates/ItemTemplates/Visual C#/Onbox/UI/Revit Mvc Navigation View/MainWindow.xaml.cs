using Onbox.Mvc.Abstractions.VDev;
using Onbox.Mvc.Revit.Abstractions.VDev;
using Onbox.Mvc.Revit.VDev;
using Onbox.Mvc.VDev;
using Onbox.Revit.Abstractions.VDev;


namespace $rootnamespace$
{
    /// <summary>
    /// A view designed to have Revit as parent window
    /// </summary>
    public interface I$safeitemname$ : IRevitMvcViewBase, IMvcViewModal
    {
    }

    /// <summary>
    /// A view designed to have Revit as parent window
    /// </summary>
    public partial class $safeitemname$ : RevitMvcViewBase, I$safeitemname$
    {
        public $safeitemname$(IRevitAppData appData, INavigator navigator) : base(appData)
        {
            InitializeComponent();
            navigator.Attach(this, this.Navigator);
        }
    }
}