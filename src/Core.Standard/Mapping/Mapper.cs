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

            this.mapperOperator.MapCachedReferences(target);

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

            this.mapperOperator.MapCachedReferences(target);

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

            this.mapperOperator.MapCachedReferences(target);

            this.mapperOperator.ClearCache();
        }

    }
}