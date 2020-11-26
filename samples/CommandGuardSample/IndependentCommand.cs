using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands;

namespace CommandGuardSample
{
    [Transaction(TransactionMode.Manual)]
    public class IndependentCommand : RevitContainerCommand<CommandGuardPipeline>
    {
        public override Result Execute(IContainerResolver container, ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            TaskDialog.Show("Command","Ran the command!");

            return Result.Succeeded;
        }
    }
}
