using Autodesk.Revit.UI;
using ExtensibleStorageSample.Revit.Commands;
using ExtensibleStorageSample.Revit.Commands.Availability;
using ExtensibleStorageSample.Services;
using Onbox.Abstractions.VDev;
using Onbox.Core.VDev;
using Onbox.Revit.VDev;
using Onbox.Revit.VDev.Applications;
using Onbox.Revit.VDev.UI;
using Onbox.Revit.VDev.ExtensibleStorage;
using ExtensibleStorageSample.ExtensibleStorage;
using System;

namespace ExtensibleStorageSample.Revit
{
    [ContainerProvider("f512b186-25cb-443e-8be9-c3178b535745")]
    public class App : RevitApp
    {
        public override void OnCreateRibbon(IRibbonManager ribbonManager)
        {
            // Here you can create Ribbon tabs, panels and buttons

            var br = ribbonManager.GetLineBreak();

            // Adds a new Ribbon Tab with a new Panel
            var panelManager = ribbonManager.CreatePanel("ExtensibleStorageSample", "Storage Panel");
            panelManager.AddPushButton<SaveToStorageCommand, AvailableOnProject>($"Save to{br}Storage", "onbox_logo");
            panelManager.AddPushButton<LoadFromStorageCommand, AvailableOnProject>($"Load from{br}Storage", "onbox_logo");
        }

        public override Result OnStartup(IContainer container, UIControlledApplication application)
        {
            // Here you can add all necessary dependencies to the container
            container.AddOnboxCore();

            container.CreateExtensibleStorageBuilder()
                     .ConfigureJsonStorage<SampleData>(config =>
                     {
                         config.SchemaGuid = Guid.NewGuid(); // Use a valid GUID
                         config.SchemaName = "SampleDataExample"; // Do not use spaces or special characters
                         config.VendorId = "OnboxFramework"; //Do not use spaces or special characters
                     })
                     .Build();

            // Add TaskDialog Service the message service
            container.AddSingleton<IMessageService, TaskMessageService>();

            return Result.Succeeded;
        }

        public override Result OnShutdown(IContainerResolver container, UIControlledApplication application)
        {
            // No Need to cleanup the Container, the framework will do it for you
            return Result.Succeeded;
        }
    }

}