using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Revit.VDev.Commands
{
    /// <summary>
    /// A Revit External Command
    /// </summary>
    public interface IRevitCommand
    {
        /// <summary>
        /// Overload this method to implement and external command within Revit.
        /// </summary>
        Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements);
    }
}
