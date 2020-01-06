using System.Threading.Tasks;

namespace Onbox.Core.V1.Logging
{
    public interface ILoggingService
    {
        Task Error(string message);
        Task Log(string message);
        Task Warning(string message);
    }
}
