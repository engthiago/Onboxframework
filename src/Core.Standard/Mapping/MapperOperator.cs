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
        public bool IsList { get; set; }
        public int ListIndex { get; set; }
        public IList TargetList { get; set; }
    }

    /// <summary>
    /// Support contract for performing mapping
    /// </summary>
    public class MapperOperator : IMapperOperator
    {
        private readonly IMapperActionManager mapperConfigurator;

        private readonly Dictionary<object, PropertyMap> propertyCache;

        private readonly List<PropertyData> mainObjectPropertyCache;
        private object mainObject;

        /// <summary>
        /// Constructor
        /// </summary>
        public MapperOperator(IMapperActionManager mapperConfigurator)
        {
            this.mapperConfigurator = mapperConfigurator;

            this.propertyCache = new Dictionary<object, PropertyMap>();

            this.mainObjectPropertyCache = new List<PropertyData>();
        }

        public void SetMainObject(object mainObject)
        {
            this.mainObject = mainObject;
        }

        public void ClearCache()
        {
            this.mainObject = null;
            this.mainObjectPropertyCache.Clear();
            this.propertyCache.Clear();
        }

        public List<PropertyData> GetMainObjectPropertyCache()
        {
            return this.mainObjectPropertyCache;
        }

        public Dictionary<object, PropertyMap> GetPropertyCache()
        {
            return this.propertyCache;
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
                    var sourceList = source as IList;
                    var targetList = target as IList;

                    ConstructorInfo constructorInfo = null;
                    for (int i = 0; i < sourceList.Count; i++)
                    {
                        var sourceItem = sourceList[i];
                        if (i == 0) 
                        {
                            constructorInfo = sourceItem.GetType().GetConstructor(Type.EmptyTypes);
                        }

                        if (constructorInfo == null)
                        {
                            targetList.Add(sourceItem);
                        }
                        else
                        {
                            var data = new PropertyData
                            {
                                IsList = true,
                                ListIndex = i,
                                TargetProp = null,
                                TargetObject = target,
                                TargetList = targetList,
                            };

                            if (sourceItem == this.mainObject)
                            {
                                this.mainObjectPropertyCache.Add(data);
                                targetList.Add(null);
                            }
                            else if (this.propertyCache.ContainsKey(sourceItem))
                            {
                                var properties = this.propertyCache[sourceItem];
                                properties.TargetDataList.Add(data);
                                targetList.Add(null);
                            }
                            else
                            {
                                var propertyMap = new PropertyMap
                                {
                                    TargetDataList = new List<PropertyData> { data }
                                };

                                this.propertyCache.Add(sourceItem, propertyMap);
                                var targetValue = this.Map(sourceItem);
                                propertyMap.TargetValue = targetValue;

                                targetList.Add(null);
                            }
                        }
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
                        var data = new PropertyData
                        {
                            TargetProp = targetProp,
                            TargetObject = target
                        };

                        // If the sourceValue is equal to the mainObject, it ALSO means that we are on a Refence loop,
                        // we should cache this property to apply when the mapping is done.
                        if (sourceValue == this.mainObject)
                        {
                            this.mainObjectPropertyCache.Add(data);
                        }
                        else if (this.propertyCache.ContainsKey(sourceValue))
                        {
                            // If sourceValue was already mapped, it ALSO means that we are on a Reference loop
                            // we should cache this property to apply when the mapping is done.
                            var properties = this.propertyCache[sourceValue];
                            properties.TargetDataList.Add(data);
                        }
                        else
                        {
                            var propertyMap = new PropertyMap
                            {
                                TargetDataList = new List<PropertyData> {  data }
                            };
                            this.propertyCache.Add(sourceValue, propertyMap);
                            var targetValue = this.Map(sourceValue);
                            propertyMap.TargetValue = targetValue;
                            //targetProp.SetValue(target, targetValue);
                        }
                    }
                }
            }
        }
    }
}