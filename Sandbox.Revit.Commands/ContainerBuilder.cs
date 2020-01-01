using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Onbox.Di.V1;
using Onbox.Sandbox.Revit.Commands.Extensions;

namespace Onbox.Sandbox.Revit.Commands
{
    public static class ContainerBuilder
    {
        public static Container Build()
        {
            var container = new Container();
            container.AddTransient<IOrderView, OrderView>();
            container.AddTransient<IServerService, MockServerService>();
            container.AddTransient<IMessageService, MessageBoxService>();

            container.AddJson(config =>
            {
                config.ContractResolver = new CamelCasePropertyNamesContractResolver();
                config.NullValueHandling = NullValueHandling.Ignore;
                config.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            return container;
        }
    }
}
