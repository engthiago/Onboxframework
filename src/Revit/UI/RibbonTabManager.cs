using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace Onbox.Revit.VDev.UI
{
    /// <summary>
    /// Ribbon Manager will help you create Tabs, Panels and Buttons in Revit
    /// </summary>
    public interface IRibbonManager
    {
        /// <summary>
        /// Get Revit UI Controlled App
        /// </summary>
        /// <returns></returns>
        UIControlledApplication GetApp();
        /// <summary>
        /// Creates a Ribbon Panel
        /// </summary>
        IRibbonPanelManager CreatePanel(string tabName, string panelName);
        /// <summary>
        /// Creates a Ribbon Panel on Revit's Addins Tab
        /// </summary>
        IRibbonPanelManager CreatePanel(string panelName);
        /// <summary>
        /// Creates a Ribbon Tab
        /// </summary>
        IRibbonTabManager CreateTab(string name);
        /// <summary>
        /// Gets Ribbon line break string
        /// </summary>
        string GetLineBreak();
    }

    /// <summary>
    /// Ribbon Manager will help you create Tabs, Panels and Buttons in Revit
    /// </summary>
    public class RibbonManager : IRibbonManager
    {
        private readonly UIControlledApplication app;
        private readonly ImageManager imageManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public RibbonManager(UIControlledApplication app, ImageManager imageManager)
        {
            this.app = app;
            this.imageManager = imageManager;
        }

        /// <summary>
        /// Creates a Ribbon Tab
        /// </summary>
        public IRibbonTabManager CreateTab(string name)
        {
            try
            {
                this.app.CreateRibbonTab(name);
            }
            catch
            {
            }

            return new RibbonTabManager(this.app, name, this.imageManager);
        }

        /// <summary>
        /// Creates a Ribbon Panel on Revit's Addins Tab
        /// </summary>
        public IRibbonPanelManager CreatePanel(string panelName)
        {
            var panel = this.app.CreateRibbonPanel(panelName);
            var itembuilder = new RibbonPanelManager(null, panel, this.imageManager);
            return itembuilder;
        }

        /// <summary>
        /// Creates a Ribbon Panel
        /// </summary>
        public IRibbonPanelManager CreatePanel(string tabName, string panelName)
        {
            try
            {
                this.app.CreateRibbonTab(tabName);
            }
            catch
            {
            }

            var panel = this.app.CreateRibbonPanel(tabName, panelName);
            var itembuilder = new RibbonPanelManager(tabName, panel, this.imageManager);
            return itembuilder;
        }

        /// <summary>
        /// Get Revit UI Controlled App
        /// </summary>
        /// <returns></returns>
        public UIControlledApplication GetApp()
        {
            return this.app;
        }

        /// <summary>
        /// Gets Ribbon line break string
        /// </summary>
        public string GetLineBreak()
        {
            return "\r\n";
        }
    }

    /// <summary>
    /// Ribbon Manager will help you create Tabs, Panels and Buttons in Revit
    /// </summary>
    public class RibbonTabManager : IRibbonTabManager
    {
        private readonly UIControlledApplication app;
        private readonly string tabName;
        private readonly ImageManager imageManager;

        /// <summary>
        /// Ribbon Manager will help you create Tabs, Panels and Buttons in Revit
        /// </summary>
        public RibbonTabManager(UIControlledApplication app, string tabName, ImageManager imageManager)
        {
            this.app = app;
            this.tabName = tabName;
            this.imageManager = imageManager;
        }

        /// <summary>
        /// Gets the current tab name.
        /// </summary>
        public string GetTabName()
        {
            return this.tabName;
        }

        /// <summary>
        /// Creates a Ribbon Panel
        /// </summary>
        public IRibbonPanelManager CreatePanel(string panelName)
        {
            var panel = app.CreateRibbonPanel(panelName);
            var itembuilder = new RibbonPanelManager(this.tabName, panel, this.imageManager);
            return itembuilder;
        }
    }

    /// <summary>
    /// The resulting of adding a new tab in Revit
    /// </summary>
    public interface IRibbonTabManager
    {
        /// <summary>
        /// Returns the tab name
        /// </summary>
        string GetTabName();
    }

    /// <summary>
    /// The responsible for adding UI Elements in the Ribbon, e.g. Buttons and Separators.
    /// </summary>
    public interface IRibbonPanelManager
    {
        /// <summary>
        /// Adds a separator in the current panel.
        /// </summary>
        void AddSeparator();
        /// <summary>
        /// Adds a slide out in the current panel.
        /// </summary>
        void AddSlideOut();
        /// <summary>
        /// Adds a stacked containing 2 UI Elements in the current panel.
        /// </summary>
        /// <param name="item1">Top UI Element.</param>
        /// <param name="item2">Bottom UI Element.</param>
        /// <returns>The list of elements added</returns>
        IList<RibbonItem> AddStackedItems(RibbonItemData item1, RibbonItemData item2);
        /// <summary>
        /// Adds a stacked containing 3 UI Elements in the current panel.
        /// </summary>
        /// <param name="item1">Top UI Element.</param>
        /// <param name="item2">Middle UI Element.</param>
        /// <param name="item3">Bottom UI Element.</param>
        /// <returns>The list of elements added</returns>
        IList<RibbonItem> AddStackedItems(RibbonItemData item1, RibbonItemData item2, RibbonItemData item3);
        /// <summary>
        /// Adds a new PushButton for a Command in the Revit UI with the default CommandAvailability.
        /// </summary>
        /// <typeparam name="TExternalCommand">The type of the External Command.</typeparam>
        /// <param name="name">The name of the Command as it appears in Revit UI.</param>
        /// <param name="image">The button image name, no path. The image should be a resource in the current project.</param>
        /// <param name="tooltip">The tooltip that Revit wil show for this button.</param>
        /// <param name="description">The description that Revit wil show for this button.</param>
        /// <returns>The created PushButton.</returns>
        PushButton AddPushButton<TExternalCommand>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new();
        /// <summary>
        /// Adds a new PushButton for a Command in the Revit UI with a custom CommandAvailability.
        /// </summary>
        /// <typeparam name="TExternalCommand">The type of the External Command.</typeparam>
        /// <typeparam name="TAvailability">The type of the Command Availability.</typeparam>
        /// <param name="name">The name of the Command as it appears in Revit UI.</param>
        /// <param name="image">The button image name, no path. The image should be a resource in the current project.</param>
        /// <param name="tooltip">The tooltip that Revit wil show for this button.</param>
        /// <param name="description">The description that Revit wil show for this button.</param>
        /// <returns>The created PushButton.</returns>
        PushButton AddPushButton<TExternalCommand, TAvailability>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new();
        /// <summary>
        /// Creates a new PushbuttonData for a Command with the default CommandAvailability, to be later added to another UI Element.
        /// </summary>
        /// <typeparam name="TExternalCommand">The type of the External Command.</typeparam>
        /// <param name="name">The name of the Command as it appears in Revit UI.</param>
        /// <param name="image">The button image name, no path. The image should be a resource in the current project.</param>
        /// <param name="tooltip">The tooltip that Revit wil show for this button.</param>
        /// <param name="description">The description that Revit wil show for this button.</param>
        /// <returns>The created PushButtonData.</returns>
        PushButtonData CreatePushButtonData<TExternalCommand>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new();
        /// <summary>
        /// Creates a new PushbuttonData for a Command with a custom CommandAvailability, to be later added to another UI Element.
        /// </summary>
        /// <typeparam name="TExternalCommand">The type of the External Command.</typeparam>
        /// <typeparam name="TAvailability">The type of the Command Availability.</typeparam>
        /// <param name="name">The name of the Command as it appears in Revit UI.</param>
        /// <param name="image">The button image name, no path. The image should be a resource in the current project.</param>
        /// <param name="tooltip">The tooltip that Revit wil show for this button.</param>
        /// <param name="description">The description that Revit wil show for this button.</param>
        /// <returns>The created PushButtonData.</returns>
        PushButtonData CreatePushButtonData<TExternalCommand, TAvailability>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new();
        /// <summary>
        /// Adds a Splitbutton in the Revit UI.
        /// </summary>
        /// <param name="pushButtonDataList">The list of PushButtons to add.</param>
        /// <returns>The created SplitButton</returns>
        SplitButton AddSplitButton(List<PushButtonData> pushButtonDataList);
        /// <summary>
        /// Adds a new ToggleButton for a Command in the Revit UI with the default CommandAvailability.
        /// </summary>
        /// <typeparam name="TExternalCommand">The type of the External Command.</typeparam>
        /// <param name="name">The name of the Command as it appears in Revit UI.</param>
        /// <param name="image">The button image name, no path. The image should be a resource in the current project.</param>
        /// <param name="tooltip">The tooltip that Revit wil show for this button.</param>
        /// <param name="description">The description that Revit wil show for this button.</param>
        /// <returns>The created ToggleButton.</returns>
        ToggleButton AddToggleButton<TExternalCommand>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new();
        /// <summary>
        /// Adds a new ToggleButton for a Command in the Revit UI with a custom CommandAvailability.
        /// </summary>
        /// <typeparam name="TExternalCommand">The type of the External Command.</typeparam>
        /// <typeparam name="TAvailability">The type of the Command Availability.</typeparam>
        /// <param name="name">The name of the Command as it appears in Revit UI.</param>
        /// <param name="image">The button image name, no path. The image should be a resource in the current project.</param>
        /// <param name="tooltip">The tooltip that Revit wil show for this button.</param>
        /// <param name="description">The description that Revit wil show for this button.</param>
        /// <returns>The created ToggleButton.</returns>
        ToggleButton AddToggleButton<TExternalCommand, TAvailability>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new();
        
        /// <summary>
        /// Gets the current panel name.
        /// </summary>
        /// <returns></returns>
        RibbonPanel GetPanel();
        
        /// <summary>
        /// Gets the current tab name.
        /// </summary>
        string GetTabName();
    }

    /// <summary>
    /// The responsible for adding UI Elements in the Ribbon, e.g. Buttons and Separators.
    /// </summary>
    public class RibbonPanelManager : IRibbonPanelManager
    {
        private readonly string tabName;
        private readonly RibbonPanel panel;
        private readonly RibbonHelpers ribbonHelpers;

        /// <summary>
        /// The responsible for adding UI Elements in the Ribbon, e.g. Buttons and Separators.
        /// </summary>
        public RibbonPanelManager(string tabName, RibbonPanel panel, ImageManager imageManager)
        {
            this.tabName = tabName;
            this.panel = panel;
            this.ribbonHelpers = new RibbonHelpers(imageManager);
        }

        /// <summary>
        /// Gets the current tab name.
        /// </summary>
        public string GetTabName()
        {
            return this.tabName;
        }

        /// <summary>
        /// Gets the current panel name.
        /// </summary>
        /// <returns></returns>
        public RibbonPanel GetPanel()
        {
            return this.panel;
        }

        /// <summary>
        /// Adds a new PushButton for a Command in the Revit UI with the default CommandAvailability.
        /// </summary>
        /// <typeparam name="TExternalCommand">The type of the External Command.</typeparam>
        /// <param name="name">The name of the Command as it appears in Revit UI.</param>
        /// <param name="image">The button image name, no path. The image should be a resource in the current project.</param>
        /// <param name="tooltip">The tooltip that Revit wil show for this button.</param>
        /// <param name="description">The description that Revit wil show for this button.</param>
        /// <returns>The created PushButton.</returns>
        public PushButton AddPushButton<TExternalCommand>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new()
        {
            return ribbonHelpers.CreatePushButton(panel, name, image, typeof(TExternalCommand), tooltip, description);
        }
        
        /// <summary>
        /// Adds a new PushButton for a Command in the Revit UI with a custom CommandAvailability.
        /// </summary>
        /// <typeparam name="TExternalCommand">The type of the External Command.</typeparam>
        /// <typeparam name="TAvailability">The type of the Command Availability.</typeparam>
        /// <param name="name">The name of the Command as it appears in Revit UI.</param>
        /// <param name="image">The button image name, no path. The image should be a resource in the current project.</param>
        /// <param name="tooltip">The tooltip that Revit wil show for this button.</param>
        /// <param name="description">The description that Revit wil show for this button.</param>
        /// <returns>The created PushButton.</returns>
        public PushButton AddPushButton<TExternalCommand, TAvailability>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            var button = ribbonHelpers.CreatePushButton(panel, name, image, typeof(TExternalCommand), tooltip, description);
            button.AvailabilityClassName = typeof(TAvailability).FullName;
            return button;
        }

        /// <summary>
        /// Creates a new PushbuttonData for a Command with the default CommandAvailability, to be later added to another UI Element.
        /// </summary>
        /// <typeparam name="TExternalCommand">The type of the External Command.</typeparam>
        /// <param name="name">The name of the Command as it appears in Revit UI.</param>
        /// <param name="image">The button image name, no path. The image should be a resource in the current project.</param>
        /// <param name="tooltip">The tooltip that Revit wil show for this button.</param>
        /// <param name="description">The description that Revit wil show for this button.</param>
        /// <returns>The created PushButtonData.</returns>
        public PushButtonData CreatePushButtonData<TExternalCommand>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new()
        {
            return ribbonHelpers.CreatePushButtonData(name, image, typeof(TExternalCommand), tooltip, description);
        }

        /// <summary>
        /// Creates a new PushbuttonData for a Command with a custom CommandAvailability, to be later added to another UI Element.
        /// </summary>
        /// <typeparam name="TExternalCommand">The type of the External Command.</typeparam>
        /// <typeparam name="TAvailability">The type of the Command Availability.</typeparam>
        /// <param name="name">The name of the Command as it appears in Revit UI.</param>
        /// <param name="image">The button image name, no path. The image should be a resource in the current project.</param>
        /// <param name="tooltip">The tooltip that Revit wil show for this button.</param>
        /// <param name="description">The description that Revit wil show for this button.</param>
        /// <returns>The created PushButtonData.</returns>
        public PushButtonData CreatePushButtonData<TExternalCommand, TAvailability>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            var button = ribbonHelpers.CreatePushButtonData(name, image, typeof(TExternalCommand), tooltip, description);
            button.AvailabilityClassName = typeof(TAvailability).FullName;
            return button;
        }

        /// <summary>
        /// Adds a new ToggleButton for a Command in the Revit UI with the default CommandAvailability.
        /// </summary>
        /// <typeparam name="TExternalCommand">The type of the External Command.</typeparam>
        /// <param name="name">The name of the Command as it appears in Revit UI.</param>
        /// <param name="image">The button image name, no path. The image should be a resource in the current project.</param>
        /// <param name="tooltip">The tooltip that Revit wil show for this button.</param>
        /// <param name="description">The description that Revit wil show for this button.</param>
        /// <returns>The created ToggleButton.</returns>
        public ToggleButton AddToggleButton<TExternalCommand>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new()
        {
            var button = ribbonHelpers.CreateToggleButton(panel, name, image, typeof(TExternalCommand), tooltip, description);
            return button;
        }

        /// <summary>
        /// Adds a new ToggleButton for a Command in the Revit UI with a custom CommandAvailability.
        /// </summary>
        /// <typeparam name="TExternalCommand">The type of the External Command.</typeparam>
        /// <typeparam name="TAvailability">The type of the Command Availability.</typeparam>
        /// <param name="name">The name of the Command as it appears in Revit UI.</param>
        /// <param name="image">The button image name, no path. The image should be a resource in the current project.</param>
        /// <param name="tooltip">The tooltip that Revit wil show for this button.</param>
        /// <param name="description">The description that Revit wil show for this button.</param>
        /// <returns>The created ToggleButton.</returns>
        public ToggleButton AddToggleButton<TExternalCommand, TAvailability>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            var button = ribbonHelpers.CreateToggleButton(panel, name, image, typeof(TExternalCommand), tooltip, description);
            button.AvailabilityClassName = typeof(TAvailability).FullName;
            return button;
        }

        /// <summary>
        /// Adds a Splitbutton in the Revit UI.
        /// </summary>
        /// <param name="pushButtonDataList">The list of PushButtons to add.</param>
        /// <returns>The created SplitButton</returns>
        public SplitButton AddSplitButton(List<PushButtonData> pushButtonDataList)
        {
            return ribbonHelpers.CreateSplitButton(panel, pushButtonDataList);
        }

        /// <summary>
        /// Adds a stacked containing 2 UI Elements in the current panel.
        /// </summary>
        /// <param name="item1">Top UI Element.</param>
        /// <param name="item2">Bottom UI Element.</param>
        /// <returns>The list of elements added</returns>
        public IList<RibbonItem> AddStackedItems(RibbonItemData item1, RibbonItemData item2)
        {
            return panel.AddStackedItems(item1, item2);
        }

        /// <summary>
        /// Adds a stacked containing 3 UI Elements in the current panel.
        /// </summary>
        /// <param name="item1">Top UI Element.</param>
        /// <param name="item2">Middle UI Element.</param>
        /// <param name="item3">Bottom UI Element.</param>
        /// <returns>The list of elements added</returns>
        public IList<RibbonItem> AddStackedItems(RibbonItemData item1, RibbonItemData item2, RibbonItemData item3)
        {
            return panel.AddStackedItems(item1, item2, item3);
        }

        /// <summary>
        /// Adds a separator in the current panel.
        /// </summary>
        public void AddSeparator()
        {
            panel.AddSeparator();
        }

        /// <summary>
        /// Adds a slide out in the current panel.
        /// </summary>
        public void AddSlideOut()
        {
            panel.AddSlideOut();
        }

    }
}