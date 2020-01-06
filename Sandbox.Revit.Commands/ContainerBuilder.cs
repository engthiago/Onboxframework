using Onbox.Core.V1;
using Onbox.Core.V1.Logging;
using Onbox.Core.V1.Messaging;
using Onbox.Di.V1;
using Onbox.Mvc.V1;
using Onbox.Mvc.V1.Messaging;

namespace Onbox.Sandbox.Revit.Commands
{
    public static class ContainerBuilder
    {
        public static Container Build()
        {
            var container = Container.Default();

            container.AddSingleton<IMessageService, MessageBoxService>();

            container.AddFileLogging();

            //container.AddSingleton<SomeService>();
            //container.AddSingleton<SomeOtherService>();
            //container.AddSingleton<SomeOtherOtherService>();

            container.AddTransient<ITestWindow, TestWindow>();
            
            return container;
        }
    }


    public class SomeService
    {
        public SomeService(IMessageService messageService, ILoggingService loggingService)
        {
            messageService.Show("Instantiating some service");
            loggingService.Log("Instantiating some service");
        }
    }

    public class SomeOtherService
    {
        public SomeOtherService(SomeOtherOtherService service, IMessageService messageService)
        {
            messageService.Show("Instantiating some other service");
        }
    }

    public class SomeOtherOtherService
    {
        public SomeOtherOtherService(IMessageService messageService)
        {
            messageService.Show("Instantiating some other other service");
        }
    }
}
