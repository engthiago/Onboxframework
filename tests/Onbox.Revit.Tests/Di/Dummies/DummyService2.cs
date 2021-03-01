using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Revit.Tests.Di.Dummies
{
    public class DummyService2 : IDummyService2
    {
        private DummyService1 dummyService1;

        public DummyService2(
            DummyService1 dummyService1
            )
        {
            this.dummyService1 = dummyService1;
        }
    }
}
