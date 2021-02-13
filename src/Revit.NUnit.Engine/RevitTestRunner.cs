using NUnit.Engine;
using Onbox.Revit.NUnit.Engine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Revit.NUnit.Engine
{
    public interface IRevitTestRunner
    {
        XmlNode Run(string assemblyPath);
        void Run(string assemblyPath, string resultXmlFile);
    }

    public class RevitTestRunner : IRevitTestRunner
    {
        public XmlNode Run(string assemblyPath)
        {
            var dir = Directory.GetCurrentDirectory();
            var path = Path.Combine(dir, assemblyPath);

            var factory = new TestRunnerFactory();
            var runner = factory.CreateTestRunner(path);

            var result = runner.Run(new TestRunnerListener(), TestFilter.Empty);
            return result;
        }

        public void Run(string assemblyPath, string resultXmlFile)
        {
            var result = Run(assemblyPath);
            this.SerializeNode(result, resultXmlFile);
        }

        private void SerializeNode(XmlNode node, string filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(XmlNode));
            TextWriter writer = new StreamWriter(filename);
            ser.Serialize(writer, node);
            writer.Close();
        }
    }
}
