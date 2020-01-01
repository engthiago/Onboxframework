using Newtonsoft.Json;

namespace Onbox.Sandbox.Revit.Commands
{
    public partial class Inher
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
}
