using System;
using System.Net.Http;

namespace Onbox.Core.V7.Http
{
    public class HttpInterceptor : IHttpInterceptor
    {
        internal Action<HttpRequestMessage> beforeSending;
        internal Action<HttpResponseMessage> afterSending;

        public HttpInterceptor()
        {
        }

        public void BeforeSending(HttpRequestMessage request)
        {
            this.beforeSending?.Invoke(request);
        }

        public void AfterSending(HttpResponseMessage response)
        {
            this.afterSending?.Invoke(response);
        }

    }
}
