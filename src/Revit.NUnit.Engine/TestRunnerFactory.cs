using NUnit;
using NUnit.Engine;
using NUnit.Engine.Services;
using System;
using System.IO;
using System.Reflection;

namespace Onbox.Revit.NUnit.Engine
{
    public class TestRunnerFactory
    {
        private const string DefaultAssemblyName = "nunit.engine.dll";
        private const string DefaultTypeName = "NUnit.Engine.TestEngine";

        public ITestRunner CreateTestRunner(string assemblyPath)
        {
            ITestEngine engine = CreateEngineInstance();
            var dir = Path.GetDirectoryName(assemblyPath);
            TestPackage package = new TestPackage(assemblyPath);

            string domainUsage = "None";
            string processModel = "InProcess";
            package.AddSetting(EnginePackageSettings.DomainUsage, domainUsage);
            package.AddSetting(EnginePackageSettings.ProcessModel, processModel);
            package.AddSetting(EnginePackageSettings.WorkDirectory, dir);
            var runner = engine.GetRunner(package);

            var agency = engine.Services.GetService<TestAgency>();
            if (agency != null)
            {
                agency.Stop();
            }

            return runner;
        }

        private static ITestEngine CreateEngineInstance()
        {
            var apiLocation = typeof(TestEngineActivator).Assembly.Location;
            var directoryName = Path.GetDirectoryName(apiLocation);
            var enginePath = directoryName == null ? DefaultAssemblyName : Path.Combine(directoryName, DefaultAssemblyName);
            var assembly = Assembly.LoadFrom(enginePath);
            var engineType = assembly.GetType(DefaultTypeName);
            return Activator.CreateInstance(engineType) as ITestEngine;
        }
    }
}