using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Abstractions.V7;
using Onbox.Di.V7;

namespace Onbox.Revit.V7.Commands
{
    /// <summary>
    /// An indepentend Revit Command that will create a new container instance and use it during the command runtime. Use this when an ExternalApplication is not necessary.
    /// <br>It uses a Container Pipeline to compose the container.</br>
    /// <br>After the command finishes the container will be disposed.</br>
    /// </summary>
    public abstract class RevitContainerCommandBase<TContainerPipeline, TContainer> : IExternalCommand, IRevitDestroyableCommand where TContainerPipeline : class, IContainerPipeline, new()
        where TContainer : class, IContainer, new()
    {
        /// <summary>
        /// Execution of External Command.
        /// </summary>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var pipeline = new TContainerPipeline();
            var container = new TContainer();
            var newContainer = pipeline.Pipe(container);

            try
            {
                // Runs the users Execute command
                return Execute(newContainer, commandData, ref message, elements);
            }
            catch
            {
                // If an exception is thrown on user's code, throws it back to the stack
                throw;
            }
            finally
            {
                // Safely calls lifecycle hook 
                try
                {
                    this.OnDestroy(newContainer);
                }
                catch
                {
                }

                // Cleans up the container
                container.Dispose();
            }
        }

        /// <summary>
        /// Execution of External Command.
        /// </summary>
        public abstract Result Execute(IContainerResolver container, ExternalCommandData commandData, ref string message, ElementSet elements);

        /// <summary>
        /// External Command lifecycle hook which is called just before the container is disposed.
        /// </summary>
        public abstract void OnDestroy(IContainerResolver container);
    }

    /// <summary>
    /// An indepentend Revit Command that will create a new container instance and use it during the command runtime. Use this when an ExternalApplication is not necessary.
    /// <br>It uses a Container Pipeline to compose the container.</br>
    /// <br>After the command finishes the container will be disposed.</br>
    /// </summary>
    public abstract class RevitContainerCommand<TContainerPipeline> : RevitContainerCommandBase<TContainerPipeline, Container> where TContainerPipeline : class, IContainerPipeline, new()
    {
    }
}
