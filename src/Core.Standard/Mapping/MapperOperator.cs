using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Onbox.Core.VDev.Mapping
{
    public class PropertyMap
    {
        public object TargetValue { get; set; }
        public List<PropertyData> TargetDataList { get; set; }
    }

    public class PropertyData
    {
        public PropertyInfo TargetProp { get; set; }
        public object TargetObject { get; set; }
    }

    /// <summary>
    /// Support contract for performing mapping
    /// </summary>
    public class MapperOperator : IMapperOperator
    {
        private readonly IMapperActionManager mapperConfigurator;
        private readonly Dictionary<object, PropertyMap> cache;
        private readonly List<PropertyData> propertyCache;
        private object mainObject;

        /// <summary>
        /// Constructor
        /// </summary>
        public MapperOperator(IMapperActionManager mapperConfigurator)
        {
            this.mapperConfigurator = mapperConfigurator;
            this.cache = new Dictionary<object, PropertyMap>();
            this.propertyCache = new List<PropertyData>();
        }

        public void SetMain(object mainObject)
        {
            this.mainObject = mainObject;
        }

        public void ClearCache()
        {
            this.mainObject = null;
            this.cache.Clear();
        }

        public Dictionary<object, PropertyMap> GetMappingCache()
        {
            return this.cache;
        }

        /// <summary>
        /// Maps properties of one object to another
        /// </summary>
        public object Map(object source)
        {
            if (source == null)
            {
                return null;
            }

            var type = source.GetType();
            var target = Activator.CreateInstance(type);
            return Map(source, target);
        }

        /// <summary>
        /// Creates a new object as a deep copy of the input object
        /// </summary>
        public object Map(object source, object target)
        {
            if (source == null)
            {
                return null;
            }

            var sourceType = source.GetType();
            var targetType = target.GetType();

            //try
            //{
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
            //}
            //catch
            //{
            //    throw new InvalidCastException($"Can not map between {sourceType.FullName} and {targetType.FullName}");
            //}

            var mapingFunction = this.mapperConfigurator.GetMapPostAction(sourceType, targetType);
            if (mapingFunction != null)
            {
                mapingFunction.DynamicInvoke(source, target);
            }

            return target;
        }

        private void CopyProperties(object source, object target, Type sourceType, Type targetType)
        {
            var targetProps = targetType.GetProperties();
            var sourceProps = sourceType.GetProperties();
            foreach (var sourceProp in sourceProps)
            {
                if (!sourceProp.CanRead)
                {
                    continue;
                }

                var sourceValue = sourceProp.GetValue(source);
                if (sourceValue == null)
                {
                    continue;
                }

                var targetProp = targetProps.FirstOrDefault(p => p.CanWrite && p.Name == sourceProp.Name && p.PropertyType == sourceProp.PropertyType);
                if (targetProp != null)
                {
                    var constructorInfo = targetProp.PropertyType.GetConstructor(Type.EmptyTypes);
                    if (constructorInfo == null)
                    {
                        targetProp.SetValue(target, sourceValue);
                    }
                    else
                    {
                        if (sourceValue == this.mainObject)
                        {
                            var data = new PropertyData
                            {
                                TargetProp = targetProp,
                                TargetObject = target
                            };
                            this.propertyCache.Add(data);
                        }
                        else if (!this.cache.ContainsKey(sourceValue))
                        {
                            var propertyMap = new PropertyMap
                            {
                                TargetDataList = new List<PropertyData>
                                {
                                    new PropertyData
                                    {
                                        TargetProp = targetProp,
                                        TargetObject = target
                                    }
                                }
                            };
                            this.cache.Add(sourceValue, propertyMap);
                            var targetValue = this.Map(sourceValue);
                            propertyMap.TargetValue = targetValue;
                            targetProp.SetValue(target, targetValue);
                        }
                        else
                        {
                            var data = new PropertyData
                            {
                                TargetProp = targetProp,
                                TargetObject = target
                            };
                            var properties = this.cache[sourceValue];
                            properties.TargetDataList.Add(data);
                        }
                    }
                }
            }
        }

        public List<PropertyData> GetPropertyCache()
        {
            return this.propertyCache;
        }
    }
}