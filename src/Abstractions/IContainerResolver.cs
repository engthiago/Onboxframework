using System;

namespace Onbox.Abstractions.VDev
{
    /// <summary>
    /// Onbox's IOC container read only contract
    /// </summary>
    public interface IContainerResolver: IDisposable
    {
        /// <summary>
        /// Asks the container for a new instance of a type
        /// </summary>
        T Resolve<T>();

        /// <summary>
        /// Asks the container for a new instance of a type
        /// </summary>
        object Resolve(Type type);

        /// <summary>
        /// Creates a scoped context copy of this container
        /// </summary>
        IContainerResolver CreateScope();

        /// <summary>
        /// Reports if this container is a scope of a container
        /// </summary>
        bool IsScope();
    }
}