using System.Net.Http;

namespace Onbox.Core.V7.Http
{
    /// <summary>
    /// Holds actions to run before and / or after every http request
    /// </summary>
    public interface IHttpInterceptor
    {
        /// <summary>
        /// Before the request is sent, can be used to inject headers, tokens or validation
        /// </summary>
        void BeforeSending(HttpRequestMessage request);

        /// <summary>
        /// When the request gets from remote, can be used to catch exceptions, process failed requests or validate results
        /// </summary>
        void AfterSending(HttpResponseMessage response);
    }
}