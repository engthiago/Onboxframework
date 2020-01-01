namespace Onbox.Sandbox.Revit.Commands
{
    public partial class Inher
    {
        public interface IJsonService
        {
            T Deserialize<T>(string json);
            string Serialize(object instance);
        }
    }
}
