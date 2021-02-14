using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Onbox.Revit.NUnit;
using Revit.NUnit.Engine;
using System;
using System.IO;
using System.Linq;

namespace Onbox.Revit.Remote.DATests.Revit
{
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    public class App : RevitTestRunnerApp
    {
        public override ExternalDBApplicationResult OnStartup(IRevitTestRunner tests, Application application)
        {
            var workingDir = Directory.GetCurrentDirectory();
            var workitemId = Path.GetDirectoryName(workingDir);
            Console.WriteLine($"Workitem Id: {workitemId}");
            Console.WriteLine($"Path: {RevitRemoteContainer.GetFilePath()}");
            Console.WriteLine($"Working Dir: {workingDir}");

            var files = Directory.GetFiles(workingDir, "*.*", SearchOption.AllDirectories);
            Console.WriteLine($"Total Files: {files.Length}");
            foreach (var file in files)
            {
                Console.WriteLine($"File: {file}");
            }

            Console.WriteLine($"Running Tests...");

            var testDll = files.First(f => f.EndsWith("Onbox.Revit.Remote.Tests.dll"));

            try
            {
                tests.Run(testDll, "result.xml");
                Console.WriteLine("Successfully ran tests!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error running tests:");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            Console.WriteLine("Closing and Uploading...");

            files = Directory.GetFiles(workingDir, "*.*", SearchOption.AllDirectories);
            Console.WriteLine($"Total Resulting Files: {files.Length}");
            foreach (var file in files)
            {
                Console.WriteLine($"Resulting files: {file}");
            }

            return ExternalDBApplicationResult.Succeeded;
        }

        public override ExternalDBApplicationResult OnShutdown(ControlledApplication application)
        {
            return ExternalDBApplicationResult.Succeeded;
        }
    }
}
