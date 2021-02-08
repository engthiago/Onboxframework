using Autodesk.Revit.UI;
using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev;
using Onbox.Revit.VDev.Applications;
using Onbox.Revit.VDev.UI;
using Onbox.Revit.VDev.RibbonCommands;

namespace RibbonCommandsApplicationSample.Revit
{
    [ContainerProvider("2c68acf1-609e-493c-87d1-ad6ba66ffc33")]
    public class App : RevitApp
    {
        public override void OnCreateRibbon(IRibbonManager ribbonManager)
        {
            ribbonManager.AddRibbonCommands(cfg =>
            {
                cfg.TabName = "Ribbon Commands";
                cfg.DefaultPanelName = "Sample Panel";

                cfg.AddPanel("Sample Panel");
                cfg.AddPanel("Second Panel");
            });
        }

        public override Result OnStartup(IContainer container, UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public override Result OnShutdown(IContainerResolver container, UIControlledApplication application)
        {
            // No Need to cleanup the Container, the framework will do it for you
            return Result.Succeeded;
        }
    }

}