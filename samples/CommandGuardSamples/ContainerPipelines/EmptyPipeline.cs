using Onbox.Abstractions.VDev;

namespace CommandGuardSamples.ContainerPipelines
{
    public class EmptyPipeline : IContainerPipeline
    {
        public IContainer Pipe(IContainer container)
        {
            return container;
        }
    }
}
