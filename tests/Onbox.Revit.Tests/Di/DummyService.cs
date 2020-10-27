using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Revit.Tests.Di
{
    public interface IDummyService
    {
    }

    public class DummyService : IDummyService
    {
    }

    public class DependentService
    {
        public DependentService(DummyService dummyService)
        {
        }
    }

    public class DummyHiddenConstructor
    {
        private DummyHiddenConstructor() { }
    }

    public class CircularService1
    {
        public CircularService1(CircularService1 circularService1)
        {

        }
    }

    public class CircularService2
    {
        public CircularService2(DummyService dummyService, CircularService3 circularService3)
        {
        }
    }

    public class CircularService3
    {
        public CircularService3(CircularService2 circularService2)
        {
        }
    }

    public abstract class AbstractDummyService
    {
    }
}
