using Onbox.Revit.VDev.Commands;
using Onbox.Revit.VDev.Commands.ErrorHandlers;
using System;
using System.Windows.Forms;

namespace CommandErrorHandlerSamples.Commands.ErrorHandling
{
    public class SampleCommandErrorHandler : IRevitCommandErrorHandler
    {
        public bool Handle(ICommandInfo commandInfo, Exception exception)
        {
            MessageBox.Show(exception.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return true;
        }
    }
}
