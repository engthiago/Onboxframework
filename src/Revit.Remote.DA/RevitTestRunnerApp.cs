using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using DesignAutomationFramework;
using Onbox.Abstractions.VDev;
using Onbox.Di.VDev;
using System;

namespace Onbox.Revit.Remote.DA
{
    public abstract class RemoteApp : IExternalDBApplication
    {
        private ControlledApplication application;
        private Container container;

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
            //TODO add app, context and more thing to the container, also cleanup container
            // Same things we do on Revit Framework

            this.container = new Container();
            var result = this.OnStartup(this.container, application);

            if (result != ExternalDBApplicationResult.Succeeded)
            {
                e.Succeeded = false;
            }

            e.Succeeded = true;
        }

        public abstract ExternalDBApplicationResult OnStartup(IContainerResolver container, ControlledApplication application);

        public abstract ExternalDBApplicationResult OnShutdown(ControlledApplication application);
    }
}
