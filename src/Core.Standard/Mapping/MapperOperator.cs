using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Onbox.Core.VDev.Mapping
{
    internal class PropertyMap
    {
        public object TargetValue { get; set; }
        public List<PropertyData> TargetDataList { get; set; }
    }

    internal class PropertyData
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

        private object mainObject;
        private readonly List<PropertyData> mainObjectPropertyCache;
        private readonly Dictionary<object, PropertyMap> propertyCache;

        public MapperOperator(IMapperActionManager mapperConfigurator)
        {
            this.mapperConfigurator = mapperConfigurator;

            this.mainObjectPropertyCache = new List<PropertyData>();
            this.propertyCache = new Dictionary<object, PropertyMap>();
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

        public object Map(object source)
        {
            if (source == null)
            {
                return null;
            }

            var type = source.GetType();

            var constructorInfo = type.GetConstructor(Type.EmptyTypes);
            if (constructorInfo == null)
            {
                return source;
            }

            var target = Activator.CreateInstance(type);
            return Map(source, target);
        }

        public object Map(object source, object target)
        {
            if (source == null)
            {
                return null;
            }

            var sourceType = source.GetType();
            var targetType = target.GetType();

            try
            {
                if (source is IDictionary sourceDic && target is IDictionary targetDic)
                {
                    foreach (DictionaryEntry item in sourceDic)
                    {
                        var clonedKey = this.Map(item.Key);
                        var clonedValue = this.Map(item.Value);
                        targetDic.Add(clonedKey, clonedValue);
                    }

                    return targetDic;
                }
                else if (source is IList sourceList && target is IList targetList)
                {
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
                            if (targetType.IsArray)
                            {
                                targetList[i] = sourceItem;
                            }
                            else
                            {
                                targetList.Add(sourceItem);
                            }
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
                                if (targetType.IsArray)
                                {
                                    targetList[i] = null;
                                }
                                else
                                {
                                    targetList.Add(null);
                                }

                            }
                            else if (this.propertyCache.ContainsKey(sourceItem))
                            {
                                var properties = this.propertyCache[sourceItem];
                                properties.TargetDataList.Add(data);
                                if (targetType.IsArray)
                                {
                                    targetList[i] = null;
                                }
                                else
                                {
                                    targetList.Add(null);
                                }
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

                                if (targetType.IsArray)
                                {
                                    targetList[i] = null;
                                }
                                else
                                {
                                    targetList.Add(null);
                                }
                            }
                        }
                    }
                }
                else
                {
                    this.CopyProperties(source, target, sourceType, targetType);
                }
            }
            catch
            {
                throw new InvalidCastException($"Can not map between {sourceType.FullName} and {targetType.FullName}");
            }

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
                        if (targetProp.PropertyType.IsArray)
                        {
                            var array = sourceValue as Array;
                            if (array == null || array.Length < 1)
                            {
                                continue;
                            }

                            var list = array as IList;
                            var clone = array.Clone() as IList;

                            clone = this.Map(list, clone) as IList;

                            targetProp.SetValue(target, clone);
                        }
                        else
                        {
                            targetProp.SetValue(target, sourceValue);
                        }
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
                                TargetDataList = new List<PropertyData> { data }
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

        public void MapCachedReferences<TSource>(TSource target) where TSource : new()
        {
            var propCache = this.GetPropertyCache();
            foreach (var item in propCache)
            {
                var targetValue = item.Value.TargetValue;
                foreach (var propData in item.Value.TargetDataList)
                {
                    if (propData.IsList)
                    {
                        propData.TargetList[propData.ListIndex] = targetValue;
                    }
                    else
                    {
                        propData.TargetProp.SetValue(propData.TargetObject, targetValue);
                    }
                }
            }

            var mainObjPropCache = this.GetMainObjectPropertyCache();
            foreach (var item in mainObjPropCache)
            {
                if (item.IsList)
                {
                    item.TargetList[item.ListIndex] = target;
                }
                else
                {
                    item.TargetProp.SetValue(item.TargetObject, target);
                }
            }
        }

        private List<PropertyData> GetMainObjectPropertyCache()
        {
            return this.mainObjectPropertyCache;
        }

        private Dictionary<object, PropertyMap> GetPropertyCache()
        {
            return this.propertyCache;
        }
    }
}