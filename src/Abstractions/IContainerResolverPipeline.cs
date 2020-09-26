using System;
using System.Collections.Generic;
using System.Text;

namespace Onbox.Abstractions.V7
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
