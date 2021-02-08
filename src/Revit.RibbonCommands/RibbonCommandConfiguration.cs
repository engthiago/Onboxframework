using System.Collections.Generic;

namespace Onbox.Revit.RibbonCommands.VDev
{
    /// <summary>
    /// Configuration options for automatically adding Ribbon Panels and Buttons.
    /// </summary>
    public class RibbonCommandConfiguration
    {
        internal List<string> panels;

        /// <summary>
        /// The name of the tab on which the panels and buttons will be created.
        /// </summary>
        public string TabName { get; set; }

        /// <summary>
        /// The default name for the panel, in case of not specified on the attribute.
        /// </summary>
        public string DefaultPanelName { get; set; }

        public RibbonCommandConfiguration()
        {
            panels = new List<string>();
        }

        /// <summary>
        /// Adding panels here will allow you to control the order on which the panels will be created for Ribbon Commands.
        /// </summary>
        /// <param name="panelName"></param>
        public void AddPanel(string panelName)
        {
            this.panels.Add(panelName);
        }
    }
}
