using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Di.V7;
using System;

namespace Onbox.Revit.V7
{
    public abstract class RevitExternalAppBase : IExternalApplication
    {
        private static volatile IContainer container;
        private static object syncRoot = new Object();

        private static IContainer ContainerInstance
        {
            get
            {
                if (container == null)
                {
                    lock (syncRoot)
                    {
                        if (container == null)
                            container = Container.Default();
                    }
                }

                return container;
            }
        }

        public static IContainer GetContainer()
        {
            return ContainerInstance;
        }

        public abstract void OnStartup(IContainer container, UIControlledApplication application);

        public abstract void OnShutdown(IContainer container, UIControlledApplication application);

        public Result OnShutdown(UIControlledApplication application)
        {
            OnShutdown(container, application);
            container.Dispose();

            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            AddRevit(application);
            OnStartup(container, application);

            return Result.Succeeded;
        }

        private void AddRevit(UIControlledApplication application)
        {
            var revitUIApp = new RevitUIApp
            {
                isViewerMode = application.IsViewerModeActive,
                languageType = (RevitLanguage)application.ControlledApplication.Language.GetHashCode(),
                versionBuild = application.ControlledApplication.VersionBuild,
                versionNumber = application.ControlledApplication.VersionNumber,
                subVersionNumber = application.ControlledApplication.SubVersionNumber,
                versionName = application.ControlledApplication.VersionName,
                revitWindowHandle = application.MainWindowHandle
            };

            ContainerInstance.AddSingleton<IRevitUIApp>(revitUIApp);
        }

    }
}
