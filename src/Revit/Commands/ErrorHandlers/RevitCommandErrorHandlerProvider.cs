using Onbox.Abstractions.VDev;
using System;

namespace Onbox.Revit.VDev.Commands.ErrorHandlers
{
    /// <summary>
    /// Provides functionality to allow Revit Command Exception Handling.
    /// <br>A common use case would be logging errors.</br>
    /// </summary>
    public interface IRevitCommandErrorHandler
    {
        /// <summary>
        /// Ability to handle the exception. If returned false, the exception will be thrown anyways.
        /// </summary>
        /// <param name="commandInfo">The information about the current command.</param>
        /// <param name="exception">The exception that is about to be thrown.</param>
        /// <returns>Either the exception will be thrown or be handle by this handler.</returns>
        bool Handle(ICommandInfo commandInfo, Exception exception);
    }


    internal class EmptyRevitCommandErrorHandler : IRevitCommandErrorHandler
    {
        public bool Handle(ICommandInfo commandInfo, Exception exception)
        {
            return false;
        }
    }


}
