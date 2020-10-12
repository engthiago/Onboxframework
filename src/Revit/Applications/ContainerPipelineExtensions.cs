using Onbox.Abstractions.VDev;

namespace Onbox.Revit.VDev.Applications
{
    /// <summary>
    /// Extension pipe the container
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

        /// <summary>
        /// Container Resolver pipeline is used to resolve dependencies or cleanup the dependencies of the container
        /// </summary>
        public static IContainerResolver Pipe<T>(this IContainerResolver container) where T: class, IContainerResolverPipeline, new()
        {
            var pipeline = new T();
            var newContainer = pipeline.Pipe(container);
            return newContainer;
        }
    }
}