using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands.Guards;
using System;

namespace Onbox.Revit.VDev.Commands
{
    public static class CommandExtensions
    {
        static internal IContainer AddRevitCommandGuardConditions(this IContainer container)
        {
            var commandGuardChecker = new RevitCommandGuardChecker();
            container.AddSingleton(commandGuardChecker);
            container.AddSingleton<IRevitCommandGuardChecker>(commandGuardChecker);

            return container;
        }

        static public IContainer AddRevitCommandGuardConditions(this IContainer container, Action<ConditionCollection> configuration)
        {
            var commandGuardChecker = new RevitCommandGuardChecker();
            container.AddSingleton(commandGuardChecker);
            container.AddSingleton<IRevitCommandGuardChecker>(commandGuardChecker);

            var collection = new ConditionCollection();
            configuration?.Invoke(collection);

            var builders = collection.GetBuilders();

            foreach (var builder in builders)
            {
                var commandTypes = builder.GetCommandTypes();
                var predicate = builder.GetPredicate();

                foreach (var commandType in commandTypes)
                {
                    commandGuardChecker.AddCommandTypeCondition(commandType, predicate);
                }
            }

            return container;
        }
    }
}
