using Onbox.Abstractions.V7;
using Onbox.Core.V7;
using $safeprojectname$.Services;

namespace $safeprojectname$.Revit
{
    public class ContainerPipeline : IContainerPipeline
    {
        public IContainer Pipe(IContainer container)
        {
            container.AddOnboxCore();

            // Adds MessageBoxService to the container
            container.AddSingleton<IMessageService, TaskMessageService>();

            return container;
        }
    }

}