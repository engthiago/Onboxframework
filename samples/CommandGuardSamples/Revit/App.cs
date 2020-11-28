using Autodesk.Revit.UI;
using CommandGuardSamples.ContainerPipelines;
using CommandGuardSamples.Revit.Commands;
using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev;
using Onbox.Revit.VDev.Applications;
using Onbox.Revit.VDev.UI;

namespace CommandGuardSamples.Revit
{
    [ContainerProvider("ac0c8282-d92f-4cdd-af35-d25fc69185c0")]
    public class App : RevitApp
    {
        public override void OnCreateRibbon(IRibbonManager ribbonManager)
        {
            var br = ribbonManager.GetLineBreak();

            var panelManager = ribbonManager.CreatePanel("App Command Panel");
            panelManager.AddPushButton<SingleGuardOnAppCommand>($"Single Guard{br}on App");
        }

        public override Result OnStartup(IContainer container, UIControlledApplication application)
        {
            container.Pipe<SingleGuardConditionPipeline>();

            return Result.Succeeded;
        }

        public override Result OnShutdown(IContainerResolver container, UIControlledApplication application)
        {

            return Result.Succeeded;
        }
    }

}