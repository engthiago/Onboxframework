using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommandGuardSamples.ContainerPipelines;
using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands;
using Onbox.Revit.VDev.Commands.Attributes;

namespace CommandGuardSamples.Commands
{
    [IgnoreCommandGuardConditions]
    [Transaction(TransactionMode.Manual)]
    public class IgnoreGuardConditionCommand : RevitContainerCommand<SingleGuardConditionPipeline>
    {
        public override Result Execute(IContainerResolver container, ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TaskDialog.Show("Guard", "Ran the command!");

            return Result.Succeeded;
        }
    }
}