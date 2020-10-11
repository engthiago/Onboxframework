using Onbox.Abstractions.V7;
using Onbox.Revit.V7.Commands;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using $safeprojectname$.Views;

namespace $safeprojectname$.Revit.Commands
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
