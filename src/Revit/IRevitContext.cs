using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Abstractions.V7;

namespace Onbox.Revit.V7
{
    public interface IRevitContext
    {
        Application GetApplication();
        Document GetDocument();
        UIApplication GetUIApplication();
        UIDocument GetUIDocument();
        void HookupRevitEvents(UIControlledApplication application);
        void UnhookRevitEvents(UIControlledApplication application);
        bool IsInRevitContext();
    }
}