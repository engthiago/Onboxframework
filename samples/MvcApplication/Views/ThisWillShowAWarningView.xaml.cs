using Onbox.Mvc.Abstractions.VDev;
using Onbox.Mvc.Revit.Abstractions.VDev;
using Onbox.Mvc.Revit.VDev;
using Onbox.Revit.Abstractions.VDev;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace MvcApplication.Views
{
    /// <summary>
    /// A contract a view designed to have Revit as parent window
    /// </summary>
    public interface IThisWillShowAWarningView : IRevitMvcViewBase, IMvcViewModal
    {
    }

    /// <summary>
    /// A view designed to have Revit as parent window
    /// </summary>
    public partial class ThisWillShowAWarningView : RevitMvcViewBase, IThisWillShowAWarningView
    {
        public ThisWillShowAWarningView(IRevitAppData appData) : base(appData)
        {
            InitializeComponent();
        }

        public override async Task OnInitAsync()
        {
            await this.PerformAsync(async () =>
            {
                await Task.Delay(2000);
                Warning = "Oops, this is a warning!";
            });
        }
    }
}