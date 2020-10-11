using Onbox.Abstractions.V7;
using Onbox.Core.V7;
using Onbox.Revit.V7;
using Onbox.Revit.V7.Applications;
using Onbox.Mvc.Revit.V7;
using Onbox.Mvc.V7.Messaging;
using Onbox.Revit.V7.UI;
using Autodesk.Revit.UI;
using $safeprojectname$.Revit.Commands.Availability;
using $safeprojectname$.Revit.Commands;
using $safeprojectname$.Views;

namespace $safeprojectname$.Revit
{
    [ContainerProvider("$guid1$")]
    public class App : RevitApp
    {
		public override void OnCreateRibbon(IRibbonManager ribbonManager)
        {
            // Here you can create Ribbon tabs, panels and buttons

            var br = ribbonManager.GetLineBreak();

            // Adds a Ribbon Panel to the Addins tab
            var addinPanelManager = ribbonManager.CreatePanel("$safeprojectname$");
            addinPanelManager.AddPushButton<HelloCommand, AvailableOnProject>($"Hello{br}Framework", "onbox_logo");

            // Adds a new Ribbon Tab with a new Panel
            var panelManager = ribbonManager.CreatePanel("$safeprojectname$", "Hello Panel");
            panelManager.AddPushButton<HelloCommand, AvailableOnProject>($"Hello{br}Framework", "onbox_logo");
			panelManager.AddPushButton<WPFViewCommand, AvailableOnProject>($"Hello{br}WPF", "autodesk_logo");
        }
		
        public override Result OnStartup(IContainer container, UIControlledApplication application)
        {
            // Here you can add all necessary dependencies to the container

            container.AddOnboxCore();
            container.AddRevitMvc();

            // Registers IWPFView to the container
            // Views should ALWAYS be added as Transients
            container.AddTransient<IHelloWpfView, HelloWpfView>();

            // Adds MessageBoxService to the container
            container.AddSingleton<IMessageService, MessageBoxService>();
			
			return Result.Succeeded;
        }

        public override Result OnShutdown(IContainerResolver container, UIControlledApplication application)
        {
            // No Need to cleanup the Container, the framework will do it for you
			return Result.Succeeded;
        }
    }

}
