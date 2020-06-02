using NUnit.Framework;
using Onbox.Core.V7;
using Onbox.Core.V7.Mapping;
using Onbox.Di.V7;
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
        private Mapper sut;

        public class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public Person Child { get; set; }
        }

        public class DifferentPerson
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }

        }

        public class FullNamedPerson
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FullName { get; set; }

            public Person Child { get; set; }
        }

        [SetUp]
        public void Setup()
        {
            var mapperConfigurator = new MapperConfigurator();
            var mapperOperator = new MapperOperator(mapperConfigurator);
            sut = new Mapper(mapperOperator);
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

            var person2 = sut.Map<Person>(person);

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

            sut.Map(person, person2);

            Assert.That(person2.FirstName == person.FirstName);
            Assert.That(person2.LastName == person.LastName);
            Assert.That(person2.Child.FirstName == person.Child.FirstName);
        }

        [Test]
        public void AddMappingPostAction()
        {
            var mapConfig = new MapperConfigurator();
            var mapOperator = new MapperOperator(mapConfig);
            mapConfig.AddMappingPostAction((Person p, Person p1) =>
            {
                p1.FirstName = "Daniel";
            });

            Mapper mapper = new Mapper(mapOperator);

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
        public void UseContainersPostActions()
        {
            var container = new Container();
            container.AddMapper(config =>
            {
                config.AddMappingPostAction((Person p, Person p1) =>
                {
                    p1.FirstName = "Daniel";
                });
            });

            var mapper = container.Resolve<IMapper>();

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
            Assert.That(person2.LastName == "Almeida");
        }


        [Test]
        public void MapToDifferentTypesAutomatically()
        {
            var container = new Container();
            container.AddMapper();

            var mapper = container.Resolve<IMapper>();

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

            var person2 = mapper.Map<DifferentPerson>(person);
            Assert.That(person2.FirstName == "Thiago");
        }        
        
        [Test]
        public void MapToDifferentTypesAndRunPostActions()
        {
            var container = new Container();
            container.AddMapper(config =>
            {
                config.AddMappingPostAction((Person p, FullNamedPerson p1) =>
                {
                    p1.FullName = $"{p1.FirstName} {p1.LastName}";
                    p1.FullName.Trim();
                });
            });

            var mapper = container.Resolve<IMapper>();

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

            var person2 = mapper.Map<FullNamedPerson>(person);
            Assert.That(person2.FullName == "Thiago Almeida");
        }

        [Test]
        public void MapLists()
        {
            var person = new Person { FirstName = "Thiago", LastName = "Almeida" };
            var person2 = new Person { FirstName = "Bruna", LastName = "Paes" };

            var list = new List<Person> { person, person2 };

            var list2 = sut.Map<ObservableCollection<Person>>(list);

            var person3 = list2[0];
            person3.FirstName = "Nelson";

            Assert.That(list2.Count == 2);
            Assert.That(person.FirstName == "Thiago");
            Assert.That(list2[0].FirstName == "Nelson");
        }
    }
}
