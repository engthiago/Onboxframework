using Onbox.Abstractions.VDev;
using System;

namespace Onbox.Revit.VDev.Commands.ErrorHandlers
{
    public interface IRevitCommandErrorHandler
    {
        bool GetHandle(ICommandInfo commandInfo, Exception exception);
    }


    public class EmptyRevitCommandErrorHandler : IRevitCommandErrorHandler
    {
        public bool GetHandle(ICommandInfo commandInfo, Exception exception)
        {
            return false;
        }
    }


}
