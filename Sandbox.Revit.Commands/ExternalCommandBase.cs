using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Di.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Sandbox.Revit.Commands
{
    public abstract class ExternalCommandBase : IExternalCommand
    {
        protected Container container;

        public ExternalCommandBase()
        {
            container = ContainerBuilder.Build();
        }

        public abstract Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements);
    }
}
