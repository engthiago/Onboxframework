using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ExtensibleStorageSample.Revit.Commands.Availability
{
    /// <summary>
    /// The Command will only be available on Project environment
    /// </summary>
    public class AvailableOnProject : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        {
            if (applicationData.ActiveUIDocument == null) return false;
            if (applicationData.ActiveUIDocument.Document == null) return false;
            if (applicationData.ActiveUIDocument.Document.IsFamilyDocument) return false;

            return true;
        }
    }
}