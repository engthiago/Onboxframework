using Autodesk.Revit.ApplicationServices;
using NUnit.Framework;
using Onbox.Revit.NUnit.Core.Internal;

namespace Onbox.Revit.NUnit.Core
{
    [TestFixture]
    [SingleThreaded]
    public abstract class RevitTestFixture
    {
        protected readonly Application app;
        protected readonly string workingDirectory;
        protected readonly string workItemId;
        protected readonly string addinPath;

        public RevitTestFixture()
        {
            this.app = RemoteContainer.GetRevitApplication();
            this.workingDirectory = RemoteContainer.GetWorkDirectory();
            this.workItemId = RemoteContainer.GetWorkItemId();
            this.addinPath = RemoteContainer.GetAddinDirectory();
        }
    }
}
