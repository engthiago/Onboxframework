using System.Collections.Generic;

namespace Onbox.Revit.Tests.JsonSerializer.Dummies
{
    public class DummySerializationPerson
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public List<DummySerializationPerson> Children { get; set; }
    }
}
