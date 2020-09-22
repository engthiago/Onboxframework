using Onbox.Abstractions.V7;

namespace Onbox.Revit.V7.Applications
{
    public static class ContainerPipelineExtensions
    {
        public static IContainer Pipe<T>(this IContainer container) where T : class, IContainerPipeline, new()
        {
            var factory = new T();
            var newContainer = factory.Pipe(container);
            return newContainer;
        }
    }
}
