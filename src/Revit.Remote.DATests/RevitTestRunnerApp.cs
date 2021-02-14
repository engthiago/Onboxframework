using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using DesignAutomationFramework;
using Onbox.Revit.NUnit;
using Revit.NUnit.Engine;
using System;

namespace Onbox.Revit.Remote.DATests
{
    public abstract class RevitTestRunnerApp : IExternalDBApplication
    {
        public ExternalDBApplicationResult OnStartup(ControlledApplication application)
        {
            Console.WriteLine("Design Automation App Started...");

            DesignAutomationBridge.DesignAutomationReadyEvent += OnAppReady;

            return ExternalDBApplicationResult.Succeeded;
        }

        private void OnAppReady(object sender, DesignAutomationReadyEventArgs e)
        {
            Console.WriteLine("Design Automation App Ready...");

            var doc = e.DesignAutomationData.RevitDoc;
            var app = e.DesignAutomationData.RevitApp;

            if (e.DesignAutomationData.RevitDoc == null)
            {
                doc = app.NewProjectDocument(UnitSystem.Metric);
                if (doc == null)
                {
                    throw new InvalidOperationException("Could not create new document.");
                }
            }

            Console.WriteLine("Created new document.");

            RevitRemoteContainer.Initialize(
                doc, e.DesignAutomationData.RevitApp, e.DesignAutomationData.FilePath);

            Console.WriteLine("Registered Container.");

            var testRunner = new RevitTestRunner();

            this.OnStartup(testRunner, e.DesignAutomationData.RevitApp);
        }

        public abstract ExternalDBApplicationResult OnStartup(IRevitTestRunner tests, Application application);

        public abstract ExternalDBApplicationResult OnShutdown(ControlledApplication application);
    }
}
