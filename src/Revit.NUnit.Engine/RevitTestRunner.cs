using NUnit.Engine;
using Onbox.Revit.NUnit.Engine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Revit.NUnit.Engine
{
    public interface IRevitTestRunner
    {
        XmlNode Run();
        void Run(string resultXmlFile);
    }

    public class RevitTestRunner : IRevitTestRunner
    {
        private readonly string assemblyPath;

        public RevitTestRunner(string assemblyPath)
        {
            this.assemblyPath = assemblyPath;
        }

        public XmlNode Run()
        {
            var factory = new TestRunnerFactory();
            var runner = factory.CreateTestRunner(assemblyPath);

            var result = runner.Run(new TestRunnerListener(), TestFilter.Empty);
            return result;
        }

        public void Run(string resultXmlFile)
        {
            var result = Run();
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
