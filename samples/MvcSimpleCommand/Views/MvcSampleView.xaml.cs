using Onbox.Mvc.Abstractions.VDev;
using Onbox.Mvc.Revit.Abstractions.VDev;
using Onbox.Mvc.Revit.VDev;
using Onbox.Revit.Abstractions.VDev;
using System.Threading.Tasks;

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

        public override async Task OnInitAsync()
        {
            await this.PerformAsync(async () =>
            {
                await Task.Delay(1500);
            });
        }

        public override void OnWarningRetry()
        {
            this.Warning = null;
        }

        public override void OnErrorRetry()
        {
            this.Error = null;
        }
    }
}