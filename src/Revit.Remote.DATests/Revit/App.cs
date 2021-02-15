using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Revit.NUnit.Engine;
using System;

namespace Onbox.Revit.Remote.DATests.Revit
{
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    public class App : RevitTestRunnerApp
    {
        private const string results = "result.xml";

        public override ExternalDBApplicationResult OnStartup(IRevitTestRunner tests, ControlledApplication application)
        {
            Console.WriteLine($"Running Tests...");

            // Runs the test assembly and saves the results into a NUnit xml
            try
            {
                tests.Run(results);
                Console.WriteLine("Successfully ran tests!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error running tests:");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return ExternalDBApplicationResult.Failed;
            }

            // Tests ran, wrapping up...
            Console.WriteLine("Finishing and Uploading...");

            return ExternalDBApplicationResult.Succeeded;
        }

        public override ExternalDBApplicationResult OnShutdown(ControlledApplication application)
        {
            return ExternalDBApplicationResult.Succeeded;
        }
    }
}
