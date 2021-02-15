using Autodesk.Revit.DB;
using NUnit.Framework;
using Onbox.Revit.NUnit.Core;

namespace Onbox.Revit.Remote.Tests
{
    [TestFixture]
    public class Walls : RevitTestFixture
    {
        private readonly Document doc;

        public Walls()
        {
            this.doc = app.NewProjectDocument(UnitSystem.Metric);
        }

        [Test]
        public void ShouldBeCreated()
        {
            var levelId = new FilteredElementCollector(this.doc)
                .OfCategory(BuiltInCategory.OST_Levels)
                .WhereElementIsNotElementType()
                .FirstElementId();

            var line = Line.CreateBound(new XYZ(), XYZ.BasisY.Multiply(10));
            Wall wall;

            using (Transaction t = new Transaction(this.doc, "Create Wall"))
            {
                t.Start();
                wall = Wall.Create(this.doc, line, levelId, false);
                t.Commit();
            }

            Assert.NotNull(wall);
        }

        [Test]
        public void ShouldCreateOtherWall()
        {
            var levelId = new FilteredElementCollector(this.doc)
                  .OfCategory(BuiltInCategory.OST_Levels)
                  .WhereElementIsNotElementType()
                  .FirstElementId();

            var line = Line.CreateBound(new XYZ(), XYZ.BasisX.Multiply(10));
            Wall wall;

            using (Transaction t = new Transaction(this.doc, "Create Wall 2"))
            {
                t.Start();
                wall = Wall.Create(this.doc, line, levelId, false);
                t.Commit();
            }

            Assert.NotNull(wall);
        }
    }
}
