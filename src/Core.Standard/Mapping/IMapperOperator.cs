using System.Collections.Generic;

namespace Onbox.Core.VDev.Mapping
{
    /// <summary>
    /// Support contract for performing mapping.
    /// </summary>
    public interface IMapperOperator
    {
        /// <summary>
        /// Maps properties of one object to another.
        /// </summary>
        object Map(object source, object target);

        /// <summary>
        /// Creates a new object as a deep copy of the input object.
        /// </summary>
        object Map(object source);

        /// <summary>
        /// Sets the main object to be mapped or cloned, this needs to be called before any mapping takes place.
        /// </summary>
        void SetMainObject(object mainObject);

        /// <summary>
        /// Maps all the references cached by the mapping operation, this needs to be called after any mapping takes place.
        /// </summary>
        void MapCachedReferences<TSource>(TSource target) where TSource : new();

        /// <summary>
        /// Clears the mapping refence cache. this needs to be called after MapCachedReferences is called.
        /// </summary>
        void ClearCache();
    }
}