using Onbox.Mvc.Abstractions.VDev;
using Onbox.Mvc.Revit.Abstractions.VDev;
using Onbox.Mvc.Revit.VDev;
using Onbox.Revit.Abstractions.VDev;
using System.Threading.Tasks;

namespace MvcApplication.Views
{
    /// <summary>
    /// A contract a view designed to have Revit as parent window
    /// </summary>
    public interface IThisWillAsyncLoadView : IRevitMvcViewBase, IMvcViewModal
    {
    }

    /// <summary>
    /// A view designed to have Revit as parent window
    /// </summary>
    public partial class ThisWillAsyncLoadView : RevitMvcViewBase, IThisWillAsyncLoadView
    {
        public ThisWillAsyncLoadView(IRevitAppData appData) : base(appData)
        {
            InitializeComponent();
        }

        public override async Task OnInitAsync()
        {
            await this.PerformAsync(async () =>
            {
                await Task.Delay(5000);
            });
        }
    }
}