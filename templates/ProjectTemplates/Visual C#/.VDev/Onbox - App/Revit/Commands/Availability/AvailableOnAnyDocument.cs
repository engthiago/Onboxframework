using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace $safeprojectname$.Revit.Commands.Availability
{
    /// <summary>
    /// The Command will be available on Project and Family environment
    /// </summary>
    public class AvailableOnAnyDocument : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        {
            if (applicationData.ActiveUIDocument == null) return false;
            if (applicationData.ActiveUIDocument.Document == null) return false;

            return false;
        }
    }
}