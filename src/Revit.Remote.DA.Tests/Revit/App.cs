using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Onbox.Revit.Remote.DA.Tests.NUnit;
using System;

namespace Onbox.Revit.Remote.DA.Tests.Revit
{
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    public class App : RevitTestRunnerApp
    {
        private const string configFile = "tests.json"; // Needs to match the Activity parameter
        private const string resultXml = "result.xml";  // Needs to match the ACtivity parameter

        public override ExternalDBApplicationResult OnStartup(IRevitTestRunner tests, ControlledApplication application)
        {
            Console.WriteLine($"Running Tests...");

            var testAssemblyService = new TestAssemblyService();

            // Gets the name of the test assembly from the config file
            // If this fails, theres no point in moving forward
            string assemblyName = testAssemblyService.GetAssemblyName(configFile);

            // Enumerates the files in the work directory and sub directories to find the test assembly
            // If can't find assembly, there is no point on moving forward
            string assemblyfullPath = testAssemblyService.GetAssemblyFullPath(assemblyName);

            // Runs the test assembly and saves the results into a NUnit xml
            // NUnit engine will handle Test Exceptions but we need to handle Engine / Package loading exceptions
            // We are doing this in the method bellow
            this.RunTests(tests, assemblyfullPath, resultXml);

            // Tests ran, wrapping up...
            Console.WriteLine("Finishing and Uploading...");

            return ExternalDBApplicationResult.Succeeded;
        }

        private void RunTests(IRevitTestRunner tests, string assemblyfullPath, string resultXml)
        {
            try
            {
                // When this run successfully, it doesnt mean that all tests passed of course, only means that 
                // the NUnit engine and the test assembly were loaded correctly and the tests were executed
                tests.Run(assemblyfullPath, resultXml);
                Console.WriteLine("Successfully ran tests!"); 
            }
            catch (Exception e)
            {
                Console.WriteLine("Error running tests:");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                throw;
            }
        }

        public override ExternalDBApplicationResult OnShutdown(ControlledApplication application)
        {
            return ExternalDBApplicationResult.Succeeded;
        }
    }
}
