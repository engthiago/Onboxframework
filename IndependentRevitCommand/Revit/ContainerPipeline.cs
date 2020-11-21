using IndependentRevitCommand.Services;
using Onbox.Abstractions.VDev;
using Onbox.Core.VDev;

namespace IndependentRevitCommand.Revit
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