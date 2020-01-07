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
            var someService = this.container.Resolve<SomeService>();
            var someService2 = this.container.Resolve<SomeService>();

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class ViewsCommand : ExternalCommandBase
    {
        public override Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var testWindow = container.Resolve<ITestWindow>();
            testWindow.ShowDialog();

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
