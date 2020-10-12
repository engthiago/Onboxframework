using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public class RibbonTabManager : IRibbonTabManager
    {
        private readonly UIControlledApplication app;
        private readonly string tabName;
        private readonly ImageManager imageManager;

        public RibbonTabManager(UIControlledApplication app, string tabName, ImageManager imageManager)
        {
            this.app = app;
            this.tabName = tabName;
            this.imageManager = imageManager;
        }

        public string GetTabName()
        {
            return this.tabName;
        }

        public IRibbonPanelManager CreatePanel(string panelName)
        {
            var panel = app.CreateRibbonPanel(panelName);
            var itembuilder = new RibbonPanelManager(this.tabName, panel, this.imageManager);
            return itembuilder;
        }
    }

    public interface IRibbonTabManager
    {
        string GetTabName();
    }

    public interface IRibbonPanelManager
    {
        void AddSeparator();
        void AddSlideOut();
        IList<RibbonItem> AddStackedItems(RibbonItemData item1, RibbonItemData item2);
        IList<RibbonItem> AddStackedItems(RibbonItemData item1, RibbonItemData item2, RibbonItemData item3);
        PushButton AddPushButton<TExternalCommand>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new();
        PushButton AddPushButton<TExternalCommand, TAvailability>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new();
        PushButtonData CreatePushButtonData<TExternalCommand>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new();
        PushButtonData CreatePushButtonData<TExternalCommand, TAvailability>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new();
        SplitButton AddSplitButton(List<PushButtonData> pushButtonDataList);
        ToggleButton AddToggleButton<TExternalCommand>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new();
        ToggleButton AddToggleButton<TExternalCommand, TAvailability>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new();

        RibbonPanel GetPanel();
        string GetTabName();
    }

    public class RibbonPanelManager : IRibbonPanelManager
    {
        private readonly string tabName;
        private readonly RibbonPanel panel;
        private readonly RibbonHelpers ribbonHelpers;

        public RibbonPanelManager(string tabName, RibbonPanel panel, ImageManager imageManager)
        {
            this.tabName = tabName;
            this.panel = panel;
            this.ribbonHelpers = new RibbonHelpers(imageManager);
        }

        public string GetTabName()
        {
            return this.tabName;
        }

        public RibbonPanel GetPanel()
        {
            return this.panel;
        }

        public PushButton AddPushButton<TExternalCommand>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new()
        {
            return ribbonHelpers.CreatePushButton(panel, name, image, typeof(TExternalCommand), tooltip, description);
        }

        public PushButton AddPushButton<TExternalCommand, TAvailability>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            var button = ribbonHelpers.CreatePushButton(panel, name, image, typeof(TExternalCommand), tooltip, description);
            button.AvailabilityClassName = typeof(TAvailability).FullName;
            return button;
        }

        public PushButtonData CreatePushButtonData<TExternalCommand>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new()
        {
            return ribbonHelpers.CreatePushButtonData(name, image, typeof(TExternalCommand), tooltip, description);
        }

        public PushButtonData CreatePushButtonData<TExternalCommand, TAvailability>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            var button = ribbonHelpers.CreatePushButtonData(name, image, typeof(TExternalCommand), tooltip, description);
            button.AvailabilityClassName = typeof(TAvailability).FullName;
            return button;
        }

        public ToggleButton AddToggleButton<TExternalCommand>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new()
        {
            var button = ribbonHelpers.CreateToggleButton(panel, name, image, typeof(TExternalCommand), tooltip, description);
            return button;
        }

        public ToggleButton AddToggleButton<TExternalCommand, TAvailability>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            var button = ribbonHelpers.CreateToggleButton(panel, name, image, typeof(TExternalCommand), tooltip, description);
            button.AvailabilityClassName = typeof(TAvailability).FullName;
            return button;
        }

        public SplitButton AddSplitButton(List<PushButtonData> pushButtonDataList)
        {
            return ribbonHelpers.CreateSplitButton(panel, pushButtonDataList);
        }

        public IList<RibbonItem> AddStackedItems(RibbonItemData item1, RibbonItemData item2)
        {
            return panel.AddStackedItems(item1, item2);
        }

        public IList<RibbonItem> AddStackedItems(RibbonItemData item1, RibbonItemData item2, RibbonItemData item3)
        {
            return panel.AddStackedItems(item1, item2, item3);
        }

        public void AddSeparator()
        {
            panel.AddSeparator();
        }

        public void AddSlideOut()
        {
            panel.AddSlideOut();
        }

    }
}