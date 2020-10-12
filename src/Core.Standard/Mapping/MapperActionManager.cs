using System;
using System.Collections.Generic;

namespace Onbox.Core.VDev.Mapping
{
    /// <summary>
    ///  Hold actions that can be performed after objects are mapped
    /// </summary>
    public class MapperActionManager : IMapperActionManager
    {
        private readonly Dictionary<string, Delegate> keys = new Dictionary<string, Delegate>();

        /// <summary>
        /// Adds a new an action that will be run after the map is complete
        /// </summary>
        public void AddMappingPostAction<TSource, TTaget>(Action<TSource, TTaget> action) where TSource : new() where TTaget : new()
        {
            string mapKey = this.GetMapKey<TSource, TTaget>();
            keys[mapKey] = action;
        }

        /// <summary>
        /// Retrieves a mapping post action that was previously defined by <see cref="AddMappingPostAction{TSource, TTaget}(Action{TSource, TTaget})"/>
        /// </summary>
        public Delegate GetMapPostAction(Type source, Type target)
        {
            string mapKey = GetMapKey(source, target);
            if (keys.TryGetValue(mapKey, out Delegate result))
            {
                return result;
            }
            return null;
        }

        private string GetMapKey<TSource, TTaget>() where TSource : new() where TTaget : new()
        {
            return GetMapKey(typeof(TSource), typeof(TTaget));
        }

        private string GetMapKey(Type source, Type target)
        {
            return $"{source.FullName}.{target.FullName}";
        }
    }
}