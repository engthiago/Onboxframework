using System;
using System.Collections.Generic;

namespace Onbox.Revit.VDev.Commands.Guards
{
    internal interface IConditionBuilderProvider
    {
        Predicate<ICommandInfo> GetPredicate();
        List<Type> GetCommandTypes();
    }
}
