using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Onbox.Revit.VDev.Commands.Guards
{
    internal class ConditionBuilder : IConditionBuilder, IConditionBuilderProvider
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
}
