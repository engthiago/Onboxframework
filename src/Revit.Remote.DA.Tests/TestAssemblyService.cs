using Newtonsoft.Json;
using Onbox.Revit.Remote.DAInternal;
using System;
using System.IO;
using System.Linq;

namespace Onbox.Revit.Remote.DA.Tests
{
    public class TestAssemblyService
    {
        public string GetAssemblyFullPath(string assemblyName)
        {
            try
            {
                var workDir = RemoteContainer.GetWorkDirectory();
                var files = Directory.GetFiles(workDir, "*.dll", SearchOption.AllDirectories);
                return files.First(f => Path.GetFileName(f) == assemblyName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error enumerating test assembly: {assemblyName}");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

        public string GetAssemblyName(string configFile)
        {
            try
            {
                var json = File.ReadAllText(configFile);
                var config = JsonConvert.DeserializeObject<TestConfiguration>(json);
                return config.AssemblyPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error trying to deserialize '{configFile}':");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }
    }
}
