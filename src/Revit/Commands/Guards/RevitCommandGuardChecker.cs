using Autodesk.Revit.UI;
using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Revit.VDev.Commands.Guards
{
    public interface IRevitCommandGuardChecker
    {
        bool CanExecute(Type commandType, IContainerResolver container, ExternalCommandData commandData);
    }

    public class RevitCommandGuardChecker : IRevitCommandGuardChecker
    {
        private readonly Dictionary<Type, List<Predicate<ICommandInfo>>> commandConditions;

        public RevitCommandGuardChecker()
        {
            commandConditions = new Dictionary<Type, List<Predicate<ICommandInfo>>>();
        }

        public bool CanExecute(Type commandType, IContainerResolver container, ExternalCommandData commandData)
        {
            // Loop through all RevitCommandGuardAttributes to see if we can run the command
            var guardAttrType = typeof(CommandGuardAttribute);
            var attributes = commandType.GetCustomAttributes().Where(a => a.GetType() == guardAttrType);
            if (attributes.Any())
            {
                var methodInfo = guardAttrType.GetMethod(nameof(CommandGuardAttribute.GetCommandGuardType));
                foreach (var attribute in attributes)
                {
                    var guardType = methodInfo.Invoke(attribute, null) as Type;

                    if (guardType != null)
                    {
                        var guard = Activator.CreateInstance(guardType) as IRevitCommandGuard;
                        if (guard != null)
                        {
                            if (!guard.CanExecute(commandType, container, commandData))
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            var ignoreConditionsType = typeof(IgnoreCommandGuardConditionsAttribute);
            var attributeData = commandType.CustomAttributes;

            // If IgnoreConditions is added to the command, we wont check contions, just allow the command to run
            if (attributeData.Any(a => a.AttributeType == ignoreConditionsType))
            {
                return true;
            }

            // Loop through all conditions to see if we can run the command
            if (commandConditions.ContainsKey(commandType))
            {
                var conditions = commandConditions[commandType];
                foreach (var condition in conditions)
                {
                    var commandInfo = new CommandInfo(commandType, container, commandData);
                    var canExecute = condition.Invoke(commandInfo);
                    if (!canExecute)
                    {
                        return false;
                    }
                }
                return true;
            }

            return true;
        }

        internal void AddCommandTypeCondition(Type commandType, Predicate<ICommandInfo> predicate)
        {
            if (commandConditions.ContainsKey(commandType))
            {
                var commandCondition = commandConditions[commandType];
                commandCondition.Add(predicate);
            }
            else
            {
                commandConditions.Add(commandType, new List<Predicate<ICommandInfo>> { predicate });
            }
        }
    }

    public interface IConditionBuilderProvider
    {
        Predicate<ICommandInfo> GetPredicate();
        List<Type> GetCommandTypes();
    }

    public interface IConditionBuilder
    {
        IConditionBuilder ForCommandsInAssembly(Assembly assembly);
        IConditionBuilder ForCommand<TCommand>() where TCommand : ICanBeGuardedRevitCommand;
        IConditionBuilder ExceptCommand<TCommand>() where TCommand : ICanBeGuardedRevitCommand;
        IConditionBuilder WhereCommandType(Func<Type, bool> commandTypeFilter);
        void CanExecute(Predicate<ICommandInfo> predicate);
    }

    public class ConditionBuilder : IConditionBuilder, IConditionBuilderProvider
    {
        private readonly List<Type> addedCommands;
        private readonly List<Type> removedCommands;

        private Predicate<ICommandInfo> predicate;

        private readonly List<Func<Type, bool>> filters;

        public ConditionBuilder()
        {
            this.addedCommands = new List<Type>();
            this.removedCommands = new List<Type>();
            this.filters = new List<Func<Type, bool>>();
        }

        public void CanExecute(Predicate<ICommandInfo> predicate)
        {
            this.predicate = predicate;
        }

        public IConditionBuilder ExceptCommand<TCommand>() where TCommand : ICanBeGuardedRevitCommand
        {
            var type = typeof(TCommand);
            this.removedCommands.Add(type);
            return this;
        }
        public IConditionBuilder WhereCommandType(Func<Type, bool> commandTypeFilter)
        {
            this.filters.Add(commandTypeFilter);
            return this;
        }


        public IConditionBuilder ForCommand<TCommand>() where TCommand : ICanBeGuardedRevitCommand
        {
            var type = typeof(TCommand);
            this.addedCommands.Add(type);
            return this;
        }

        public IConditionBuilder ForCommandsInAssembly(Assembly assembly)
        {
            var interFacetype = typeof(ICanBeGuardedRevitCommand);
            var types = assembly.GetTypes().Where(t => t.GetInterfaces().FirstOrDefault(i => i == interFacetype) != null);
            this.addedCommands.AddRange(types);
            return this;
        }

        public Predicate<ICommandInfo> GetPredicate()
        {
            return this.predicate;
        }

        public List<Type> GetCommandTypes()
        {
            var commands = this.addedCommands.Distinct().Except(removedCommands.Distinct());

            foreach (var filter in this.filters)
            {
                commands = commands.Where(filter);
            }

            return commands.ToList();
        }


    }

    public interface IConditionCollection
    {
        IConditionBuilder AddCondition();
    }

    public class ConditionCollection
    {
        readonly List<IConditionBuilderProvider> builders;

        public ConditionCollection()
        {
            this.builders = new List<IConditionBuilderProvider>();
        }

        public ConditionBuilder AddCondition()
        {
            var conditionBuilder = new ConditionBuilder();
            this.builders.Add(conditionBuilder);
            return conditionBuilder;
        }

        internal List<IConditionBuilderProvider> GetBuilders()
        {
            return this.builders;
        }
    }

    public interface ICommandInfo
    {
        Type GetCommandType();
        ExternalCommandData GetCommandData();
    }

    public class CommandInfo : ICommandInfo
    {
        private readonly Type commandType;
        private readonly IContainerResolver container;
        private readonly ExternalCommandData commandData;

        internal CommandInfo(Type commandType, IContainerResolver container, ExternalCommandData CommandData)
        {
            this.commandType = commandType;
            this.container = container;
            this.commandData = CommandData;
        }

        public IContainerResolver GetContainer()
        {
            return this.container;
        }

        public Type GetCommandType()
        {
            return this.commandType;
        }

        public ExternalCommandData GetCommandData()
        {
            return this.commandData;
        }
    }
}
