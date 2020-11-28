using Onbox.Revit.VDev.Commands;
using Onbox.Revit.VDev.Commands.ErrorHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandErrorHandlerSamples.Revit.Commands.ErrorHandling
{
    class ThrowErrorAnywaysHandler : IRevitCommandErrorHandler
    {
        public bool Handle(ICommandInfo commandInfo, Exception exception)
        {
            // You can use this to log errors

            return false;
        }
    }
}
