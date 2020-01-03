using Onbox.Core.V1;
using Onbox.Core.V1.Messaging;
using Onbox.Di.V1;
using Onbox.Mvc.V1.Messaging;

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

            container.AddSingleton<SomeService>();
            container.AddTransient<OtherService>();
            container.AddTransient<OtherOtherService>();
            container.AddTransient<OtherOtherOtherService>();

            container.AddOnboxCore();

            return container;
        }
    }

    public class SomeService
    {
        public SomeService(OtherService service)
        {
        }
    }

    public class OtherService
    {
        public OtherService(OtherOtherService service)
        {
        }
    }

    public class OtherOtherService
    {
        public OtherOtherService(OtherOtherOtherService service)
        {
        }
    }

    public class OtherOtherOtherService
    {
        public OtherOtherOtherService()
        {

        }
    }
}
