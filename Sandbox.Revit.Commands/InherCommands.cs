using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Onbox.Di.V1;
using Onbox.Mvc.V1;
using System.Collections.ObjectModel;
using System.Linq;

namespace Onbox.Sandbox.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public partial class Inher : ExternalCommandBase
    {
        public override Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var newOrderViewModel = container.Resolve<IOrderView>();
            var result = newOrderViewModel.ShowDialog();

            return Result.Succeeded;
        }
    }
}
