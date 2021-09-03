using Autodesk.Revit.UI;
using Newtonsoft.Json.Linq;
using NUnit.Engine;
using Onbox.Abstractions.V9;
using Onbox.Core.V9;
using Onbox.Revit.Remote.DA.Tests.NUnit;
using Onbox.Revit.Tests.Di.Dummies;
using Onbox.Revit.V9;
using Onbox.Revit.V9.Applications;
using Onbox.Revit.V9.UI;
using Revit.Local.Tests.Revit.Commands;
using Revit.Local.Tests.Revit.Commands.Availability;
using Revit.Local.Tests.Services;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Revit.Local.Tests.Revit
{
    [ContainerProvider("96deedfe-ca34-49c9-b134-5a8d9435d93d")]
    public class App : RevitApp
    {
        public override void OnCreateRibbon(IRibbonManager ribbonManager)
        {
            // Here you can create Ribbon tabs, panels and buttons

            //var br = ribbonManager.GetLineBreak();

            // Adds a Ribbon Panel to the Addins tab
            //var addinPanelManager = ribbonManager.CreatePanel("Revit.Local.Tests");
        }

        UIControlledApplication app;
        IContainer container;

        public override Result OnStartup(IContainer container, UIControlledApplication application)
        {
            // Here you can add all necessary dependencies to the container
            container.AddOnboxCore();

            // Add TaskDialog Service the message service
            container.AddSingleton<IMessageService, TaskMessageService>();

            this.app = application;

            application.ControlledApplication.ApplicationInitialized += this.ControlledApplication_ApplicationInitialized;

            this.container = container;
            return Result.Succeeded;
        }

        private void ControlledApplication_ApplicationInitialized(object sender, Autodesk.Revit.DB.Events.ApplicationInitializedEventArgs e)
        {
            var d = new DummyService1();

            TestRunnerFactory factory = new TestRunnerFactory();

            //var test_assembly = container.Resolve<IJsonService>().Deserialize<JObject>("test_assembly.json");
            //var assemblyPath = test_assembly["assembly"].ToString();
            var runner = factory.CreateTestRunner("Onbox.Revit.Tests.dll");
            var result = runner.Run(new TestRunnerListener(), TestFilter.Empty);
            XmlSerializer ser = new XmlSerializer(typeof(XmlNode));
            TextWriter writer = new StreamWriter("c:/thiago/result.xml");
            ser.Serialize(writer, result);
            writer.Close();
        }


        public override Result OnShutdown(IContainerResolver container, UIControlledApplication application)
        {
            // No Need to cleanup the Container, the framework will do it for you
            return Result.Succeeded;
        }
    }

}