using MvcSimpleCommand.Services;
using MvcSimpleCommand.Views;
using Onbox.Abstractions.VDev;
using Onbox.Core.VDev;
using Onbox.Mvc.Revit.VDev;

namespace MvcSimpleCommand.Revit
{
    public class ContainerPipeline : IContainerPipeline
    {
        public IContainer Pipe(IContainer container)
        {
            container.AddOnboxCore();
            container.AddRevitMvc();

            // Adds MessageBoxService to the container
            container.AddSingleton<IMessageService, TaskMessageService>();

            container.AddTransient<IMvcSampleView, MvcSampleView>();

            return container;
        }
    }

}