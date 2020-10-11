using Onbox.Core.V7.Http;
using System.Net.Http;

namespace $rootnamespace$
{
    /// <summary>
    /// Intercepts http calls, both incoming and outcoming ones. You can modify the request / response before they get processed
    /// </summary>
    public class $safeitemname$ : IHttpInterceptor
    {
        /// <summary>
        /// Before sending a http resquest, this function will run
        /// </summary>
        public void BeforeSending(HttpRequestMessage request)
        {
        }

        /// <summary>
        /// After recieving a http response, this function will run
        /// </summary>
        public void AfterSending(HttpResponseMessage response)
        {
        }

    }
}
