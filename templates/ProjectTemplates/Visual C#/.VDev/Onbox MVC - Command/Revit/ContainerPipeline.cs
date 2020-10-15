using Onbox.Abstractions.VDev;
using Onbox.Core.VDev;
using Onbox.Mvc.Revit.VDev;
using $safeprojectname$.Services;

namespace $safeprojectname$.Revit
{
    public class ContainerPipeline : IContainerPipeline
    {
        public IContainer Pipe(IContainer container)
        {
            container.AddOnboxCore();
            container.AddRevitMvc();

            // Adds MessageBoxService to the container
            container.AddSingleton<IMessageService, TaskMessageService>();

            return container;
        }
    }

}