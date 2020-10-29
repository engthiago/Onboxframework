using System.Collections.Generic;

namespace Onbox.Revit.Tests.JsonService.Dummies
{
    public class DummySerializationPerson
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DummySerializationAddress Address { get; set; }
        public List<DummySerializationPerson> Children { get; set; }
    }
}
