using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using NUnit.Framework;
using Onbox.Revit.NUnit;
using Onbox.Revit.NUnit.Core;

namespace Onbox.Revit.Remote.Tests
{
    [TestFixture]
    public class FrameworkContainer : RevitTestFixture
    {
        public FrameworkContainer()
        {
        }

        [Test]
        public void ShouldResolveRevitApplication()
        {
            Assert.NotNull(app);
        }

    }
}
