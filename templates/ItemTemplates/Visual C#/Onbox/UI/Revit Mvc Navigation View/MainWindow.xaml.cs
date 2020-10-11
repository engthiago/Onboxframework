using Onbox.Mvc.Abstractions.V7;
using Onbox.Mvc.Revit.Abstractions.V7;
using Onbox.Mvc.Revit.V7;
using Onbox.Mvc.V7;
using Onbox.Revit.Abstractions.V7;


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
