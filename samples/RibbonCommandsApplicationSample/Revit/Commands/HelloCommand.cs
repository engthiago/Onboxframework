using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands;
using Onbox.Revit.RibbonCommands.VDev.Attributes;

namespace RibbonCommandsApplicationSample.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    [RibbonPushButton(FirstLine = "Hey", PanelPriority = 1, Image = "onbox_logo")]
    [RibbonSplitButton("New Group", SplitGroupPriority = 1, PanelName = "Second Panel")]
    public class HelloCommand : RevitAppCommand<App>
    {
        public override Result Execute(IContainerResolver container, ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TaskDialog.Show("Hello Command", "Hello!");

            return Result.Succeeded;
        }
    }
}