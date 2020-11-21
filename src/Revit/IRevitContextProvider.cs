using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Revit.VDev
{
    public interface IRevitContextProvider
    {
        /// <summary>
        /// Hooks up Revit Events to the context
        /// </summary>
        void HookupRevitEvents(UIControlledApplication application);
        /// <summary>
        /// Unhooks Revit Events to the context
        /// </summary>
        void UnhookRevitEvents(UIControlledApplication application);
        /// <summary>
        /// Hooks up Revit Events to the context
        /// </summary>
        void HookupRevitEvents(UIApplication application);
        /// <summary>
        /// Unhooks Revit Events to the context
        /// </summary>
        void UnhookRevitEvents(UIApplication application);
    }
}
