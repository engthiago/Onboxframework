using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands.ErrorHandlers;
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

        /// <summary>
        /// Adds support for handling exceptions globally in Revit Commands.
        /// </summary>
        /// <typeparam name="THandler">A class that inherits from <see cref="IRevitCommandErrorHandler"/> to handle the exceptions.</typeparam>
        /// <param name="container">The current container.</param>
        /// <returns>The Container.</returns>
        static public IContainer AddRevitCommandErrorHandling<THandler>(this IContainer container) where THandler : class, IRevitCommandErrorHandler, new()
        {
            var hanlder = new THandler();
            container.AddSingleton(hanlder);
            container.AddSingleton<IRevitCommandErrorHandler, THandler>();

            return container;
        }
    }
}
