namespace Onbox.Sandbox.Revit.Commands
{
    public interface IJsonService
    {
        T Deserialize<T>(string json);
        string Serialize(object instance);
    }
}
