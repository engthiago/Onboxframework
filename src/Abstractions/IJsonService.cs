namespace Onbox.Abstractions.VDev
{
    /// <summary>
    /// Default contract for dealing with serialization / deserialization
    /// </summary>
    public interface IJsonService
    {
        /// <summary>
        /// Deserializes an object
        /// </summary>
        T Deserialize<T>(string json);

        /// <summary>
        /// Serializes an object
        /// </summary>
        string Serialize(object instance);
    }
}