using System.Collections.Generic;

namespace Onbox.Revit.Tests.JsonSerializer
{
    public class DummySerializationPerson
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public List<DummySerializationPerson> Children { get; set; }
    }

    public class DummySerializationAddress
    {
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
    }
}
