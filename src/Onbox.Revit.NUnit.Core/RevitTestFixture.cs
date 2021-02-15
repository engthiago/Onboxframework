using Autodesk.Revit.ApplicationServices;
using NUnit.Framework;
using Onbox.Revit.Remote.DAInternal;

namespace Onbox.Revit.NUnit.Core
{
    /// <summary>
    /// Base class to help running Revit related tests. It has ready to use Revit Context variables like Application and directories.<br/>
    /// It also helps to ensure the fixture will run on single thread.
    /// </summary>
    [TestFixture]
    [SingleThreaded]
    public abstract class RevitTestFixture
    {
        /// <summary>
        /// The current Revit Application.
        /// </summary>
        protected readonly Application app;
        /// <summary>
        /// The current work directory for Design Automation for Revit.
        /// </summary>
        protected readonly string workingDirectory;
        /// <summary>
        /// The current work id for Design Automation for Revit.
        /// </summary>
        protected readonly string workItemId;
        /// <summary>
        /// The current addin path on Design Automation for Revit.
        /// </summary>
        protected readonly string addinPath;

        /// <summary>
        /// Creates a new instance of Revit Fixture.
        /// </summary>
        public RevitTestFixture()
        {
            this.app = RemoteContainer.GetRevitApplication();
            this.workingDirectory = RemoteContainer.GetWorkDirectory();
            this.workItemId = RemoteContainer.GetWorkItemId();
            this.addinPath = RemoteContainer.GetAddinDirectory();
        }
    }
}
