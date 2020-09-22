using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Abstractions.V7;
using Onbox.Revit.V7.Applications;

namespace Onbox.Revit.V7.Commands
{
    /// <summary>
    /// Base class to implement when implementing RevitExternal Commands with containers
    /// </summary>
    public abstract class RevitAppCommand<TApplication> : IExternalCommand where TApplication : RevitApp, new()
    {
        /// <summary>
        /// Execution of External Command
        /// </summary>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Gets the original container
            IContainer container = GetContainer();

            // Creates an scoped copy of the container
            var scope = container.CreateScope();

            try
            {
                // Runs the users Execute command
                return Execute(scope, commandData, ref message, elements);
            }
            catch
            {
                // If an exception is thrown on user's code, trows it back to the stack
                throw;
            }
            finally
            {
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
    }
}
