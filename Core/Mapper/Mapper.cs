using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Core.V4.Mapper
{
    public class Mapper
    {
        public TSorce Map<TSorce>(object source) where TSorce : new()
        {
            var target = new TSorce();
            MappingConfigurator.Map(source, target);
            return target;
        }

        public void Map<TSorce, TTarget>(TSorce source, TTarget target) where TSorce : new() where TTarget : new()
        {
            MappingConfigurator.Map(source, target);
        }
    }

    public static class MappingConfigurator
    {
        static Dictionary<string, Delegate> keys = new Dictionary<string, Delegate>();

        public static void AddMappingPostAction<TSource, TTaget>(Action<TSource, TTaget> action) where TSource : new() where TTaget : new()
        {
            var mapKey = typeof(TSource).FullName + typeof(TTaget).FullName;
            keys[mapKey] = action;
        }

        private static Delegate GetMapFunction(Type source, Type target)
        {
            var mapKey = source.FullName + target.FullName;
            if (keys.TryGetValue(mapKey, out Delegate result))
            {
                return result;
            }
            return null;
        }

        internal static object Map(object source, object target)
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

        private static void CopyProperties(object source, object target, Type sourceType, Type targetType)
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

        internal static object Map(object source)
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
