using Autodesk.Revit.UI;
using Onbox.Abstractions.VDev;
using Onbox.Revit.Abstractions.VDev;
using System;
using System.Collections.Concurrent;

namespace Onbox.Revit.VDev
{
    /// <summary>
    /// The most base class for a container provider, it will manage containers and events during its lifetime
    /// </summary>
    public abstract class RevitContainerProviderBase
    {
        internal static ConcurrentDictionary<string, IContainer> containers = new ConcurrentDictionary<string, IContainer>();

        internal static IContainer GetContainer(string guid)
        {
            if (string.IsNullOrWhiteSpace(guid))
            {
                throw new Exception($"Application should have a valid GUID to locate the container");
            }

            if (containers.TryGetValue(guid, out IContainer container))
            {
                return container;
            }

            throw new Exception($"No container found for this Application with specified GUID: {guid}");
        }

        internal IContainer HookUpContainer(IContainer container, string containerGuid)
        {
            // Do not duplicate existing container with the same GUID, instead, share same container
            if (containers.ContainsKey(containerGuid))
            {
                container = containers[containerGuid];
            }
            else
            {
                containers[containerGuid] = container;
            }

            return container;
        }

        internal IContainer InjectContainerToItself(IContainer container)
        {
            container.AddSingleton(container);
            container.AddSingleton<IContainerResolver>(container);
            return container;
        }

        internal IContainer UnhookContainer(string containerGuid, IContainer container)
        {
            container.Dispose();
            containers.TryRemove(containerGuid, out _);
            return container;
        }

        internal IContainer HookupRevitContext(UIControlledApplication application, IContainer container)
        {
            var revitContext = new RevitContext();
            revitContext.HookupRevitEvents(application);
            container.AddSingleton<IRevitContextProvider>(revitContext);
            container.AddSingleton<IRevitContext>(revitContext);
            return container;
        }

        internal IContainer HookupRevitContext(UIApplication application, IContainer container)
        {
            var revitContext = new RevitContext();
            revitContext.HookupRevitEvents(application);
            container.AddSingleton<IRevitContextProvider>(revitContext);
            container.AddSingleton<IRevitContext>(revitContext);
            return container;
        }

        internal IContainer UnhookRevitContext(UIControlledApplication application, string containerGuid)
        {
            try
            {
                var container = GetContainer(containerGuid);
                var revitContext = container.Resolve<IRevitContextProvider>();
                revitContext.UnhookRevitEvents(application);
                return container;
            }
            catch
            {
                return null;
            }
        }

        internal IContainer UnhookRevitContext(UIApplication application, IContainer container)
        {
            try
            {
                var revitContext = container.Resolve<IRevitContextProvider>();
                revitContext.UnhookRevitEvents(application);
                return container;
            }
            catch
            {
                return null;
            }
        }

        internal IContainer AddRevitUI(IContainer container, UIApplication application)
        {
            var revitUIApp = new RevitAppData
            {
                languageType = (RevitLanguage)application.Application.Language.GetHashCode(),
                versionBuild = application.Application.VersionBuild,
                versionNumber = application.Application.VersionNumber,
                subVersionNumber = application.Application.SubVersionNumber,
                versionName = application.Application.VersionName,
                revitWindowHandle = application.MainWindowHandle
            };

            container.AddSingleton<IRevitAppData>(revitUIApp);
            return container;
        }

        internal IContainer AddRevitUI(IContainer container, UIControlledApplication application)
        {
            var revitUIApp = new RevitAppData
            {
                languageType = (RevitLanguage)application.ControlledApplication.Language.GetHashCode(),
                versionBuild = application.ControlledApplication.VersionBuild,
                versionNumber = application.ControlledApplication.VersionNumber,
                subVersionNumber = application.ControlledApplication.SubVersionNumber,
                versionName = application.ControlledApplication.VersionName,
                revitWindowHandle = application.MainWindowHandle
            };

            container.AddSingleton<IRevitAppData>(revitUIApp);
            return container;
        }

    }
}