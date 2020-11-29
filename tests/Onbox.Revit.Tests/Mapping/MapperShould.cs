using NUnit.Framework;
using Onbox.Core.VDev.Mapping;
using Onbox.Revit.Tests.Mapping.Dummies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Revit.Tests.Mapping
{
    public class MapperShould
    {
        private Person CreatePerson()
        {
            var father = new Person
            {
                FirstName = "Rickard",
                LastName = "Stark",
                Age = 65,
            };

            var person = new Person
            {
                FirstName = "Eddard",
                LastName = "Stark",
                Age = 46,
                Father = father,
            };

            father.Children = new List<Person> { person };

            person.Children = new List<Person>
            {
                new Person
                {
                    FirstName = "Robb",
                    LastName = "Stark",
                    Father = person,
                    Age = 19,
                },
            };

            return person;
        }

        [TestCase]
        public void CloneAndCopyObjects()
        {
            var mapperManager = new MapperActionManager();
            var mapperOperator = new MapperOperator(mapperManager);
            var sut = new Mapper(mapperOperator);
            
            var person1 = CreatePerson();
            var person2 = sut.Clone(person1);

            Assert.That(person2.Age == person2.Age);
            Assert.That(person2.FirstName == person2.FirstName);
            Assert.That(person2.Father != null);
            Assert.That(person2.Father.Children != null);
        }
    }
}
