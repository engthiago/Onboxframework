using NUnit.Framework;
using Onbox.Core.V4.Mapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    [TestFixture]
    public class MapperShould
    {
        public class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public Person Child { get; set; }
        }

        [Test]
        public void CloneObject()
        {
            var person = new Person
            {
                FirstName = "Thiago",
                LastName = "Almeida",
                Child = new Person
                {
                    FirstName = "Jessica",
                    LastName = "Almeida"
                }
            };

            Mapper mapper = new Mapper(null);
            var person2 = mapper.Map<Person>(person);

            person.FirstName = "Bruna";
            person.Child.FirstName = "Isa";

            Assert.That(person2.FirstName != person.FirstName);
            Assert.That(person2.LastName == person.LastName);
            Assert.That(person2.Child.FirstName != person.Child.FirstName);
        }

        [Test]
        public void MapObjects()
        {
            var person = new Person
            {
                FirstName = "Thiago",
                LastName = "Almeida",
                Child = new Person
                {
                    FirstName = "Jessica",
                    LastName = "Almeida"
                }
            };

            var person2 = new Person();
            person2.FirstName = "Bruna";

            Mapper mapper = new Mapper(null);
            mapper.Map(person, person2);

            Assert.That(person2.FirstName == person.FirstName);
            Assert.That(person2.LastName == person.LastName);
            Assert.That(person2.Child.FirstName == person.Child.FirstName);
        }

        [Test]
        public void AddConfigurator()
        {
            var mapconfig = new MappingConfigurator();
            mapconfig.AddMappingPostAction((Person p, Person p1) =>
            {
                p1.FirstName = "Daniel";
            });

            Mapper mapper = new Mapper(mapconfig);

            var person = new Person
            {
                FirstName = "Thiago",
                LastName = "Almeida",
                Child = new Person
                {
                    FirstName = "Jessica",
                    LastName = "Almeida"
                }
            };
            var person2 = mapper.Map<Person>(person);
            Assert.That(person2.FirstName == "Daniel");
        }

        [Test]
        public void MapLists()
        {
            var person = new Person { FirstName = "Thiago", LastName = "Almeida" };
            var person2 = new Person { FirstName = "Bruna", LastName = "Paes" };

            var list = new List<Person> { person, person2 };

            Mapper mapper = new Mapper(null);
            var list2 = mapper.Map<ObservableCollection<Person>>(list);

            var person3 = list2[0];
            person3.FirstName = "Nelson";

            Assert.That(list2.Count == 2);
            Assert.That(person.FirstName == "Thiago");
            Assert.That(list2[0].FirstName == "Nelson");
        }
    }
}
