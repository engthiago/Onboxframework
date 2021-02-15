using NUnit.Engine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Onbox.Revit.Remote.DA.Tests.NUnit
{
    public interface IRevitTestRunner
    {
        XmlNode Run(string testAssemblyPath);
        void Run(string testAssemblyPath, string resultXmlPath);
    }

    public class RevitTestRunner : IRevitTestRunner
    {

        public XmlNode Run(string testAssemblyPath)
        {
            var factory = new TestRunnerFactory();
            var runner = factory.CreateTestRunner(testAssemblyPath);

            var result = runner.Run(new TestRunnerListener(), TestFilter.Empty);
            return result;
        }

        public void Run(string assemblyPath, string resultXmlFile)
        {
            var result = Run(assemblyPath);
            SerializeNode(result, resultXmlFile);
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
