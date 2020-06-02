using System.Net.Http;

namespace Onbox.Core.V7.Http
{
    public interface IHttpInterceptor
    {
        void BeforeSending(HttpRequestMessage request);
        void AfterSending(HttpResponseMessage response);
    }
}