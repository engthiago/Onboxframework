using Autodesk.Revit.UI;
using Onbox.Revit.RibbonCommands.VDev.Attributes;
using Onbox.Revit.VDev.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Onbox.Revit.RibbonCommands.VDev
{
    public static class Extensions
    {
        private static readonly RibbonHelpers ribbonHelpers = new RibbonHelpers(new ImageManager());

        /// <summary>
        /// Automatically adds buttons for Revit Commands decorated with: <see cref="RibbonPushButtonAttribute"/>, <see cref="RibbonSplitButtonAttribute"/> and <see cref="RibbonStackButtonAttribute"/>
        /// </summary>
        /// <param name="ribbonManager"></param>
        /// <param name="config">Configuration options for automatically adding Ribbon Panels and Buttons.</param>
        public static void AddRibbonCommands(this IRibbonManager ribbonManager, Action<RibbonCommandConfiguration> config = null)
        {
            var assembly = Assembly.GetCallingAssembly();
            var assemblyName = assembly.GetName().Name;

            var commandItemType = typeof(IRibbonCommandAttribute);

            var ribbonConfig = new RibbonCommandConfiguration();
            ribbonConfig.DefaultPanelName = assemblyName;
            ribbonConfig.TabName = assemblyName;

            config?.Invoke(ribbonConfig);

            var types = assembly.GetTypes();

            var ribbonCommands = types
                .Where(t => t.GetInterfaces().Contains(typeof(IExternalCommand)))
                .Where(t => t.GetCustomAttributes().Any(a => a.GetType().GetInterfaces().Contains(commandItemType)));

            var ribbonCommandDataList = BuildCommandDataStructure(ribbonManager, ribbonCommands, ribbonConfig);
            CreateCommandButtons(ribbonCommandDataList);
        }

        private static void CreateCommandButtons(List<RibbonCommandData> ribbonCommandDataList)
        {
            var addedSplitButtonGroups = new List<string>();
            var addedStackButtonGroups = new List<string>();

            foreach (var commandData in ribbonCommandDataList)
            {
                if (commandData.commandAttribute is RibbonPushButtonAttribute pushButtonAttr)
                {
                    try
                    {
                        commandData.ribbonPanel.AddItem(commandData.button);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else if (commandData.commandAttribute is RibbonSplitButtonAttribute splitButtonAttr)
                {
                    if (!addedSplitButtonGroups.Contains(splitButtonAttr.SplitButtonGroup))
                    {
                        addedSplitButtonGroups.Add(splitButtonAttr.SplitButtonGroup);

                        // Ignore buttons with not group name
                        if (string.IsNullOrWhiteSpace(splitButtonAttr.SplitButtonGroup))
                        {
                            continue;
                        }

                        var items = ribbonCommandDataList
                            .Where(d => d.commandAttribute.PanelName == commandData.commandAttribute.PanelName)
                            .Where(d => d.commandAttribute is RibbonSplitButtonAttribute)
                            .Where(d => (d.commandAttribute as RibbonSplitButtonAttribute).SplitButtonGroup == splitButtonAttr.SplitButtonGroup)
                            .OrderByDescending(d => (d.commandAttribute as RibbonSplitButtonAttribute).SplitGroupPriority);

                        var splitGroup = commandData.ribbonPanel.AddItem(new SplitButtonData(splitButtonAttr.SplitButtonGroup, splitButtonAttr.SplitButtonGroup)) as SplitButton;
                        foreach (var item in items)
                        {
                            try
                            {
                                splitGroup.AddPushButton(item.button);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
                else if (commandData.commandAttribute is RibbonStackButtonAttribute stackButtonAttr)
                {
                    if (!addedStackButtonGroups.Contains(stackButtonAttr.StackButtonGroup))
                    {
                        addedStackButtonGroups.Add(stackButtonAttr.StackButtonGroup);

                        // Ignore buttons with not group name
                        if (string.IsNullOrWhiteSpace(stackButtonAttr.StackButtonGroup))
                        {
                            continue;
                        }

                        var items = ribbonCommandDataList
                            .Where(d => d.commandAttribute.PanelName == commandData.commandAttribute.PanelName)
                            .Where(d => d.commandAttribute is RibbonStackButtonAttribute)
                            .Where(d => (d.commandAttribute as RibbonStackButtonAttribute).StackButtonGroup == stackButtonAttr.StackButtonGroup)
                            .OrderByDescending(d => (d.commandAttribute as RibbonStackButtonAttribute).StackGroupPriority)
                            .ToList();

                        if (items.Count() >= 3)
                        {
                            try
                            {
                                var item0 = items[0];
                                var item1 = items[1];
                                var item2 = items[2];
                                commandData.ribbonPanel.AddStackedItems(item0.button, item1.button, item2.button);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (items.Count() == 2)
                        {
                            try
                            {
                                var item0 = items[0];
                                var item1 = items[1];
                                commandData.ribbonPanel.AddStackedItems(item0.button, item1.button);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
            }
        }

        private static List<RibbonCommandData> BuildCommandDataStructure(IRibbonManager ribbonManager, IEnumerable<Type> ribbonCommands, RibbonCommandConfiguration ribbonConfig)
        {
            var commandAttrType = typeof(IRibbonCommandAttribute);
            var ribbonCommandDataList = new List<RibbonCommandData>();

            ribbonManager.CreateTab(ribbonConfig.TabName);
            var panelsToCreate = ribbonConfig.panels.Distinct();
            foreach (var panelToCreate in panelsToCreate)
            {
                ribbonManager.CreatePanel(ribbonConfig.TabName, panelToCreate);
            }

            var panels = ribbonManager.GetApp().GetRibbonPanels(ribbonConfig.TabName);
            var buttonDataList = new List<PushButtonData>();

            foreach (var commandType in ribbonCommands)
            {
                foreach (var attr in commandType.GetCustomAttributes().Where(a => a.GetType().GetInterfaces().Contains(commandAttrType)))
                {
                    var commandAttr = attr as IRibbonCommandAttribute;
                    var panel = CreatePanelIfNeeded(commandAttr, ribbonManager, ribbonConfig, panels);
                    var buttonName = GetPushButtonName(commandAttr, ribbonManager, commandType);

                    // Check if a previous item was already created, this would avoid items with same name being created twice. Revit would throw exception.
                    var buttonData = buttonDataList.FirstOrDefault(b => b.Name == buttonName);
                    if (buttonData == null)
                    {
                        buttonData = CreatePushButtonData(commandAttr, buttonName, commandType);
                        buttonDataList.Add(buttonData);
                    }

                    ribbonCommandDataList.Add(new RibbonCommandData
                    {
                        button = buttonData,
                        commandAttribute = commandAttr,
                        ribbonPanel = panel
                    });
                }
            }

            ribbonCommandDataList = ribbonCommandDataList.OrderByDescending(c => c.commandAttribute.PanelPriority).ToList();
            return ribbonCommandDataList;
        }

        private static RibbonPanel CreatePanelIfNeeded(IRibbonCommandAttribute commandAttr, IRibbonManager ribbonManager, RibbonCommandConfiguration ribbonConfig, List<RibbonPanel> panels)
        {
            string panelName = string.Empty;
            if (!string.IsNullOrWhiteSpace(commandAttr.PanelName))
            {
                panelName = commandAttr.PanelName;
            }
            else
            {
                panelName = ribbonConfig.DefaultPanelName;
            }

            var panel = panels.FirstOrDefault(p => p.Name == panelName);
            if (panel == null)
            {
                var panelManager = ribbonManager.CreatePanel(ribbonConfig.TabName, panelName);
                panel = panelManager.GetPanel();
                panels.Add(panel);
            }

            return panel;
        }

        private static string GetPushButtonName(IRibbonCommandAttribute commandAttr, IRibbonManager ribbonManager, Type commandType)
        {
            var br = ribbonManager.GetLineBreak();

            string buttonName;
            if (!string.IsNullOrWhiteSpace(commandAttr.FirstLine))
            {
                buttonName = commandAttr.FirstLine;
                if (!string.IsNullOrWhiteSpace(commandAttr.SecondLine))
                {
                    buttonName += br + commandAttr.SecondLine;
                }
            }
            else
            {
                buttonName = commandType.Name.Replace("Command", "");
            }

            return buttonName;
        }

        private static PushButtonData CreatePushButtonData(IRibbonCommandAttribute commandAttr, string buttonName, Type commandType)
        {
            try
            {
                var buttonData = ribbonHelpers.CreatePushButtonData(buttonName, commandAttr.Image, commandType, commandAttr.Tooltip, commandAttr.Description);
                if (commandAttr.Availability != null)
                {
                    buttonData.AvailabilityClassName = commandAttr.Availability.FullName;
                }
                return buttonData;
            }
            catch
            {
                return null;
            }
        }
    }
}
