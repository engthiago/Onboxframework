using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Abstractions.V7;

namespace Onbox.Revit.V7.Commands
{
    /// <summary>
    /// An indepentend Revit Command that will create a new container instance and use it during the command runtime. Use this when an ExternalApplication is not necessary.
    /// <br>After the command finishes the container will be destroyed / disposed.</br>
    /// </summary>
    /// <typeparam name="TContainerFactory"></typeparam>
    public abstract class IndependentRevitCommandBase<TContainerFactory> : IExternalCommand where TContainerFactory : class, IContainerFactory, new()
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
                // If an exception is thrown on user's code, throws it back to the stack
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
