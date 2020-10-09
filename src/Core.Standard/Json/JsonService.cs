using Newtonsoft.Json;
using Onbox.Abstractions.V7;

namespace Onbox.Core.V7.Json
{
    /// <summary>
    /// Default implementation for <see cref="IJsonService"/>, it uses Newtonsoft Json
    /// </summary>
    public class JsonService : IJsonService
    {
        readonly JsonSerializerSettings settings;

        /// <summary>
        /// Constructor
        /// </summary>
        public JsonService(JsonSerializerSettings settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Deserializes an object
        /// </summary>
        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        /// <summary>
        /// Serializes an object
        /// </summary>
        public string Serialize(object instance)
        {
            var json = JsonConvert.SerializeObject(instance, settings);
            return json;
        }
    }
}
