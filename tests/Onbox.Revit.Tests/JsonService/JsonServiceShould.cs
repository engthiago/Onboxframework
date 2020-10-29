using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Onbox.Revit.Tests.JsonService.Dummies;
using System.Collections.Generic;

namespace Onbox.Revit.Tests.JsonService
{
    [Category("Json Service")]
    public class JsonServiceShould
    {
        private string SerializeDummyPersonDirectlyWithNewtonsoft()
        {
            return JsonConvert.SerializeObject(CreateDummyPerson());
        }

        private DummySerializationPerson CreateDummyPerson()
        {
            return new DummySerializationPerson
            {
                Age = 48,
                Name = "Eddard",
                Address = new DummySerializationAddress
                {
                    StreetName = "Winterfell",
                    StreetNumber = "Tower 1"
                },
                Children = new List<DummySerializationPerson>
                {
                    new DummySerializationPerson
                    {
                        Age = 26,
                        Name = "Robb",
                        Address = new DummySerializationAddress
                        {
                            StreetName = "Winterfell",
                            StreetNumber = "Tower 2"
                        },
                        Children = new List<DummySerializationPerson>()
                    },
                    new DummySerializationPerson
                    {
                        Age = 19,
                        Name = "Sansa",
                        Address = new DummySerializationAddress
                        {
                            StreetName = "King's Landing",
                            StreetNumber = "Tower 1"
                        },
                        Children = new List<DummySerializationPerson>()
                    },
                }
            };
        }

        [Test]
        public void SerializeJson()
        {
            var mockSettings = new Mock<JsonSerializerSettings>();

            var sut = new Core.VDev.Json.JsonService(mockSettings.Object);
            var dummyPerson = CreateDummyPerson();

            var json = sut.Serialize(dummyPerson);
            Assert.That(json, Does.Contain("Eddard"));
            Assert.That(json, Does.Contain("Robb"));
            Assert.That(json, Does.Contain("Sansa"));
            Assert.That(json, Does.Contain("48"));
            Assert.That(json, Does.Contain("26"));
            Assert.That(json, Does.Contain("19"));
            Assert.That(json, Does.Contain("Winterfell"));
            Assert.That(json, Does.Contain("Tower 2"));
            Assert.That(json, Does.Contain("King's Landing"));
        }

        [Test]
        public void DeserializeJson()
        {
            var mockSettings = new Mock<JsonSerializerSettings>();
            var sut = new Core.VDev.Json.JsonService(mockSettings.Object);

            var json = SerializeDummyPersonDirectlyWithNewtonsoft();

            var dummyPerson = sut.Deserialize<DummySerializationPerson>(json);

            Assert.That(dummyPerson.Age, Is.EqualTo(48));
            Assert.That(dummyPerson.Name, Is.EqualTo("Eddard"));
            Assert.That(dummyPerson.Children[0].Name, Is.EqualTo("Robb"));
            Assert.That(dummyPerson.Children[0].Age, Is.EqualTo(26));
            Assert.That(dummyPerson.Children[0].Address.StreetName, Is.EqualTo("Winterfell"));
            Assert.That(dummyPerson.Children[1].Name, Is.EqualTo("Sansa"));
            Assert.That(dummyPerson.Children[1].Age, Is.EqualTo(19));
            Assert.That(dummyPerson.Children[1].Address.StreetName, Is.EqualTo("King's Landing"));
        }
    }
}
