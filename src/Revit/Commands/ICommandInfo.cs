using Autodesk.Revit.UI;
using Onbox.Abstractions.VDev;
using System;

namespace Onbox.Revit.VDev.Commands
{
    /// <summary>
    /// Contains Information about the current running Revit Command.
    /// </summary>
    public interface ICommandInfo
    {
        /// <summary>
        /// Gets the type of the Revit Command.
        /// </summary>
        /// <returns>The type of the command.</returns>
        Type GetCommandType();
        /// <summary>
        /// Gets the command data from the Revit Command.
        /// </summary>
        /// <returns>The external command data.</returns>
        ExternalCommandData GetCommandData();
        /// <summary>
        /// Gets the current container from the Revit Command.
        /// </summary>
        /// <returns>Current Container.</returns>
        IContainerResolver GetContainer();
    }
}
