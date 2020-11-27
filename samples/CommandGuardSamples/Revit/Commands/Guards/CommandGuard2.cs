using Autodesk.Revit.UI;
using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands;
using Onbox.Revit.VDev.Commands.Guards;
using System;
using System.Windows.Forms;

namespace CommandGuardSamples.Commands.Guards
{
    public class CommandGuard2 : IRevitCommandGuard
    {
        public bool CanExecute(ICommandInfo commandInfo)
        {
            var result = MessageBox.Show("Can run Command Guard 2?", "Guard", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                return true;
            }
            return false;
        }
    }
}
