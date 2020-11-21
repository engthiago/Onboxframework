using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace Onbox.Revit.VDev
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

        /// <summary>
        /// Constructor
        /// </summary>
        public RevitContext()
        {
        }

        /// <summary>
        /// Gets the current Revit Document
        /// </summary>
        /// <returns></returns>
        public Document GetDocument()
        {
            return this.document;
        }

        /// <summary>
        /// Gets the current Revit UIDocument
        /// </summary>
        /// <returns></returns>
        public UIDocument GetUIDocument()
        {
            return this.uiDocument;
        }

        /// <summary>
        /// Gets the current Revit Application
        /// </summary>
        /// <returns></returns>
        public Application GetApplication()
        {
            return this.application;
        }

        /// <summary>
        /// Gets the current Revit UI Application
        /// </summary>
        /// <returns></returns>
        public UIApplication GetUIApplication()
        {
            return this.uiApplication;
        }

        /// <summary>
        /// Hooks up Revit Events to the context
        /// </summary>
        public void HookupRevitEvents(UIControlledApplication application)
        {
            application.ControlledApplication.DocumentCreated += OnDocumentCreated;
            application.ControlledApplication.DocumentChanged += OnDocumentChanged;
            application.ControlledApplication.DocumentOpened += OnDocumentOpened;
            application.ControlledApplication.DocumentClosed += OnDocumentClosed;
        }

        /// <summary>
        /// Unhooks Revit Events to the context
        /// </summary>
        public void UnhookRevitEvents(UIControlledApplication application)
        {
            application.ControlledApplication.DocumentCreated -= this.OnDocumentCreated;
            application.ControlledApplication.DocumentChanged -= this.OnDocumentChanged;
            application.ControlledApplication.DocumentOpened -= this.OnDocumentOpened;
            application.ControlledApplication.DocumentClosed -= this.OnDocumentClosed;

            // Makes sure to unhook the ViewChanged event and get rid of reference to uiApplication
            UnhookViewChanged();
        }

        /// <summary>
        /// Hooks up Revit Events to the context
        /// </summary>
        public void HookupRevitEvents(UIApplication application)
        {
            if (application?.ActiveUIDocument is UIDocument uIDocument)
            {
                UpdateContext(uIDocument.Document);
            }
            application.Application.DocumentCreated += OnDocumentCreated;
            application.Application.DocumentChanged += OnDocumentChanged;
            application.Application.DocumentOpened += OnDocumentOpened;
            application.Application.DocumentClosed += OnDocumentClosed;
        }

        /// <summary>
        /// Unhooks Revit Events to the context
        /// </summary>
        public void UnhookRevitEvents(UIApplication application)
        {
            application.Application.DocumentCreated -= this.OnDocumentCreated;
            application.Application.DocumentChanged -= this.OnDocumentChanged;
            application.Application.DocumentOpened -= this.OnDocumentOpened;
            application.Application.DocumentClosed -= this.OnDocumentClosed;

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
            if (hookupViewChanged && this.uiApplication != null)
            {
                hookupViewChanged = false;
                this.uiApplication.ViewActivated -= this.OnViewChanged;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Identifies if Revit is in the current context (Revit API context)
        /// </summary>
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