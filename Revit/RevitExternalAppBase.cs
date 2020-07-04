using Autodesk.Revit.UI;
using Onbox.Di.V7;
using Onbox.Revit.V7.UI;
using System.Collections.Concurrent;

namespace Onbox.Revit.V7
{
    public abstract class RevitExternalAppBase : RevitExternalAppBase<Container>
    {
    }

    public abstract class RevitExternalAppBase<TContainer> : IExternalApplication
        where TContainer : class, IContainer, new()
    {
        private static RevitEventTracker revitEventTracker;
        private static bool hookedUpEvents;
        private static ConcurrentDictionary<string, IContainer> containers = new ConcurrentDictionary<string, IContainer>();

        internal static IContainer GetContainer(string guid)
        {
            if (containers.TryGetValue(guid, out IContainer container))
            {
                return container;
            }

            return null;
        }

        public abstract void OnStartup(IContainer container, UIControlledApplication application);

        public abstract void OnShutdown(IContainerResolver container, UIControlledApplication application);

        public Result OnStartup(UIControlledApplication application)
        {
            var containerGuid = ContainerProviderReflector.GetContainerGuid(this);

            IContainer container = new TContainer();

            // Do not duplicate existing container with the same GUID, instead, share same container
            if (containers.ContainsKey(containerGuid))
            {
                container = containers[containerGuid];
            }
            else
            {
                containers[containerGuid] = container;
                container.AddSingleton(container);
                container.AddSingleton<IContainerResolver>(container);
            }

            // Only Hook events once, accross all apps using Onbox
            if (!hookedUpEvents)
            {
                hookedUpEvents = true;
                revitEventTracker = new RevitEventTracker(containers);
                revitEventTracker.HookupRevitEvents(application);
            }

            RevitInjector.AddRevitUI(container, application);

            OnStartup(container, application);

            var imageManager = new ImageManager();
            var ribbonManager = new RibbonManager(application, imageManager);
            OnCreateRibbon(ribbonManager);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            revitEventTracker.UnhookRevitEvents(application);

            foreach (var item in containers)
            {
                var container = item.Value;

                try
                {
                    OnShutdown(container, application);
                }
                finally
                {
                    container.Dispose();
                    containers.TryRemove(item.Key, out IContainer removedContainer);
                }
            }

            return Result.Succeeded;
        }

        public virtual void OnCreateRibbon(IRibbonManager ribbonManager)
        {
        }



    }
}
