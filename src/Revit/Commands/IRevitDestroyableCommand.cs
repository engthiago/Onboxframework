using Onbox.Abstractions.V7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Revit.V7.Commands
{
    /// <summary>
    /// Holds lifecycle hook for Commands to be able to hook up to disposing container
    /// </summary>
    public interface IRevitDestroyableCommand
    {
        /// <summary>
        /// External Command lifecycle hook which is called just before the container is disposed.
        /// </summary>
        void OnDestroy(IContainerResolver container);
    }
}
