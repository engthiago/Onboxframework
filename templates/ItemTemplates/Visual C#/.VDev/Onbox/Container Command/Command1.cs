using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace $rootnamespace$
{
    [Transaction(TransactionMode.Manual)]
    public class $safeitemname$ : RevitContainerCommand<ContainerPipeline>
    {
        public override Result Execute(IContainerResolver container, ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            return Result.Succeeded;
        }
    }
}