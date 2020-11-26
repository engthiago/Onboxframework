using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands;

namespace CommandGuardSamples.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class SingleGuardOnAppCommand : RevitAppCommand<App>
    {
        public override Result Execute(IContainerResolver container, ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TaskDialog.Show("Guard", "Ran the command!");

            return Result.Succeeded;
        }
    }
}