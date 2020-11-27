using Autodesk.Revit.UI;
using Onbox.Abstractions.VDev;
using System;

namespace Onbox.Revit.VDev.Commands.Guards
{
    public interface IRevitCommandGuardChecker
    {
        bool CanExecute(ICommandInfo commandInfo);
    }
}
