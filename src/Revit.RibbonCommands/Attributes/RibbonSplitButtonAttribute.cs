using System;

namespace Onbox.Revit.VDev.RibbonCommands.Attributes
{
    /// <summary>
    /// A Split Button on Revit's Ribbon.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class RibbonSplitButtonAttribute : Attribute, IRibbonCommandAttribute
    {
        /// <summary>
        /// The class to decide availability of this button.
        /// </summary>
        public Type Availability { get; set; }
        /// <summary>
        /// First line of the name of this button.
        /// </summary>
        public string FirstLine { get; set; }
        /// <summary>
        /// Second line of the name of this button.
        /// </summary>
        public string SecondLine { get; set; }
        /// <summary>
        /// The panel name of this button.
        /// </summary>
        public string PanelName { get; set; }
        /// <summary>
        /// The tooltip of this button.
        /// </summary>
        public string Tooltip { get; set; }
        /// <summary>
        /// The description of this button.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The image of this button. 
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// The priority of this button on the panel. Higher numbers will be added to the panel first. 
        /// </summary>
        public int PanelPriority { get; set; }
        /// <summary>
        /// The name of the split group. Buttons with the same name will be grouped together.
        /// </summary>
        public string SplitButtonGroup { get; }
        /// <summary>
        /// The priority of this button on the split stack. Higher numbers will be added first. 
        /// </summary>
        public int SplitGroupPriority { get; set; }

        public RibbonSplitButtonAttribute(string splitButtonGroup)
        {
            SplitButtonGroup = splitButtonGroup;
        }
    }
}
