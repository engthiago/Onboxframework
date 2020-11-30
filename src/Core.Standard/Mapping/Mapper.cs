using Onbox.Abstractions.VDev;
using System.Collections;
using System.Collections.Generic;

namespace Onbox.Core.VDev.Mapping
{
    /// <summary>
    /// Onbox implementation of mapper service. Similar to Automapper
    /// </summary>
    public class Mapper : IMapper
    {
        private readonly IMapperOperator mapperOperator;

        /// <summary>
        /// Creates a new mapper object
        /// </summary>
        public Mapper(IMapperOperator mapperOperator)
        {
            this.mapperOperator = mapperOperator;
        }

        /// <summary>
        /// Clones an object (Maps to a new instance)
        /// </summary>
        /// <returns>The cloned object with all properties copied</returns>
        public TSource Clone<TSource>(TSource source) where TSource : new()
        {
            var target = new TSource();

            this.mapperOperator.SetMainObject(source);

            this.mapperOperator.Map(source, target);

            this.MapCachedReferences(target);

            this.mapperOperator.ClearCache();

            return target;
        }

        /// <summary>
        /// Clones an object (Maps to a new instance)
        /// </summary>
        /// <returns>The cloned object with all properties copied</returns>
        public TSource Clone<TSource>(object source) where TSource : new()
        {
            var target = new TSource();

            this.mapperOperator.SetMainObject(source);

            this.mapperOperator.Map(source, target);

            this.MapCachedReferences(target);

            this.mapperOperator.ClearCache();

            return target;
        }

        /// <summary>
        /// Maps one object to another
        /// </summary>
        /// <param name="source">The source object</param>
        /// <param name="target">The target objects that the properties will be copied to</param>
        public void Map<TSorce, TTarget>(TSorce source, TTarget target) where TSorce : new() where TTarget : new()
        {
            this.mapperOperator.SetMainObject(source);

            this.mapperOperator.Map(source, target);

            this.MapCachedReferences(target);

            this.mapperOperator.ClearCache();
        }

        private void MapCachedReferences<TSource>(TSource target) where TSource : new()
        {
            var propCache = this.mapperOperator.GetPropertyCache();
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

            var mainObjPropCache = this.mapperOperator.GetMainObjectPropertyCache();
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

    }
}