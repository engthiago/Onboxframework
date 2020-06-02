using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
