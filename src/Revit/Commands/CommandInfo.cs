using Autodesk.Revit.UI;
using Onbox.Abstractions.VDev;
using System;

namespace Onbox.Revit.VDev.Commands
{
    public class CommandInfo : ICommandInfo
    {
        private readonly Type commandType;
        private readonly IContainerResolver container;
        private readonly ExternalCommandData commandData;

        internal CommandInfo(Type commandType, IContainerResolver container, ExternalCommandData CommandData)
        {
            this.commandType = commandType;
            this.container = container;
            commandData = CommandData;
        }

        public IContainerResolver GetContainer()
        {
            return container;
        }

        public Type GetCommandType()
        {
            return commandType;
        }

        public ExternalCommandData GetCommandData()
        {
            return commandData;
        }
    }
}
