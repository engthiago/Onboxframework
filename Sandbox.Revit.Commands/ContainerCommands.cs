using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Onbox.Sandbox.Revit.Commands
{
    //[Transaction(TransactionMode.Manual)]
    //public class Register : IExternalCommand
    //{
    //    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    //    {
    //        var someService = new SomeService();
    //        someService.Number = 10;
    //        App.Container.Register(someService);
    //        return Result.Succeeded;
    //    }
    //}

    //[Transaction(TransactionMode.Manual)]
    //public class Resolve : IExternalCommand
    //{
    //    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    //    {
    //        var service = App.Container.Resolve<SomeService>();
    //        TaskDialog.Show("Result", service.Number.ToString());
    //        return Result.Succeeded;
    //    }
    //}

    //[Transaction(TransactionMode.Manual)]
    //public class Reset : IExternalCommand
    //{
    //    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    //    {
    //        App.Container.Reset();
    //        return Result.Succeeded;
    //    }
    //}
}
