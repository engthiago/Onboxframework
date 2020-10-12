using Onbox.Abstractions.VDev;
using Onbox.Mvc.Revit.VDev;
using Onbox.Mvc.VDev.Messaging;

namespace $safeprojectname$
{
    public class MvcContainerPipeline : IContainerPipeline
    {
        public IContainer Pipe(IContainer container)
        {
			// Adds Support for Revit Mvc
			container.AddRevitMvc();
			
            // Adds MessageBoxService to the container
            container.AddSingleton<IMessageService, MessageBoxService>();

            return container;
        }
    }

}