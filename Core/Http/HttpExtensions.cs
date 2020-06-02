using Onbox.Di.V7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Core.V7.Http
{
    public static class HttpExtensions
    {
        public static IContainer AddHttp(this IContainer container)
        {
            AddHttp(container, null);

            return container;
        }

        public static IContainer AddHttp(this IContainer container, Action<HttpSettings> config = null)
        {
            container.ConfigureHttp(config)
                     .AddSingleton<IHttpService, HttpService>();

            container.AddSingleton<IHttpInterceptor, HttpInterceptor>();

            return container;
        }

        public static IContainer AddHttp(this IContainer container, IHttpInterceptor httpInterceptor, Action<HttpSettings> config = null)
        {
            container.ConfigureHttp(config)
                     .AddSingleton<IHttpService, HttpService>();

            container.AddSingleton(httpInterceptor);

            return container;
        }

        public static IContainer AddHttp<TInterceptor>(this IContainer container, Action<HttpSettings> config = null) where TInterceptor : IHttpInterceptor, new ()
        {
            container.ConfigureHttp(config)
                     .AddSingleton<IHttpService, HttpService>();

            var interceptor = new TInterceptor();
            container.AddSingleton<IHttpInterceptor>(interceptor);

            return container;
        }

        public static IContainer AddHttp(this IContainer container, Action<HttpSettings> config = null, Action<HttpRequestMessage> beforeSendingAction = null, Action<HttpResponseMessage> afterSendingAction = null)
        {
            container.ConfigureHttp(config)
                     .AddSingleton<IHttpService, HttpService>();

            var interceptor = new HttpInterceptor();
            interceptor.beforeSending = beforeSendingAction;
            interceptor.afterSending = afterSendingAction;

            container.AddSingleton<IHttpInterceptor>(interceptor);

            return container;
        }

        public static IContainer ConfigureHttp(this IContainer container, Action<HttpSettings> config)
        {
            var settings = new HttpSettings
            {
                AllowCache = false,
                Timeout = 25000,
            };

            config?.Invoke(settings);

            container.AddSingleton(settings);

            return container;
        }
    }
}
