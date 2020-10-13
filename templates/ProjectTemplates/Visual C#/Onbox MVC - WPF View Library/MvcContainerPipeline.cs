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

            return container;
        }
    }

}