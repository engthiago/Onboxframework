using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Core.V1.Messaging;
using Onbox.Mvc.V1;
using Onbox.Mvc.V1.Messaging;
using System.Threading.Tasks;

namespace Onbox.Sandbox.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Test : ExternalCommandBase
    {
        public override Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //var newOrderViewModel = container.Resolve<IOrderView>();
            //newOrderViewModel.SetTitle("Edit");
            //var result = newOrderViewModel.ShowDialog();
            var messageService = this.container.Resolve<IMessageService>();

            var someService = this.container.Resolve<SomeService>();
            var someService2 = this.container.Resolve<SomeService>();

            if (someService.Equals(someService2))
            {
                messageService.Show("Equals");
            }
            else
            {
                messageService.Show("Not equals");
            }

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class CleanContainer : ExternalCommandBase
    {
        public override Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            this.container.Reset();

            return Result.Succeeded;
        }
    }
}
