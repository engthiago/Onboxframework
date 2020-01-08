using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
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
            var someService = this.container.Resolve<SomeService>();
            var someService2 = this.container.Resolve<SomeService>();

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

            progressIndicator.Run(100, true, () =>
            {
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

                    if (i == 30)
                    {
                        throw new ProgressCancelledException("Ooops!");
                    }

                    progressIndicator.Iterate("Current: " + (i + 1).ToString());
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
            this.container.Reset();

            return Result.Succeeded;
        }
    }
}
