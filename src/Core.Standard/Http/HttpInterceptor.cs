using System;
using System.Net.Http;

namespace Onbox.Core.V7.Http
{
    /// <summary>
    /// Provides the ability to intercept requests made by <see cref="HttpService"/>
    /// </summary>
    public class HttpInterceptor : IHttpInterceptor
    {
        internal Action<HttpRequestMessage> beforeSending;
        internal Action<HttpResponseMessage> afterSending;

        /// <summary>
        /// Right before all requests are sent, this method will be called.
        /// <br>You have the ability to modify the request like adding authentication tokens, identification, data modifiers and so on.</br>
        /// </summary>
        public void BeforeSending(HttpRequestMessage request)
        {
            this.beforeSending?.Invoke(request);
        }

        /// <summary>
        /// Right all responses are processed, this method will be called. You have the ability to modify the response before they are serialized.
        /// <br>You can use this to log incoming messages, process errors and so on.</br>
        /// </summary>
        public void AfterSending(HttpResponseMessage response)
        {
            this.afterSending?.Invoke(response);
        }

    }
}
