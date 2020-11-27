using CommandErrorHandlerSamples.Commands.ErrorHandling;
using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands;

namespace CommandErrorHandlerSamples.ContainerPipelines
{
    public class SwallowErrorPipeline : IContainerPipeline
    {
        public IContainer Pipe(IContainer container)
        {
            container.AddRevitCommandErrorHandling<SwallowErrorHandler>();

            return container;
        }
    }

}