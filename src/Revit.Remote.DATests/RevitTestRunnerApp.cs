using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using DesignAutomationFramework;
using Onbox.Revit.NUnit;
using Revit.NUnit.Engine;

namespace Onbox.Revit.Remote.DATests
{
    public abstract class RevitTestRunnerApp : IExternalDBApplication
    {
        private ControlledApplication application;

        public ExternalDBApplicationResult OnStartup(ControlledApplication application)
        {
            this.application = application;
            DesignAutomationBridge.DesignAutomationReadyEvent += OnAppReady;
            return ExternalDBApplicationResult.Succeeded;
        }

        private void OnAppReady(object sender, DesignAutomationReadyEventArgs e)
        {
            RevitRemoteContainer.Initialize(
                e.DesignAutomationData.RevitDoc, e.DesignAutomationData.RevitApp, e.DesignAutomationData.FilePath);

            var testRunner = new RevitTestRunner();
            this.OnStartup(testRunner, application);
        }

        public abstract ExternalDBApplicationResult OnStartup(IRevitTestRunner tests, ControlledApplication application);

        public abstract ExternalDBApplicationResult OnShutdown(ControlledApplication application);

    }

    public class RevitApp : RevitTestRunnerApp
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
