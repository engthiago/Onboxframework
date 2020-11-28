using CommandErrorHandlerSamples.Revit.Commands.ErrorHandling;
using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands;

namespace CommandErrorHandlerSamples.ContainerPipelines
{
    public class ThrowErrorAnywaysPipeline : IContainerPipeline
    {
        public IContainer Pipe(IContainer container)
        {
            container.AddRevitCommandErrorHandling<ThrowErrorAnywaysHandler>();

            return container;
        }
    }

}