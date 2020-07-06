using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Di.V7;

namespace Onbox.Revit.V7
{
    internal class RevitEventTracker
    {
        private readonly IContainer container;

        public RevitEventTracker(IContainer container)
        {
            this.container = container;
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
            this.container.AddSingleton(doc);
        }

        private void OnDocumentCreated(object sender, Autodesk.Revit.DB.Events.DocumentCreatedEventArgs e)
        {
            var doc = e.Document;
            this.container.AddSingleton(doc);
        }

        private void OnDocumentClosed(object sender, Autodesk.Revit.DB.Events.DocumentClosedEventArgs e)
        {
            this.container.AddSingleton<Document>(null);
        }

        private void OnDocumentOpened(object sender, Autodesk.Revit.DB.Events.DocumentOpenedEventArgs e)
        {
            var doc = e.Document;
            this.container.AddSingleton(doc);
        }
    }
}
