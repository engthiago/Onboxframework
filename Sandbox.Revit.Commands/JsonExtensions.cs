using Newtonsoft.Json;
using Onbox.Di.V1;
using System;
using static Onbox.Sandbox.Revit.Commands.Inher;

namespace Onbox.Sandbox.Revit.Commands
{
    public static class JsonExtensions
    {
        public static Container AddJson(this Container container)
        {
            return AddJson(container, null);
        }

        public static Container AddJson(this Container container, Action<JsonSerializerSettings> config)
        {
            container.ConfigureJson(config)
                     .AddTransient<IJsonService, JsonService>();

            return container;
        }

        public static Container ConfigureJson(this Container container, Action<JsonSerializerSettings> config)
        {
            var settings = new JsonSerializerSettings();
            config?.Invoke(settings);

            container.AddSingleton(settings);

            return container;
        }
    }
}
