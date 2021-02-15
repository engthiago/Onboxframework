using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using DesignAutomationFramework;
using Onbox.Revit.Remote.DA.Tests.NUnit;
using Onbox.Revit.Remote.DAInternal;
using System;
using System.IO;

namespace Onbox.Revit.Remote.DA.Tests
{
    public abstract class RevitTestRunnerApp : IExternalDBApplication
    {
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

            var testRunner = new RevitTestRunner();
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
