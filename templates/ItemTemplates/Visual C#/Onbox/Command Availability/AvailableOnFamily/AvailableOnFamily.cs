using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace $rootnamespace$
{
    /// <summary>
    /// The Command will only be available on Family environment
    /// </summary>
    public class $safeitemname$ : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        {
            if (applicationData.ActiveUIDocument == null) return false;
            if (applicationData.ActiveUIDocument.Document == null) return false;
            if (applicationData.ActiveUIDocument.Document.IsFamilyDocument) return true;

            return false;
        }
    }
}