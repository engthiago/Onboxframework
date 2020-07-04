using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Di.V7;
using System.Collections.Concurrent;

namespace Onbox.Revit.V7
{
    internal class RevitEventTracker
    {
        private readonly ConcurrentDictionary<string, IContainer> containers;

        public RevitEventTracker(ConcurrentDictionary<string, IContainer>  containers)
        {
            this.containers = containers;
        }

        internal void HookupRevitEvents(UIControlledApplication application)
        {
            application.ControlledApplication.DocumentChanged += OnDocumentChanged;
            application.ControlledApplication.DocumentOpened += OnDocumentOpened;
            application.ControlledApplication.DocumentClosed += OnDocumentClosed;
            application.ControlledApplication.DocumentCreated += OnDocumentCreated;
        }

        internal void UnhookRevitEvents(UIControlledApplication application)
        {
            application.ControlledApplication.DocumentChanged -= this.OnDocumentChanged;
            application.ControlledApplication.DocumentOpened -= this.OnDocumentOpened;
            application.ControlledApplication.DocumentClosed -= this.OnDocumentClosed;
            application.ControlledApplication.DocumentCreated -= this.OnDocumentCreated;
        }

        private void OnDocumentChanged(object sender, Autodesk.Revit.DB.Events.DocumentChangedEventArgs e)
        {
            var doc = e.GetDocument();
            foreach (var item in containers)
            {
                item.Value.AddSingleton(doc);
            }
        }

        private void OnDocumentCreated(object sender, Autodesk.Revit.DB.Events.DocumentCreatedEventArgs e)
        {
            foreach (var item in containers)
            {
                item.Value.AddSingleton(e.Document);
            }
        }

        private void OnDocumentClosed(object sender, Autodesk.Revit.DB.Events.DocumentClosedEventArgs e)
        {
            foreach (var item in containers)
            {
                item.Value.AddSingleton<Document>(null);
            }
        }

        private void OnDocumentOpened(object sender, Autodesk.Revit.DB.Events.DocumentOpenedEventArgs e)
        {
            foreach (var item in containers)
            {
                item.Value.AddSingleton(e.Document);
            }
        }
    }
}
