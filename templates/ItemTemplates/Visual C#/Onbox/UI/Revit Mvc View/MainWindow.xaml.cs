using Onbox.Mvc.Abstractions.V7;
using Onbox.Mvc.Revit.Abstractions.V7;
using Onbox.Mvc.Revit.V7;
using Onbox.Revit.Abstractions.V7;
using System;
using System.Windows;

namespace $rootnamespace$
{
    /// <summary>
    /// A contract a view designed to have Revit as parent window
    /// </summary>
    public interface I$safeitemname$ : IRevitMvcViewBase, IMvcViewModal
    {
    }

    /// <summary>
    /// A view designed to have Revit as parent window
    /// </summary>
    public partial class $safeitemname$ : RevitMvcViewBase, I$safeitemname$
    {
        public $safeitemname$(IRevitAppData appData) : base(appData)
        {
            InitializeComponent();
        }
    }
}
