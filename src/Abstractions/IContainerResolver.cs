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
        /// <br>An exception will be thrown for non registered abstract types in the dependency tree</br>
        /// <br>The container will automatically resolve non registered concrete types in the dependency tree</br>
        /// <br>Check ResolveOrNull() for exception free version</br>
        /// </summary>
        T Resolve<T>() where T : class;

        /// <summary>
        /// Asks the container for a new instance of a type
        /// <br>It will return null if any non registered abstract type in the dependency tree</br>
        /// </summary>
        T ResolveOrNull<T>() where T : class;

        /// <summary>
        /// Asks the container for a new instance of a type
        /// </summary>
        object Resolve(Type type);

        /// <summary>
        /// Creates a scoped context copy of this container
        /// </summary>
        IContainerResolver CreateScope();

        /// <summary>
        /// Checks if a singleton instance for this type is registered in the container
        /// </summary>
        /// <typeparam name="T">The target type, abstract or concrete</typeparam>
        /// <returns>true if the type is registered, false if not</returns>
        bool HasSingletonInstance<T>();

        /// <summary>
        /// Checks if a scoped instance for this type is registered in the container
        /// </summary>
        /// <typeparam name="T">The target type, abstract or concrete</typeparam>
        /// <returns>true if the type is registered, false if not</returns>
        bool HasScopedInstance<T>();

        /// <summary>
        /// Reports if this container is a scope of a container
        /// </summary>
        bool IsScope();
    }
}