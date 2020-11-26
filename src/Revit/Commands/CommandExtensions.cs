using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands.Guards;
using System;

namespace Onbox.Revit.VDev.Commands
{
    public static class CommandExtensions
    {
        static internal IContainer AddRevitCommandGuard(this IContainer container)
        {
            var revitCommandGuard = new RevitCommandGuard();
            container.AddSingleton(revitCommandGuard);
            container.AddSingleton<IRevitCommandGuard>(revitCommandGuard);

            return container;
        }

        static public IContainer AddRevitCommandGuard(this IContainer container, Action<ConditionCollection> configuration)
        {
            var revitCommandGuard = new RevitCommandGuard();
            container.AddSingleton(revitCommandGuard);
            container.AddSingleton<IRevitCommandGuard>(revitCommandGuard);

            var collection = new ConditionCollection();
            configuration?.Invoke(collection);

            var builders = collection.GetBuilders();

            foreach (var builder in builders)
            {
                var commandTypes = builder.GetCommandTypes();
                var predicate = builder.GetPredicate();

                foreach (var commandType in commandTypes)
                {
                    revitCommandGuard.AddCommandTypeCondition(commandType, predicate);
                }
            }

            return container;
        }
    }
}
