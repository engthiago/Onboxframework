using Onbox.Abstractions.VDev;
using System;
using System.Net.Http;

namespace Onbox.Core.VDev.Http
{
    /// <summary>
    /// Helper extensions for <see cref="HttpService"/> and IOC container
    /// </summary>
    public static class HttpExtensions
    {
        /// <summary>
        /// Adds <see cref="IHttpService"/> as <see cref="HttpService"/> to the container
        /// </summary>
        public static IContainer AddHttp(this IContainer container)
        {
            AddHttp(container, null);

            return container;
        }

        /// <summary>
        /// Adds <see cref="IHttpService"/> as <see cref="HttpService"/> to the container with configuration
        /// </summary>
        public static IContainer AddHttp(this IContainer container, Action<HttpSettings> config = null)
        {
            container.ConfigureHttp(config)
                     .AddSingleton<IHttpService, HttpService>();

            container.AddSingleton<IHttpInterceptor, HttpInterceptor>();

            return container;
        }

        /// <summary>
        /// Adds <see cref="IHttpService"/> as <see cref="HttpService"/> to the container with interception and configuration
        /// </summary>
        public static IContainer AddHttp(this IContainer container, IHttpInterceptor httpInterceptor, Action<HttpSettings> config = null)
        {
            container.ConfigureHttp(config)
                     .AddSingleton<IHttpService, HttpService>();

            container.AddSingleton(httpInterceptor);

            return container;
        }

        /// <summary>
        /// Adds <see cref="IHttpService"/> as <see cref="HttpService"/> to the container with interception and configuration
        /// </summary>
        public static IContainer AddHttp<TInterceptor>(this IContainer container, Action<HttpSettings> config = null) where TInterceptor : IHttpInterceptor, new ()
        {
            container.ConfigureHttp(config)
                     .AddSingleton<IHttpService, HttpService>();

            var interceptor = new TInterceptor();
            container.AddSingleton<IHttpInterceptor>(interceptor);

            return container;
        }

        /// <summary>
        /// Adds <see cref="IHttpService"/> as <see cref="HttpService"/> to the container with interception
        /// </summary>
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

        /// <summary>
        /// Adds <see cref="IHttpService"/> as <see cref="HttpService"/> to the container with configuration
        /// </summary>
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