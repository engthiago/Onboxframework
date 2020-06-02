using Onbox.Di.V7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Core.V7.Http
{
    public static class HttpExtensions
    {
        public static IContainer AddHttp(this IContainer container)
        {
            return AddHttp(container, null);
        }

        public static IContainer AddHttp(this IContainer container, Action<HttpSettings> config)
        {
            container.ConfigureHttp(config)
                     .AddSingleton<IHttpService, HttpService>();

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
