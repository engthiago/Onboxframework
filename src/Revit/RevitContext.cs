using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace Onbox.Revit.V7
{
    /// <summary>
    /// This class will keep track of Revit UI events to always have the current <see cref="Document"/>, <see cref="Application"/>, <see cref="UIDocument"/>, and <see cref="UIApplication"/>
    /// </summary>
    internal class RevitContext : IRevitContext
    {
        private Document document;
        private Application application;
        private UIDocument uiDocument;
        private UIApplication uiApplication;

        private bool hookupViewChanged;

        public RevitContext()
        {
        }

        public Document GetDocument()
        {
            return this.document;
        }

        public UIDocument GetUIDocument()
        {
            return this.uiDocument;
        }

        public Application GetApplication()
        {
            return this.application;
        }

        public UIApplication GetUIApplication()
        {
            return this.uiApplication;
        }

        public void HookupRevitEvents(UIControlledApplication application)
        {
            application.ControlledApplication.DocumentCreated += OnDocumentCreated;
            application.ControlledApplication.DocumentChanged += OnDocumentChanged;
            application.ControlledApplication.DocumentOpened += OnDocumentOpened;
            application.ControlledApplication.DocumentClosed += OnDocumentClosed;
        }

        public void UnhookRevitEvents(UIControlledApplication application)
        {
            application.ControlledApplication.DocumentCreated -= this.OnDocumentCreated;
            application.ControlledApplication.DocumentChanged -= this.OnDocumentChanged;
            application.ControlledApplication.DocumentOpened -= this.OnDocumentOpened;
            application.ControlledApplication.DocumentClosed -= this.OnDocumentClosed;

            // Makes sure to unhook the ViewChanged event and get rid of reference to uiApplication
            UnhookViewChanged();
        }
        private void OnDocumentCreated(object sender, Autodesk.Revit.DB.Events.DocumentCreatedEventArgs e)
        {
            var doc = e.Document;
            this.UpdateContext(doc);
            HookUpViewChanged(doc);
        }

        private void OnDocumentChanged(object sender, Autodesk.Revit.DB.Events.DocumentChangedEventArgs e)
        {
            var doc = e.GetDocument();
            this.UpdateContext(doc);
            this.HookUpViewChanged(doc);
        }

        private void OnDocumentOpened(object sender, Autodesk.Revit.DB.Events.DocumentOpenedEventArgs e)
        {
            var doc = e.Document;
            this.UpdateContext(doc);
            this.HookUpViewChanged(doc);
        }

        private void OnViewChanged(object sender, Autodesk.Revit.UI.Events.ViewActivatedEventArgs e)
        {
            var doc = e.Document;
            this.UpdateContext(doc);
        }

        private void OnDocumentClosed(object sender, Autodesk.Revit.DB.Events.DocumentClosedEventArgs e)
        {
            // If the ActiveUIdocument is not null, it means that there is another document in the background
            // The DocumentChanged or ViewChanged event will pickup that new Document
            // So in the case of null ActiveUIDocument we cleanup the container from Documents and Applications
            if (this.uiApplication?.ActiveUIDocument == null)
            {
                UpdateContext(null);
            }
        }

        private void UpdateContext(Document doc)
        {
            this.document = doc;

            // In the case of a valid Document context
            if (doc != null)
            {
                var uidoc = new UIDocument(doc);

                this.application = doc.Application;
                this.uiDocument = uidoc;
                this.uiApplication = uidoc.Application;
            }
            else
            {
                this.application = null;
                this.uiDocument = null;
                this.uiApplication = null;
            }

        }

        private bool HookUpViewChanged(Document doc)
        {
            if (!this.hookupViewChanged)
            {
                this.hookupViewChanged = true;
                var uidoc = new UIDocument(doc);
                uidoc.Application.ViewActivated += this.OnViewChanged;
                return true;
            }

            return false;
        }

        private bool UnhookViewChanged()
        {
            if (hookupViewChanged)
            {
                hookupViewChanged = false;
                this.uiApplication.ViewActivated -= this.OnViewChanged;
                return true;
            }

            return false;
        }

        public bool IsInRevitContext()
        {
            if (this.document == null)
            {
                return false;
            }

            if (this.document.IsModifiable)
            {
                return true;
            }

            using (var t = new Transaction(document, "Context"))
            {
                try
                {
                    t.Start();
                    t.RollBack();
                    return true;
                }
                catch (Autodesk.Revit.Exceptions.InvalidOperationException)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
