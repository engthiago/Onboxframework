using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using MvcApplication.Views;
using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands;

namespace MvcApplication.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class WPFViewCommand : RevitAppCommand<App>
    {
        public override Result Execute(IContainerResolver container, ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Asks the container for a new instance of a view
            var wpfView = container.Resolve<IHelloWpfView>();
            wpfView.ShowDialog();

            return Result.Succeeded;
        }
    }
}