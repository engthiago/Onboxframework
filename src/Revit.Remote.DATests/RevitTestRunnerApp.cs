using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using DesignAutomationFramework;
using Newtonsoft.Json;
using Onbox.Revit.NUnit.Core.Internal;
using Revit.NUnit.Engine;
using System;
using System.IO;
using System.Linq;

namespace Onbox.Revit.Remote.DATests
{
    public abstract class RevitTestRunnerApp : IExternalDBApplication
    {
        private const string configFile = "tests.json";
        private ControlledApplication application;

        public ExternalDBApplicationResult OnStartup(ControlledApplication application)
        {
            Console.WriteLine("Design Automation App Started...");

            this.application = application;
            DesignAutomationBridge.DesignAutomationReadyEvent += OnAppReady;

            return ExternalDBApplicationResult.Succeeded;
        }

        private void OnAppReady(object sender, DesignAutomationReadyEventArgs e)
        {
            Console.WriteLine("Design Automation App Ready...");

            var app = e.DesignAutomationData.RevitApp;

            var workDir = Directory.GetCurrentDirectory();
            var workitemId = Path.GetDirectoryName(workDir);

            var thisAddin = typeof(RevitTestRunnerApp).Assembly.Location;
            var addinPath = Path.GetDirectoryName(thisAddin);

            RemoteContainer.Register(app, addinPath, workDir, workitemId);
            Console.WriteLine("Registered Remote Container.");

            string assemblyName;
            try
            {
                var json = File.ReadAllText(configFile);
                var config = JsonConvert.DeserializeObject<TestConfiguration>(json);
                assemblyName = config.AssemblyPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error trying to deserialize '{configFile}':");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return;
            }

            // Enumerates the files in the work directory and sub directories to find the test assembly
            // If can't find assembly, there is no point on moving forward
            string assemblyfullPath;
            try
            {
                var files = Directory.GetFiles(workDir, "*.dll", SearchOption.AllDirectories);
                assemblyfullPath = files.First(f => Path.GetFileName(f) == assemblyName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error enumerating test assembly:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace); 
                return;
            }

            var testRunner = new RevitTestRunner(assemblyfullPath);
            var result = this.OnStartup(testRunner, application);

            if (result != ExternalDBApplicationResult.Succeeded)
            {
                e.Succeeded = false;
            }

            e.Succeeded = true;
        }

        public abstract ExternalDBApplicationResult OnStartup(IRevitTestRunner tests, ControlledApplication application);

        public abstract ExternalDBApplicationResult OnShutdown(ControlledApplication application);
    }
}
