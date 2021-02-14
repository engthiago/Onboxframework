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

        public ITestRunner CreateTestRunner(string testAssemblyPath)
        {
            ITestEngine engine = CreateEngineInstance();
            if (engine == null)
            {
                throw new Exception("Failed to load NUnit Test Engine!");
            }

            Console.WriteLine($"Loaded Test engine: {engine.WorkDirectory}");

            var dir = Path.GetDirectoryName(testAssemblyPath);
            TestPackage package = new TestPackage(testAssemblyPath);

            if (package == null)
            {
                throw new Exception("Failed to load test assembly package!");
            }

            Console.WriteLine($"Loaded Test: {package.FullName}");
            Console.WriteLine($"Assembly Test Id: {package.ID}");
            Console.WriteLine($"Assembly Test Name: {package.Name}");

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
            Console.WriteLine($"Loading Engine From: {apiLocation}");
            var folder = Path.GetDirectoryName(apiLocation);
            foreach (var file in Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories))
            {
                Console.WriteLine($"Addin folder: {file}");
            }
            var directoryName = Path.GetDirectoryName(apiLocation);
            var enginePath = directoryName == null ? DefaultAssemblyName : Path.Combine(directoryName, DefaultAssemblyName);
            var assembly = Assembly.LoadFrom(enginePath);
            var engineType = assembly.GetType(DefaultTypeName);
            return Activator.CreateInstance(engineType) as ITestEngine;
        }
    }
}