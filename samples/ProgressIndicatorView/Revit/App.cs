using Autodesk.Revit.UI;
using Onbox.Abstractions.VDev;
using Onbox.Core.VDev;
using Onbox.Mvc.Revit.VDev;
using Onbox.Revit.VDev;
using Onbox.Revit.VDev.Applications;
using Onbox.Revit.VDev.UI;
using ProgressIndicatorView.Revit.Commands;
using ProgressIndicatorView.Revit.Commands.Availability;

namespace ProgressIndicatorView.Revit
{
    [ContainerProvider("55625aa1-2a24-4fa5-a5f9-ca7c2c1bd72d")]
    public class App : RevitApp
    {
        public override void OnCreateRibbon(IRibbonManager ribbonManager)
        {
            // Here you can create Ribbon tabs, panels and buttons

            var br = ribbonManager.GetLineBreak();

            // Adds a new Ribbon Tab with a new Panel
            var panelManager = ribbonManager.CreatePanel("ProgressIndicatorView", "Hello Panel");
            panelManager.AddPushButton<ProgressIndicatorViewCommand, AvailableOnProject>($"Progress{br}Indicator Sample", "onbox_logo");
        }

        public override Result OnStartup(IContainer container, UIControlledApplication application)
        {
            // Here you can add all necessary dependencies to the container

            container.AddOnboxCore();

            // Progress Indicator is added when RevitMVC is added
            container.AddRevitMvc();

            return Result.Succeeded;
        }

        public override Result OnShutdown(IContainerResolver container, UIControlledApplication application)
        {
            // No Need to cleanup the Container, the framework will do it for you
            return Result.Succeeded;
        }
    }

}