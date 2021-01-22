using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ExtensibleStorageSample.ExtensibleStorage;
using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands;
using Onbox.Revit.VDev.ExtensibleStorage;

namespace ExtensibleStorageSample.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class LoadFromStorageCommand : RevitAppCommand<App>
    {
        public override Result Execute(IContainerResolver container, ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var doc = commandData.Application.ActiveUIDocument.Document;
            var uidoc = commandData.Application.ActiveUIDocument;

            // Select elements in Revit UI
            var elemRef = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, "Please select and element");
            var element = doc.GetElement(elemRef);

            // Request store from the container
            var store = container.Resolve<IRevitJsonStorage<SampleData>>();

            var data = store.Load(element);
            TaskDialog.Show("This is your data:", $"Id {data.Quantity}: {data.SomeRandomString}");

            return Result.Succeeded;
        }
    }
}