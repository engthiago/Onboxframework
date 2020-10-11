using Onbox.Abstractions.V7;
using Onbox.Revit.V7.Commands;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace $rootnamespace$
{
    [Transaction(TransactionMode.Manual)]
    public class $safeitemname$ : RevitAppCommand<App>
    {
        public override Result Execute(IContainerResolver container, ExternalCommandData commandData, ref string message, ElementSet elements)
        {
			
            return Result.Succeeded;
        }
    }
}
