using Autodesk.Revit.UI;
using Onbox.Revit.VDev.RibbonCommands.Attributes;

namespace Onbox.Revit.VDev.RibbonCommands
{
    internal class RibbonCommandData
    {
        internal RibbonPanel ribbonPanel;
        internal PushButtonData button;
        internal IRibbonCommandAttribute commandAttribute;
    }
}
