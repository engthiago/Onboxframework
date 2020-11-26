using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands;
using Onbox.Revit.VDev.Commands.Attributes;
using Onbox.Revit.VDev.Commands.Guards;
using System;

namespace CommandGuardSample
{
    [CommandGuard(typeof(CommandGuardSample))]
    //[RevitCommandGuard(typeof(CommandGuardSample2))]
    //[IgnoreCommandGuards]
    [IgnoreCommandGuardConditions]
    [Transaction(TransactionMode.Manual)]
    public class IndependentCommand : RevitContainerCommand<CommandGuardPipeline>
    {
        public override Result Execute(IContainerResolver container, ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            TaskDialog.Show("Command", "Ran the command!");

            return Result.Succeeded;
        }
    }

    public class CommandGuardSample : IRevitCommandGuard
    {
        public bool CanExecute(Type commandType, IContainerResolver container, ExternalCommandData commandData)
        {
            TaskDialog.Show("Command", "Guard 1");

            return true;
        }
    }

    public class CommandGuardSample2 : IRevitCommandGuard
    {
        public bool CanExecute(Type commandType, IContainerResolver container, ExternalCommandData commandData)
        {
            TaskDialog.Show("Command", "Guard 2");

            return true;
        }
    }
}
