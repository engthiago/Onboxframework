using Autodesk.Revit.UI;
using MvcApplication.Revit.Commands;
using MvcApplication.Revit.Commands.Availability;
using MvcApplication.Views;
using Onbox.Abstractions.VDev;
using Onbox.Core.VDev;
using Onbox.Mvc.Revit.VDev;
using Onbox.Mvc.VDev.Messaging;
using Onbox.Revit.VDev;
using Onbox.Revit.VDev.Applications;
using Onbox.Revit.VDev.UI;

namespace MvcApplication.Revit
{
    [ContainerProvider("650ccca5-fecb-4d84-9cad-544fadd06bb5")]
    public class App : RevitApp
    {
        public override void OnCreateRibbon(IRibbonManager ribbonManager)
        {
            // Here you can create Ribbon tabs, panels and buttons

            var br = ribbonManager.GetLineBreak();

            // Adds a Ribbon Panel to the Addins tab
            var addinPanelManager = ribbonManager.CreatePanel("Onbox.MvcApplication");
            addinPanelManager.AddPushButton<HelloCommand, AvailableOnProject>($"Hello{br}Framework", "onbox_logo");

            var ribbonName = "Onbox.MvcApplication";

            // Adds a new Ribbon Tab with a new Panel
            var panelHello = ribbonManager.CreatePanel(ribbonName, "Hello Panel");
            panelHello.AddPushButton<HelloCommand, AvailableOnProject>($"Hello{br}Framework", "onbox_logo");
            panelHello.AddPushButton<WPFViewCommand, AvailableOnProject>($"Hello{br}WPF", "onbox_logo");

            var panelAsync = ribbonManager.CreatePanel(ribbonName, "Async Panel");
            panelAsync.AddPushButton<ThisWillAsyncLoadViewCommand, AvailableOnProject>($"ImgButton{br}View", "onbox_logo");
            panelAsync.AddPushButton<ThisWillShowAnErrorViewCommand, AvailableOnProject>($"Error{br}View", "onbox_logo");
            panelAsync.AddPushButton<ThisWillShowAWarningViewCommand, AvailableOnProject>($"Warning{br}View", "onbox_logo");
        }

        public override Result OnStartup(IContainer container, UIControlledApplication application)
        {
            // Here you can add all necessary dependencies to the container

            container.AddOnboxCore();
            container.AddRevitMvc();

            // Registers IWPFView to the container
            // Views should ALWAYS be added as Transients
            container.AddTransient<IHelloWpfView, HelloWpfView>();

            container.AddTransient<IThisWillAsyncLoadView, ThisWillAsyncLoadView>();
            container.AddTransient<IThisWillShowAnErrorView, ThisWillShowAnErrorView>();
            container.AddTransient<IThisWillShowAWarningView, ThisWillShowAWarningView>();

            // Adds MessageBoxService to the container
            container.AddSingleton<IMessageService, MessageBoxService>();

            return Result.Succeeded;
        }

        public override Result OnShutdown(IContainerResolver container, UIControlledApplication application)
        {
            // No Need to cleanup the Container, the framework will do it for you
            return Result.Succeeded;
        }
    }

}