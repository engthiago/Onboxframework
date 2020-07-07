using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Di.V7;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Onbox.Revit.V7
{
    /// <summary>
    /// The most base class for a container provider, it will manage containers and events during its lifetime
    /// </summary>
    public abstract class RevitContainerBase
    {
        internal static ConcurrentDictionary<string, IContainer> containers = new ConcurrentDictionary<string, IContainer>();
        internal static Dictionary<string, RevitEventTracker> events = new Dictionary<string, RevitEventTracker>();

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

        internal static RevitEventTracker GetEvent(string guid)
        {
            if (string.IsNullOrWhiteSpace(guid))
            {
                throw new Exception($"Application should have a valid GUID to locate the event tracker");
            }

            if (events.TryGetValue(guid, out RevitEventTracker eventTracker))
            {
                return eventTracker;
            }

            throw new Exception($"No Event Tracker found for this Application with specified GUID: {guid}");
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

        internal RevitEventTracker HookupEventTracker(UIControlledApplication application, IContainer container, string containerGuid)
        {
            var eventTracker = new RevitEventTracker(container);
            eventTracker.HookupRevitEvents(application);
            events.Add(containerGuid, eventTracker);

            return eventTracker;
        }

        internal RevitEventTracker UnhookEventTracker(UIControlledApplication application, string containerGuid)
        {
            if (events.TryGetValue(containerGuid, out RevitEventTracker eventTracker))
            {
                eventTracker.UnhookRevitEvents(application);
                events.Remove(containerGuid);
                return eventTracker;
            }

            return null;
        }

        internal IContainer AddRevitUI(IContainer container, UIControlledApplication application)
        {
            var revitUIApp = new RevitAppData
            {
                isViewerMode = application.IsViewerModeActive,
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
