using CommandErrorHandlerSamples.Commands.ErrorHandling;
using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands;

namespace CommandErrorHandlerSamples.ContainerPipelines
{
    public class SampleCommandErrorHandlingPipeline : IContainerPipeline
    {
        public IContainer Pipe(IContainer container)
        {
            container.AddRevitCommandErrorHandling<SampleCommandErrorHandler>();

            return container;
        }
    }

}