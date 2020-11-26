using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands.Guards;
using System;

namespace Onbox.Revit.VDev.Commands
{
    /// <summary>
    /// Command Extensions for Onbox's DI system.
    /// </summary>
    public static class CommandExtensions
    {
        static internal IContainer AddRevitCommandGuardConditions(this IContainer container)
        {
            var commandGuardChecker = new RevitCommandGuardChecker();
            container.AddSingleton(commandGuardChecker);
            container.AddSingleton<IRevitCommandGuardChecker>(commandGuardChecker);

            return container;
        }

        /// <summary>
        /// Adds support for guarding Revit Commands with Guard Conditions.
        /// </summary>
        /// <param name="container">The current container.</param>
        /// <param name="configuration">The configuration to add guard conditions.</param>
        /// <returns>The container.</returns>
        static public IContainer AddRevitCommandGuardConditions(this IContainer container, Action<IConditionCollection> configuration)
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
