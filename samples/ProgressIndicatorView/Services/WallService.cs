using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProgressIndicatorView.Services
{
    public class WallService
    {
        public Wall CreateWall(Document doc, XYZ start, XYZ end)
        {
            var line = Line.CreateBound(start, end);

            var level = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Levels)
                .WhereElementIsNotElementType()
                .Cast<Level>()
                .OrderBy(w => w.Elevation)
                .First();

            var wall = Wall.Create(doc, line, level.Id, false);
            return wall;
        }

        public ICollection<ElementId> DeleteAllWalls(Document doc)
        {
            var wallIds = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Walls)
                .WhereElementIsNotElementType()
                .ToElementIds();

            if (wallIds.Any())
            {
                return doc.Delete(wallIds);
            }

            return new ElementId[0];
        }
    }
}
