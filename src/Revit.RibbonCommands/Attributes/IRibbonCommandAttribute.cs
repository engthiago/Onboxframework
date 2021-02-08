using System;

namespace Onbox.Revit.RibbonCommands.VDev.Attributes
{
    public interface IRibbonCommandAttribute
    {
        Type Availability { get; set; }
        string FirstLine { get; set; }
        string SecondLine { get; set; }
        string PanelName { get; set; }
        int PanelPriority { get; set; }
        string Tooltip { get; set; }
        string Description { get; set; }
        string Image { get; set; }
    }
}
