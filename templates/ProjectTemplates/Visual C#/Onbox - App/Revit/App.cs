using Onbox.Abstractions.V7;
using Onbox.Core.V7;
using Onbox.Revit.V7;
using Onbox.Revit.V7.Applications;
using Onbox.Revit.V7.UI;
using Autodesk.Revit.UI;
using $safeprojectname$.Revit.Commands.Availability;
using $safeprojectname$.Revit.Commands;
using $safeprojectname$.Services;

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
        }
		
        public override Result OnStartup(IContainer container, UIControlledApplication application)
        {
            // Here you can add all necessary dependencies to the container
            container.AddOnboxCore();
			
			// Add TaskDialog Service the message service
			container.AddSingleton<IMessageService, TaskMessageService>();
			
			return Result.Succeeded;
        }

        public override Result OnShutdown(IContainerResolver container, UIControlledApplication application)
        {
            // No Need to cleanup the Container, the framework will do it for you
			return Result.Succeeded;
        }
    }

}
