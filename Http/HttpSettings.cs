using Onbox.Abstractions.V7;

namespace Onbox.Http.V7.Http
{
    /// <summary>
    /// Preferences for the default implementation of <see cref="IHttpService"/>
    /// </summary>
    public class HttpSettings
    {
        /// <summary>
        /// Timeout in milliseconds
        /// </summary>
        public int Timeout { get; set; } = 2500;

        /// <summary>
        /// The "cache-control" header will be set to no-cache by default
        /// </summary>
        public bool AllowCache { get; set; }
    }
}
