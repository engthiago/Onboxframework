using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using NUnit.Framework;
using Onbox.Revit.NUnit;

namespace Onbox.Revit.Remote.Tests
{
    [TestFixture]
    public class TestFramework
    {
        private readonly Application app;
        private readonly Document doc;
        private readonly string path;

        public TestFramework()
        {
            this.app = RevitRemoteContainer.GetRevitApplication();
            this.doc = RevitRemoteContainer.GetRevitDocument();
            this.path = RevitRemoteContainer.GetFilePath();
        }

        [Test]
        public void ShouldResolveRevitApplication()
        {
            Assert.NotNull(app);
        }

        [Test]
        public void ShouldResolveRevitDocument()
        {
            Assert.NotNull(doc);
        }

        [Test]
        public void ShouldResolveRevitPath()
        {
            Assert.IsNotEmpty(path);
        }
    }
}
