using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Threading.Tasks;

namespace Onbox.Sandbox.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Inher : ExternalCommandBase
    {
        public override Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var newOrderViewModel = container.Resolve<IOrderView>();
            newOrderViewModel.SetTitle("Edit");
            var result = newOrderViewModel.ShowDialog();

            return Result.Succeeded;
        }
    }
}
