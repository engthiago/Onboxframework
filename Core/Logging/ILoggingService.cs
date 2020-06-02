using System;
using System.Threading.Tasks;

namespace Onbox.Core.V7.Logging
{
    /// <summary>
    /// The contract for logging activities
    /// </summary>
    public interface ILoggingService
    {
        /// <summary>
        /// Logs an error
        /// </summary>
        Task Error(string message);
        /// <summary>
        /// Logs an exception
        /// </summary>
        Task Exception(Exception exception);
        /// <summary>
        /// Logs a message
        /// </summary>
        Task Log(string message);
        /// <summary>
        /// Logs a warning
        /// </summary>
        Task Warning(string message);
    }
}
