using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using MvcSimpleCommand.Views;
using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands;
using System.Threading.Tasks;

namespace MvcSimpleCommand.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class MainCommand : RevitContainerCommand<ContainerPipeline>
    {
        public override Result Execute(IContainerResolver container, ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Asks the container for a new instance a message service
            var sampleView = container.Resolve<IMvcSampleView>();
            
            // This will run after the view is rendered 
            sampleView.RunOnInitFunc(async () =>
            {
                await Task.Delay(1500);
            });
            sampleView.ShowDialog();

            return Result.Succeeded;
        }
    }
}