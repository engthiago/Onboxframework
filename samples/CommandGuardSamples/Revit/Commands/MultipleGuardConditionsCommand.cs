using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommandGuardSamples.ContainerPipelines;
using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands;

namespace CommandGuardSamples.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class MultipleGuardConditionsCommand : RevitContainerCommand<MultipleGuardConditionsPipeline>
    {
        public override Result Execute(IContainerResolver container, ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TaskDialog.Show("Guard", "Ran the command!");

            return Result.Succeeded;
        }
    }
}