using Onbox.Revit.VDev.Commands;
using Onbox.Revit.VDev.Commands.ErrorHandlers;
using System;

namespace $rootnamespace$
{
    public class $safeitemname$ : IRevitCommandErrorHandler
    {
        public bool Handle(ICommandInfo commandInfo, Exception exception)
        {
            // You can use this method to log errors, display error messages, swallow exceptions and so on.

            return true;
        }
    }
}
