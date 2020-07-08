namespace Onbox.Abstractions.V7
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
        /// Clones an object (Maps to a new instance)
        /// </summary>
        /// <returns>The cloned object with all properties copied</returns>
        TSorce Map<TSorce>(object source) where TSorce : new();
    }
}
