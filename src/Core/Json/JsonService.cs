using Newtonsoft.Json;
using Onbox.Abstractions.V7;

namespace Onbox.Core.V7.Json
{
    public class JsonService : IJsonService
    {
        readonly JsonSerializerSettings settings;

        public JsonService(JsonSerializerSettings settings)
        {
            this.settings = settings;
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        public string Serialize(object instance)
        {
            var json = JsonConvert.SerializeObject(instance, settings);
            return json;
        }
    }
}
