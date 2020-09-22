using Onbox.Abstractions.V7;

namespace Onbox.Revit.V7.Applications
{
    /// <summary>
    /// Extension methods to compose the container
    /// </summary>
    public static class ContainerPipelineExtensions
    {

        /// <summary>
        /// Adds dependencies specified on the pipeline. It is used to compose the container with these dependencies
        /// </summary>
        public static IContainer Pipe<T>(this IContainer container) where T : class, IContainerPipeline, new()
        {
            var pipeline = new T();
            var newContainer = pipeline.Pipe(container);
            return newContainer;
        }
    }
}
