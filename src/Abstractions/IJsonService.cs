namespace Onbox.Abstractions.V7
{
    public interface IJsonService
    {
        T Deserialize<T>(string json);
        string Serialize(object instance);
    }
}
