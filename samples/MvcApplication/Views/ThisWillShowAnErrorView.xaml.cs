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
    public interface IThisWillShowAnErrorView : IRevitMvcViewBase, IMvcViewModal
    {
    }

    /// <summary>
    /// A view designed to have Revit as parent window
    /// </summary>
    public partial class ThisWillShowAnErrorView : RevitMvcViewBase, IThisWillShowAnErrorView
    {
        public ThisWillShowAnErrorView(IRevitAppData appData) : base(appData)
        {
            InitializeComponent();
        }

        public override async Task OnInitAsync()
        {
            await this.PerformAsync(async () =>
            {
                await Task.Delay(2000);
                throw new Exception("Oops, this is an error! It's expected though ;)");
            }, error =>
            {
                Error = error.Message;
            });
        }
    }
}