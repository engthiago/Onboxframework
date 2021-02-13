using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Revit.NUnit.Engine;

namespace Onbox.Revit.Remote.DATests.Revit
{
    public class App : RevitTestRunnerApp
    {
        public override ExternalDBApplicationResult OnStartup(IRevitTestRunner tests, ControlledApplication application)
        {
            tests.Run("Onbox.Revit.Remote.Tests.dll", "result.xml");

            return ExternalDBApplicationResult.Succeeded;
        }

        public override ExternalDBApplicationResult OnShutdown(ControlledApplication application)
        {
            return ExternalDBApplicationResult.Succeeded;
        }
    }
}
