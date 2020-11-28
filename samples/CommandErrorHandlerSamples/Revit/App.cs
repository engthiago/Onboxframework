using Autodesk.Revit.UI;
using CommandErrorHandlerSamples.ContainerPipelines;
using CommandErrorHandlerSamples.Revit.Commands;
using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev;
using Onbox.Revit.VDev.Applications;
using Onbox.Revit.VDev.UI;

namespace CommandErrorHandlerSamples.Revit
{
    [ContainerProvider("7a83ee6f-05dd-4f46-849e-80445042700d")]
    public class App : RevitApp
    {
        public override void OnCreateRibbon(IRibbonManager ribbonManager)
        {
            var br = ribbonManager.GetLineBreak();
            var panelManager = ribbonManager.CreatePanel("CommandErrorHandlerSamples");
            panelManager.AddPushButton<ExceptionControlledByAppCommand>($"Error{br}Handling");
        }

        public override Result OnStartup(IContainer container, UIControlledApplication application)
        {
            container.Pipe<ShowErrorMessagePipeline>();
            
            return Result.Succeeded;
        }

        public override Result OnShutdown(IContainerResolver container, UIControlledApplication application)
        {

            return Result.Succeeded;
        }
    }

}