using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Core.V1.Logging;
using Onbox.Core.V1.Messaging;
using Onbox.Core.V1.Reporting;
using Onbox.Mvc.V1;
using Onbox.Mvc.V1.Messaging;
using System.Threading.Tasks;

namespace Onbox.Sandbox.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Test : ExternalCommandBase
    {
        public override Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var test = this.container.Resolve<ITestWindow>();
            test.ShowDialog();

            //var logging = this.container.Resolve<ILoggingService>();
            //logging.Log("Test log now...");

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class ViewsCommand : ExternalCommandBase
    {
        public override Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;


            IProgressIndicator progressIndicator = container.Resolve<IProgressIndicator>();

            progressIndicator.Run(50, true, () =>
            {
                for (int i = 0; i < 50; i++)
                {
                    using (Transaction t = new Transaction(doc, "Long operation"))
                    {
                        t.Start();

                        for (int k = 0; k < 1; k++)
                        {
                            var skp = SketchPlane.Create(doc, Plane.CreateByNormalAndOrigin(XYZ.BasisZ, XYZ.Zero));
                            doc.Create.NewModelCurve(Line.CreateBound(XYZ.Zero, new XYZ(0, 10, 0)), skp);
                        }

                        t.RollBack();
                    }
                    progressIndicator.Iterate("Step [1]: " + (i + 1).ToString());
                }

                progressIndicator.Reset(100);

                for (int i = 0; i < 100; i++)
                {
                    using (Transaction t = new Transaction(doc, "Long operation"))
                    {
                        t.Start();

                        for (int k = 0; k < 1; k++)
                        {
                            var skp = SketchPlane.Create(doc, Plane.CreateByNormalAndOrigin(XYZ.BasisZ, XYZ.Zero));
                            doc.Create.NewModelCurve(Line.CreateBound(XYZ.Zero, new XYZ(0, 10, 0)), skp);
                        }

                        t.RollBack();
                    }
                    progressIndicator.Iterate("Step [2]: " + (i + 1).ToString());
                }

            });

            return Result.Succeeded;
        }
    }


    [Transaction(TransactionMode.Manual)]
    public class CleanContainer : ExternalCommandBase
    {
        public override Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            this.container.Clear();

            return Result.Succeeded;
        }
    }
}
