
namespace Onbox.Abstractions.VDev
{
    /// <summary>
    /// Container Resolver pipeline is used to resolve dependencies or cleanup the dependencies of the container
    /// </summary>
    public interface IContainerResolverPipeline
    {
        /// <summary>
        /// Pipes the container resolver and its dependencies
        /// </summary>
        IContainerResolver Pipe(IContainerResolver container);
    }
}