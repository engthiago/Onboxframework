using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Applications;
using Onbox.Revit.VDev.Commands.ErrorHandlers;
using Onbox.Revit.VDev.Commands.Guards;
using System;

namespace Onbox.Revit.VDev.Commands
{
    /// <summary>
    /// Base class to implement Revit Commands linked to a App Container
    /// <br>It will use a scope of the container declared on the App</br>
    /// </summary>
    public abstract class RevitAppCommand<TApplication> : IExternalCommand, ICanBeGuardedRevitCommand, IRevitDestroyableCommand where TApplication : RevitApp, new()
    {
        /// <summary>
        /// Execution of External Command
        /// </summary>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var commandType = this.GetType();

            // Gets the original container
            IContainer container = GetContainer();

            // Creates an scoped copy of the container
            var scope = container.CreateScope();

            var commandInfo = new CommandInfo(commandType, scope, commandData);

            try
            {
                var commandGuardChecker = scope.Resolve<IRevitCommandGuardChecker>();

                if (commandGuardChecker.CanExecute(commandInfo))
                {
                    // Runs the users Execute command
                    return this.Execute(scope, commandData, ref message, elements);
                }
                else
                {
                    return Result.Cancelled;
                }
            }
            catch (Exception exception)
            {
                var errorHandler = scope.Resolve<IRevitCommandErrorHandler>();
                // If an exception is thrown on user's code, and the handler doesnt handle it, throw the except it back to the stack
                if (!errorHandler.GetHandle(commandInfo, exception))
                {
                    throw;
                }
                else
                {
                    // If the hanlder handles the exception, the command will return succeeded
                    return Result.Succeeded;
                }
            }
            finally
            {
                // Safely calls lifecycle hook 
                try
                {
                    this.OnDestroy(scope);
                }
                catch
                {
                }

                // Cleans up the scoped copy of the container
                scope.Dispose();
            }
        }

        private static IContainer GetContainer()
        {
            var type = typeof(TApplication);
            var containerGuid = ContainerProviderReflector.GetContainerGuid(type);
            var container = RevitContainerProviderBase.GetContainer(containerGuid);
            return container;
        }

        /// <summary>
        /// Execution of External Command
        /// </summary>
        public abstract Result Execute(IContainerResolver container, ExternalCommandData commandData, ref string message, ElementSet elements);

        /// <summary>
        /// External Command lifecycle hook which is called just before the scoped container is disposed.
        /// </summary>
        public virtual void OnDestroy(IContainerResolver container)
        {
        }
    }
}