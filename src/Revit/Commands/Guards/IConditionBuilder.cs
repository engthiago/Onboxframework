using System;
using System.Reflection;

namespace Onbox.Revit.VDev.Commands.Guards
{
    /// <summary>
    /// Adds or Removes Commands from CommandGuard Conditions. The order doesnt matter.
    /// </summary>
    public interface IConditionBuilder
    {
        /// <summary>
        /// Adds all commands in the target Assembly.
        /// </summary>
        /// <param name="assembly">The targeted assembly</param>
        /// <returns>The condition builder.</returns>
        IConditionBuilder ForCommandsInAssembly(Assembly assembly);
        /// <summary>
        /// Adds a specific command to the condition guard.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <returns>The condition builder.</returns>
        IConditionBuilder ForCommand<TCommand>() where TCommand : ICanBeGuardedRevitCommand;
        /// <summary>
        /// Explicit removes a specific command to the condition guard. Once a command is explicit removed, it can not be added back to the condition.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <returns>The condition builder.</returns>
        IConditionBuilder ExceptCommand<TCommand>() where TCommand : ICanBeGuardedRevitCommand;
        /// <summary>
        /// Filters commands from the condition guard.
        /// </summary>
        /// <param name="commandTypeFilter">The filter predicate</param>
        /// <returns>The condition builder.</returns>
        IConditionBuilder WhereCommandType(Func<Type, bool> commandTypeFilter);
        /// <summary>
        /// Creates the condition guard.
        /// </summary>
        /// <param name="predicate">The condition for the command to run.</param>
        void CanExecute(Predicate<ICommandInfo> predicate);
    }
}
