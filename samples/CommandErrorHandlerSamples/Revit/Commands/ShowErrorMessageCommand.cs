using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommandErrorHandlerSamples.ContainerPipelines;
using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands;

namespace CommandErrorHandlerSamples.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class ShowErrorMessageCommand : RevitContainerCommand<ShowErrorMessagePipeline>
    {
        public override Result Execute(IContainerResolver container, ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            throw new System.Exception("Boooom!");
        }
    }
}