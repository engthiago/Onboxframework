using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands;
using ProgressIndicatorView.Services;

namespace ProgressIndicatorView.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class ProgressIndicatorViewCommand : RevitAppCommand<App>
    {
        public override Result Execute(IContainerResolver container, ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var doc = commandData.Application.ActiveUIDocument.Document;
            var uidoc = commandData.Application.ActiveUIDocument;

            var progress = container.Resolve<IProgressIndicator>();
            var wallService = container.Resolve<WallService>();

            using (Transaction t = new Transaction(doc, "Create several walls"))
            {
                t.Start();
                
                var yPosition = 0.0;
                var yOffset = 3;
                var numberOfWallsToCreate = 200;
                var canCancelProgress = true; // Allows the user to cancel the progress

                // Delete All Walls From the project first
                wallService.DeleteAllWalls(doc);

                // Run the Progress indicator executing the action
                progress.Run(numberOfWallsToCreate, canCancelProgress, () =>
                {
                    // Long Run Operation
                    var start = new XYZ( 0, yPosition, 0);
                    var end =   new XYZ(20, yPosition, 0);
                    var wall = wallService.CreateWall(doc, start, end);

                    // Tell the progress to iterate
                    progress.Iterate($"Creating wall: at {yPosition}feet");

                    // Optional: Refresh Revit UI in the background
                    uidoc.RefreshActiveView();
                    doc.Regenerate();

                    // Prepare for the next iteration
                    yPosition += yOffset;
                });

                // If the indicator is finished successfully commit the transaction, otherwise roll it back
                if (progress.FinishedSuccessfully())
                {
                    t.Commit();
                }
                else
                {
                    t.RollBack();
                }
            }

            return Result.Succeeded;
        }
    }
}