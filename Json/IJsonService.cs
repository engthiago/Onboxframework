namespace Onbox.Json.V1
{
    public interface IJsonService
    {
        T Deserialize<T>(string json);
        string Serialize(object instance);
    }
}
