using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands;
using Onbox.Revit.RibbonCommands.VDev.Attributes;

namespace RibbonCommandsApplicationSample.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    [RibbonSplitButton("New Group", PanelName = "Second Panel")]
    public class Hello2Command : RevitAppCommand<App>
    {
        public override Result Execute(IContainerResolver container, ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TaskDialog.Show("Hello 2", "Hello 2!");

            return Result.Succeeded;
        }
    }
}