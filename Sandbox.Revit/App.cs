using Autodesk.Revit.UI;
using Onbox.Di.V1;

namespace Onbox.Sandbox.Revit
{
    public class App : IExternalApplication
    {
        public static Container Container { get; set; }

        public Result OnStartup(UIControlledApplication application)
        {
            Container = new Container();
            TaskDialog.Show("Registered Container", "Succeeeded");
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
