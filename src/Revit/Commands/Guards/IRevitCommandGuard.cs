using Autodesk.Revit.UI;
using Onbox.Abstractions.VDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Revit.VDev.Commands.Guards
{
    public interface IRevitCommandGuard
    {
        bool CanExecute(Type commandType, IContainerResolver container, ExternalCommandData commandData);
    }
}
