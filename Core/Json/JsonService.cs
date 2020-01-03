using Newtonsoft.Json;

namespace Onbox.Core.V1.Json
{
    public interface IJsonService
    {
        T Deserialize<T>(string json);
        string Serialize(object instance);
    }

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
