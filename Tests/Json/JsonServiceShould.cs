using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Onbox.Core.V5.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Json
{
    [TestFixture]
    public class JsonServiceShould
    {
        JsonService jsonService;
        string jsonExample;

        [OneTimeSetUp]
        public void Setup()
        {
            this.jsonService = new JsonService(new Newtonsoft.Json.JsonSerializerSettings());
            this.jsonExample = System.IO.File.ReadAllText("JsonExample.json");
        }

        [Test]
        public void DeserializeJson()
        {
            var jObject = this.jsonService.Deserialize<JObject>(this.jsonExample);
            Assert.That(jObject, Is.Not.Null);
        }

        [Test]
        public void SerializeJson()
        {
            var manager = new
            {
                name = "Thiago",
                lastName = "Almeida",
                devs = new []
                {
                    "Raphael", "Eduardo", "Ramoon"
                }
            };

            var jsonString = this.jsonService.Serialize(manager);
            Assert.That(jsonString, Is.InstanceOf<string>()
                                    .And.Contains("Thiago")
                                    .And.Contain("Almeida")
                                    .And.Contain("Raphael"));
        }
    }
}

