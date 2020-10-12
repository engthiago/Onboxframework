using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Onbox.Abstractions.VDev;
using System;

namespace Onbox.Core.VDev.Json
{
    /// <summary>
    /// Helper extensions for json parser and IOC container
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// Adds JsonService with default settings for Revit: CamelCaseProperties, Ignores nulls, and Ignores Reference loop
        /// </summary>
        public static IContainer AddJson(this IContainer container)
        {
            return AddJson(container, null);
        }

        /// <summary>
        /// Adds JsonService with custom settings configuration
        /// </summary>
        public static IContainer AddJson(this IContainer container, Action<JsonSerializerSettings> config)
        {
            container.ConfigureJson(config)
                     .AddSingleton<IJsonService, JsonService>();

            return container;
        }

        /// <summary>
        /// Runtime configuration for custom Json Settings
        /// </summary>
        public static IContainer ConfigureJson(this IContainer container, Action<JsonSerializerSettings> config)
        {
            // Default Settings
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };

            config?.Invoke(settings);
            container.AddSingleton(settings);

            return container;
        }
    }
}