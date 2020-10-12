namespace Onbox.Core.VDev.Mapping
{
    /// <summary>
    /// Support contract for performing mapping
    /// </summary>
    public interface IMapperOperator
    {
        /// <summary>
        /// Maps properties of one object to another
        /// </summary>
        object Map(object source, object target);

        /// <summary>
        /// Creates a new object as a deep copy of the input object
        /// </summary>
        object Map(object source);
    }
}