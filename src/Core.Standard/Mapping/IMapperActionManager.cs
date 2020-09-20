using System;

namespace Onbox.Core.V7.Mapping
{
    /// <summary>
    ///  Hold actions that can be performed after objects are mapped
    /// </summary>
    public interface IMapperActionManager
    {
        /// <summary>
        /// Adds a new an action that will be run after the map is complete
        /// </summary>
        void AddMappingPostAction<TSource, TTaget>(Action<TSource, TTaget> action) where TSource : new() where TTaget : new();
        /// <summary>
        /// Retrieves a mapping post action that was previously defined by <see cref="AddMappingPostAction{TSource, TTaget}(Action{TSource, TTaget})"/>
        /// </summary>
        Delegate GetMapPostAction(Type source, Type target);
    }
}