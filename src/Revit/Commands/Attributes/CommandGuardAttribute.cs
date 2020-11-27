using Onbox.Revit.VDev.Commands.Guards;
using System;
using System.Linq;

namespace Onbox.Revit.VDev.Commands.Attributes
{
    /// <summary>
    /// Adds a CommandGuard to to a Revit Command.
    /// <br>The type MUST implement <see cref="IRevitCommandGuard"/> interface!</br>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class CommandGuardAttribute : Attribute
    {
        private readonly Type revitCommandGuardType;

        /// <summary>
        ///  Adds a CommandGuard to to a Revit Command.
        /// </summary>
        /// <param name="revitCommandGuardType">The type mus implement <see cref="IRevitCommandGuard"/> interface</param>
        public CommandGuardAttribute(Type revitCommandGuardType)
        {
            this.revitCommandGuardType = revitCommandGuardType;
        }

        /// <summary>
        /// Gets the CommandGuard Type
        /// </summary>
        /// <returns>The type of the Command Guard</returns>
        public Type GetCommandGuardType()
        {
            return this.revitCommandGuardType;
        }
    }
}
