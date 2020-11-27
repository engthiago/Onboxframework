using Onbox.Revit.VDev.Commands;
using Onbox.Revit.VDev.Commands.ErrorHandlers;
using System;
using System.Windows.Forms;

namespace CommandErrorHandlerSamples.Commands.ErrorHandling
{
    public class ShowErrorMessageHandler : IRevitCommandErrorHandler
    {
        public bool Handle(ICommandInfo commandInfo, Exception exception)
        {
            // You can use this to log errors

            MessageBox.Show(exception.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return true;
        }
    }
}
