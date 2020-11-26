using Autodesk.Revit.UI;
using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands.Guards;
using System;
using System.Windows.Forms;

namespace CommandGuardSamples.Commands.Guards
{
    public class CommandGuard1 : IRevitCommandGuard
    {
        public bool CanExecute(Type commandType, IContainerResolver container, ExternalCommandData commandData)
        {
            var result = MessageBox.Show("Can run Command Guard 1?", "Guard", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                return true;
            }
            return false;
        }
    }
}
