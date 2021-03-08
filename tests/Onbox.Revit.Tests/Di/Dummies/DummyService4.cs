using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Revit.Tests.Di.Dummies
{
    public class DummyService4
    {
        private readonly IDummyService3 dummyService3;
        private readonly IDummyService2 dummyService2;

        public DummyService4(
            IDummyService3 dummyService3,
            IDummyService2 dummyService2
            )
        {
            this.dummyService3 = dummyService3;
            this.dummyService2 = dummyService2;
        }
    }
}
