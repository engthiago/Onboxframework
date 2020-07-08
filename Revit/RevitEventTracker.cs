using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Abstractions.V7;

namespace Onbox.Revit.V7
{
    /// <summary>
    /// This class will keep track of Revit UI events to always have the current <see cref="Document"/>, <see cref="Application"/>, <see cref="UIDocument"/>, and <see cref="UIApplication"/>
    /// </summary>
    internal class RevitEventTracker
    {
        private readonly IContainer container;
        private UIApplication uiApplication;

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

            // Makes sure that the Documents and Applications exists even when no document is opened for the first time
            this.container.AddSingleton<Document>(null);
            this.container.AddSingleton<Application>(null);
            this.container.AddSingleton<UIDocument>(null);
            this.container.AddSingleton<UIApplication>(null);
        }

        internal void UnhookRevitEvents(UIControlledApplication application)
        {
            application.ControlledApplication.DocumentChanged -= this.OnDocumentChanged;
            application.ControlledApplication.DocumentOpened -= this.OnDocumentOpened;
            application.ControlledApplication.DocumentClosed -= this.OnDocumentClosed;
            application.ControlledApplication.DocumentCreated -= this.OnDocumentCreated;

            // Makes sure to unhook the ViewChanged event and get rid of reference to uiApplication
            UnhookViewChanged();
        }

        private void OnDocumentChanged(object sender, Autodesk.Revit.DB.Events.DocumentChangedEventArgs e)
        {
            var doc = e.GetDocument();
            HookUpViewChanged(doc);
        }

        private void OnViewChanged(object sender, Autodesk.Revit.UI.Events.ViewActivatedEventArgs e)
        {
            var doc = e.Document;
            this.AddToContainer(doc);
        }

        private void OnDocumentCreated(object sender, Autodesk.Revit.DB.Events.DocumentCreatedEventArgs e)
        {
            var doc = e.Document;
            HookUpViewChanged(doc);
        }

        private void OnDocumentClosed(object sender, Autodesk.Revit.DB.Events.DocumentClosedEventArgs e)
        {
            // If the ActiveUIdocument is not null, it means that there is another document in the background
            // The DocumentChanged or ViewChanged event will pickup that new Document
            // So in the case of null ActiveUIDocument we cleanup the container from Documents and Applications
            if (this.uiApplication?.ActiveUIDocument == null)
            {
                AddToContainer(null);
            }
        }

        private void AddToContainer(Document doc)
        {
            this.container.AddSingleton(doc);

            // In the case of a valid Document context
            if (doc != null)
            {
                var uidoc = new UIDocument(doc);
                this.container.AddSingleton<Application>(doc.Application);
                this.container.AddSingleton<UIDocument>(uidoc);
                this.container.AddSingleton<UIApplication>(uidoc.Application);
            }
            else
            {
                this.container.AddSingleton<Application>(null);
                this.container.AddSingleton<UIDocument>(null);
                this.container.AddSingleton<UIApplication>(null);
            }

        }

        private void OnDocumentOpened(object sender, Autodesk.Revit.DB.Events.DocumentOpenedEventArgs e)
        {
            var doc = e.Document;
            HookUpViewChanged(doc);
        }

        private bool HookUpViewChanged(Document doc)
        {
            if (this.uiApplication == null)
            {
                var uidoc = new UIDocument(doc);
                this.uiApplication = uidoc.Application;
                this.uiApplication.ViewActivated += this.OnViewChanged;
                return true;
            }

            return false;
        }

        private bool UnhookViewChanged()
        {
            if (this.uiApplication != null)
            {
                this.uiApplication.ViewActivated -= this.OnViewChanged;
                this.uiApplication = null;
                return true;
            }

            return false;
        }
    }
}
