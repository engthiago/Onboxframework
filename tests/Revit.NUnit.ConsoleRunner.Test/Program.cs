using Revit.NUnit.Engine;

namespace Revit.NUnit.ConsoleRunner.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var testRunner = new RevitTestRunner();

            var result = testRunner.Run(@"C:\Users\Thiago\source\repos\engthiago\OnboxFramework\tests\Onbox.Revit.Remote.Tests\bin\x64\Debug\Onbox.Revit.Remote.Tests.dll");
        }
    }
}
