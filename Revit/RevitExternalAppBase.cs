using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Di.V7;
using Onbox.Revit.V7.UI;
using System;

namespace Onbox.Revit.V7
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class ContainerOriginAttribute : Attribute
    {
        public ContainerOriginAttribute()
        {
        }
    }

    public abstract class RevitExternalAppBase : RevitExternalAppBase<Container>
    {
    }

    [ContainerOrigin]
    public abstract class RevitExternalAppBase<TContainer> : IExternalApplication 
        where TContainer : class, IContainer, new()
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
                        {
                            container = new TContainer();
                        }
                            
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

        public abstract void OnShutdown(IContainerResolver container, UIControlledApplication application);

        public Result OnStartup(UIControlledApplication application)
        {
            HookupRevitEvents(application);

            AddRevit(application);

            ContainerInstance.AddSingleton(ContainerInstance);
            ContainerInstance.AddSingleton<IContainerResolver>(ContainerInstance);

            OnStartup(ContainerInstance, application);

            var imageManager = new ImageManager();
            var ribbonManager = new RibbonManager(application, imageManager);
            OnCreateRibbon(ribbonManager);

            return Result.Succeeded;
        }

        private void HookupRevitEvents(UIControlledApplication application)
        {
            application.ControlledApplication.DocumentChanged += this.OnDocumentChanged;
            application.ControlledApplication.DocumentOpened += this.OnDocumentOpened;
            application.ControlledApplication.DocumentClosed += this.OnDocumentClosed;
            application.ControlledApplication.DocumentCreated += this.OnDocumentCreated;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            UnhookRevitEvents(application);

            try
            {
                OnShutdown(ContainerInstance, application);
            }
            finally
            {
                ContainerInstance.Dispose();
            }

            return Result.Succeeded;
        }

        private void UnhookRevitEvents(UIControlledApplication application)
        {
            application.ControlledApplication.DocumentChanged -= this.OnDocumentChanged;
            application.ControlledApplication.DocumentOpened -= this.OnDocumentOpened;
            application.ControlledApplication.DocumentClosed -= this.OnDocumentClosed;
            application.ControlledApplication.DocumentCreated -= this.OnDocumentCreated;
        }

        public virtual void OnCreateRibbon(IRibbonManager ribbonManager)
        {
        }

        private void OnDocumentCreated(object sender, Autodesk.Revit.DB.Events.DocumentCreatedEventArgs e)
        {
            ContainerInstance.AddSingleton(e.Document);
        }

        private void OnDocumentClosed(object sender, Autodesk.Revit.DB.Events.DocumentClosedEventArgs e)
        {
            ContainerInstance.AddSingleton<Document>(null);
        }

        private void OnDocumentOpened(object sender, Autodesk.Revit.DB.Events.DocumentOpenedEventArgs e)
        {
            ContainerInstance.AddSingleton(e.Document);
        }

        private void OnDocumentChanged(object sender, Autodesk.Revit.DB.Events.DocumentChangedEventArgs e)
        {
            ContainerInstance.AddSingleton(e.GetDocument());
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
            ContainerInstance.AddSingleton<Document>(null);
        }

    }
}
