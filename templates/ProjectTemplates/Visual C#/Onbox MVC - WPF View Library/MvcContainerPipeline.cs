using Onbox.Abstractions.V7;
using Onbox.Mvc.Revit.V7;
using Onbox.Mvc.V7.Messaging;

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
