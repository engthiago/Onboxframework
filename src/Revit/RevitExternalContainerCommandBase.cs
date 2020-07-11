using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Abstractions.V7;

namespace Onbox.Revit.V7
{
    public abstract class RevitExternalContainerCommandBase<TContainerFactory> : IExternalCommand where TContainerFactory : class, IContainerFactory, new()
    {
        /// <summary>
        /// Execution of External Command
        /// </summary>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var factory = new TContainerFactory();
            var container = factory.CreateContainer();

            try
            {
                // Runs the users Execute command
                return Execute(container, commandData, ref message, elements);
            }
            catch
            {
                // If an exception is thrown on user's code, trows it back to the stack
                throw;
            }
            finally
            {
                container.Dispose();
            }
        }

        /// <summary>
        /// Execution of External Command
        /// </summary>
        public abstract Result Execute(IContainerResolver container, ExternalCommandData commandData, ref string message, ElementSet elements);
    }
}
