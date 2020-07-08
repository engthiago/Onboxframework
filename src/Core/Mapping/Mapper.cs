using Onbox.Abstractions.V7;

namespace Onbox.Core.V7.Mapping
{
    /// <summary>
    /// Onbox Mapper can clone objects and map properties and lists
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
        public TSorce Map<TSorce>(object source) where TSorce : new()
        {
            var target = new TSorce();
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
