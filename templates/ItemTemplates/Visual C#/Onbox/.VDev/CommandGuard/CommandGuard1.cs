using Onbox.Revit.VDev.Commands;
using Onbox.Revit.VDev.Commands.Guards;

namespace CommandGuardSamples.Commands.Guards
{
    public class $safeitemname$ : IRevitCommandGuard
    {
        public bool CanExecute(ICommandInfo commandInfo)
        {
			// Return true to allow Command execution
            return true;
        }
    }
}
