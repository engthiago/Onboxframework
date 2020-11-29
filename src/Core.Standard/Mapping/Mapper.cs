using Onbox.Abstractions.VDev;
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

            this.mapperOperator.SetMain(source);

            var result = this.mapperOperator.Map(source, target);

            var cache = this.mapperOperator.GetMappingCache();
            foreach (var item in cache)
            {
                foreach (var target2 in item.Value.TargetDataList)
                {
                    target2.TargetProp.SetValue(target2.TargetObject, item.Value.TargetValue);
                }
            }

            var propCache = this.mapperOperator.GetPropertyCache();
            foreach (var item in propCache)
            {
                item.TargetProp.SetValue(item.TargetObject, result);
            }

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
            this.mapperOperator.Map(source, target);
            return target;
        }

        /// <summary>
        /// Maps one object to another
        /// </summary>
        /// <param name="source">The source object</param>
        /// <param name="target">The target objects that the properties will be copied to</param>
        public void Map<TSorce, TTarget>(TSorce source, TTarget target) where TSorce : new() where TTarget : new()
        {
            this.mapperOperator.Map(source, target);
        }
    }
}