using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Onbox.Core.V7.Mapping
{
    /// <summary>
    /// A Mapping configurator that will tell <see cref="Mapper"/> how to map objects
    /// </summary>
    public class MapperConfigurator : IMapperConfigurator
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

        public Delegate GetMapFunction(Type source, Type target)
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
