using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Revit.VDev.Commands.Guards
{
    public interface IRevitCommandGuard
    {
        bool CanExecute(Type commandType, ExternalCommandData commandData);
    }

    public class RevitCommandGuard : IRevitCommandGuard
    {
        private readonly Dictionary<Type, List<Predicate<ICommandInfo>>> commandConditions;

        public RevitCommandGuard()
        {
            commandConditions = new Dictionary<Type, List<Predicate<ICommandInfo>>>();
        }

        public bool CanExecute(Type commandType, ExternalCommandData commandData)
        {
            if (commandConditions.ContainsKey(commandType))
            {
                var conditions = commandConditions[commandType];
                foreach (var condition in conditions)
                {
                    var commandInfo = new CommandInfo(commandType, commandData);
                    var canExecute = condition.Invoke(commandInfo);
                    if (!canExecute)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return true;
            }
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
        IConditionBuilder ForCommand<TCommand>() where TCommand : IRevitCommand;
        IConditionBuilder ExceptCommand<TCommand>() where TCommand : IRevitCommand;
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

        public IConditionBuilder ExceptCommand<TCommand>() where TCommand : IRevitCommand
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


        public IConditionBuilder ForCommand<TCommand>() where TCommand : IRevitCommand
        {
            var type = typeof(TCommand);
            this.addedCommands.Add(type);
            return this;
        }

        public IConditionBuilder ForCommandsInAssembly(Assembly assembly)
        {
            var interFacetype = typeof(IRevitCommand);
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
        private readonly ExternalCommandData commandData;

        internal CommandInfo(Type commandType, ExternalCommandData CommandData)
        {
            this.commandType = commandType;
            this.commandData = CommandData;
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
