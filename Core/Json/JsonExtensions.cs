using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Onbox.Di.V7;
using System;

namespace Onbox.Core.V7.Json
{
    public static class JsonExtensions
    {
        /// <summary>
        /// Adds JsonService with default settings for Revit: CamelCaseProperties, Ignores nulls, and Ignores Reference loop
        /// </summary>
        public static Container AddJson(this Container container)
        {
            return AddJson(container, null);
        }

        /// <summary>
        /// Adds JsonService with custom settings configuration
        /// </summary>
        public static Container AddJson(this Container container, Action<JsonSerializerSettings> config)
        {
            container.ConfigureJson(config)
                     .AddSingleton<IJsonService, JsonService>();

            return container;
        }

        /// <summary>
        /// Runtime configuration for custom Json Settings
        /// </summary>
        public static Container ConfigureJson(this Container container, Action<JsonSerializerSettings> config)
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
