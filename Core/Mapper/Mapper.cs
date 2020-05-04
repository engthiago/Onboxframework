using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Core.V4.Mapper
{
    /// <summary>
    /// Onbox Mapper can Clone objects and map properties and lists
    /// </summary>
    public class Mapper
    {
        private readonly MappingConfigurator mappingConfigurator;

        /// <summary>
        /// Creates a new mapper object
        /// </summary>
        /// <param name="mappingConfigurator">Optional configurator that allows for adding post map actions, you can pass null if not needed</param>
        public Mapper(MappingConfigurator mappingConfigurator)
        {
            this.mappingConfigurator = mappingConfigurator;
            if (this.mappingConfigurator == null)
            {
                this.mappingConfigurator = new MappingConfigurator();
            }
        }

        /// <summary>
        /// Clones an object
        /// </summary>
        /// <returns>The cloned object with all properties copied</returns>
        public TSorce Map<TSorce>(object source) where TSorce : new()
        {
            var target = new TSorce();
            mappingConfigurator.Map(source, target);
            return target;
        }

        /// <summary>
        /// Maps one object to another
        /// </summary>
        /// <param name="source">The source object</param>
        /// <param name="target">The target objects that the properties will be copied to</param>
        public void Map<TSorce, TTarget>(TSorce source, TTarget target) where TSorce : new() where TTarget : new()
        {
            mappingConfigurator.Map(source, target);
        }
    }

    /// <summary>
    /// A Mapping configurator that will tell <see cref="Mapper"/> how to map objects
    /// </summary>
    public class MappingConfigurator
    {
        Dictionary<string, Delegate> keys = new Dictionary<string, Delegate>();

        /// <summary>
        /// Adds a new an action that will be run after the map is complete
        /// </summary>
        public void AddMappingPostAction<TSource, TTaget>(Action<TSource, TTaget> action) where TSource : new() where TTaget : new()
        {
            var mapKey = typeof(TSource).FullName + typeof(TTaget).FullName;
            keys[mapKey] = action;
        }

        private Delegate GetMapFunction(Type source, Type target)
        {
            var mapKey = source.FullName + target.FullName;
            if (keys != null && keys.TryGetValue(mapKey, out Delegate result))
            {
                return result;
            }
            return null;
        }

        internal object Map(object source, object target)
        {
            if (source == null)
            {
                return null;
            }

            var sourceType = source.GetType();
            var targetType = target.GetType();

            try
            {
                if (sourceType.IsGenericType)
                {
                    if (sourceType.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IList<>)) &&
                        targetType.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IList<>)))
                    {
                        var listSource = source as IList;
                        var listTaget = target as IList;
                        foreach (var item in listSource)
                        {
                            listTaget.Add(Map(item));
                        }
                    }
                    else
                    {
                        CopyProperties(source, target, sourceType, targetType);
                    }
                }
                else
                {
                    CopyProperties(source, target, sourceType, targetType);
                }
            }
            catch
            {
                throw new InvalidCastException($"Can not map between {sourceType.FullName} and {targetType.FullName}");
            }

            var mapingFunction = GetMapFunction(sourceType, targetType);
            if (mapingFunction != null)
            {
                mapingFunction.DynamicInvoke(source, target);
            }

            return target;
        }

        private void CopyProperties(object source, object target, Type sourceType, Type targetType)
        {
            var targetProps = targetType.GetProperties();
            var props = sourceType.GetProperties();
            foreach (var prop in props)
            {
                if (!prop.CanRead)
                {
                    continue;
                }

                var value = prop.GetValue(source);

                var targetProp = targetProps.FirstOrDefault(p => p.CanWrite && p.Name == prop.Name && p.PropertyType == prop.PropertyType);
                if (targetProp != null)
                {
                    var constructorInfo = targetProp.PropertyType.GetConstructor(Type.EmptyTypes);
                    if (constructorInfo == null)
                    {
                        targetProp.SetValue(target, value);
                    }
                    else
                    {
                        var propValue = Map(value);
                        targetProp.SetValue(target, propValue);
                    }
                }
            }
        }

        internal object Map(object source)
        {
            if (source == null)
            {
                return null;
            }

            var type = source.GetType();
            var target = Activator.CreateInstance(type);
            return Map(source, target);
        }
    }
}
