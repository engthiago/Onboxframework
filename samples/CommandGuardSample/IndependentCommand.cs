using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands;
using Onbox.Revit.VDev.Commands.Attributes;
using Onbox.Revit.VDev.Commands.Guards;
using System;
using System.Windows.Forms;

namespace CommandGuardSample
{
    [CommandGuard(typeof(CommandGuardSample))]
    //[RevitCommandGuard(typeof(CommandGuardSample2))]
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
            var result = MessageBox.Show("Can run?", "Command Guard 1", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                return true;
            }
            return false;
        }
    }

    public class CommandGuardSample2 : IRevitCommandGuard
    {
        public bool CanExecute(Type commandType, IContainerResolver container, ExternalCommandData commandData)
        {
            var result = MessageBox.Show("Can run?", "Command Guard 2", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                return true;
            }
            return false;
        }
    }
}
