using System.Text;
using System.Threading.Tasks;

namespace Onbox.Core.V7.Mapping
{
    /// <summary>
    /// Onbox Mapper can clone objects and map properties and lists
    /// </summary>
    public interface IMapper
    {
        /// <summary>
        /// Clones an object (Maps to a new instance)
        /// </summary>
        /// <returns>The cloned object with all properties copied</returns>
        void Map<TSorce, TTarget>(TSorce source, TTarget target) where TSorce : new() where TTarget : new();

        /// <summary>
        /// Maps one object to another
        /// </summary>
        /// <param name="source">The source object</param>
        /// <param name="target">The target objects that the properties will be copied to</param>
        TSorce Map<TSorce>(object source) where TSorce : new();
    }

    /// <summary>
    /// Onbox Mapper can clone objects and map properties and lists
    /// </summary>
    public class Mapper : IMapper
    {
        private readonly MapperSettings mappingConfigurator;

        /// <summary>
        /// Creates a new mapper object
        /// </summary>
        /// <param name="mappingConfigurator">Optional configurator that allows for adding post map actions, you can pass null if not needed</param>
        public Mapper(MapperSettings mappingConfigurator)
        {
            this.mappingConfigurator = mappingConfigurator;
            if (this.mappingConfigurator == null)
            {
                this.mappingConfigurator = new MapperSettings();
            }
        }

        /// <summary>
        /// Clones an object (Maps to a new instance)
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
}
