using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using MvcApplication.Views;
using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands;

namespace MvcApplication.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]

    class ThisWillAsyncLoadViewCommand : RevitAppCommand<App>
    {
        public override Result Execute(IContainerResolver container, ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var view = container.Resolve<IThisWillAsyncLoadView>();
            view.ShowDialog();

            return Result.Succeeded;
        }
    }
}
