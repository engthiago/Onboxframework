using NUnit.Framework;
using Onbox.Core.VDev.Mapping;
using Onbox.Revit.Tests.Mapping.Dummies;
using System.Collections.Generic;

namespace Onbox.Revit.Tests.Mapping
{
    public partial class MapperShould
    {
        private Mapper SetupMapper()
        {
            var mapperManager = new MapperActionManager();
            var mapperOperator = new MapperOperator(mapperManager);
            var mapper = new Mapper(mapperOperator);
            return mapper;
        }

        private List<Person> SetupPersonList()
        {
            return new List<Person>
            {
                new Person { Age = 10, FirstName = "Jon", LastName = "Snow"},
                new Person { Age = 8, FirstName = "Sansa", LastName = "Stark"},
                new Person { Age = 4, FirstName = "Arya", LastName = "Stark"},
                new Person { Age = 12, FirstName = "Robb", LastName = "Stark"},
            };
        }

        private Person SetupPerson()
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

            var sister = new Person
            {
                FirstName = "Allana",
                LastName = "Stark",
                Age = 40,
                Father = father,
            };

            father.Children = new List<Person> { person, sister };

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

        [Test]
        public void CloneObject()
        {
            var sut = this.SetupMapper();

            var person1 = this.SetupPerson();
            var clone = sut.Clone(person1);

            Assert.AreNotSame(clone, person1);
        }

        [Test]
        public void CloneAndCastObject()
        {
            var sut = this.SetupMapper();

            var person1 = this.SetupPerson() as object;
            var clone = sut.Clone<Person>(person1);

            Assert.AreNotSame(clone, person1);
        }

        [Test]
        public void CloneObjectAndCopySimpleProperties()
        {
            var sut = this.SetupMapper();

            var person1 = this.SetupPerson();
            var clone = sut.Clone(person1);

            Assert.AreEqual(clone.FirstName, person1.FirstName);
            Assert.AreEqual(clone.LastName, person1.LastName);
            Assert.AreEqual(clone.Age, person1.Age);
        }

        [Test]
        public void CloneNestedObjectsAndCopySimpleProperties()
        {
            var sut = this.SetupMapper();

            var person1 = this.SetupPerson();
            var clone = sut.Clone(person1);

            Assert.AreEqual(clone.Father.FirstName, person1.Father.FirstName);
            Assert.AreEqual(clone.Father.LastName, person1.Father.LastName);
            Assert.AreEqual(clone.Father.Age, person1.Father.Age);
        }

        [Test]
        public void CloneLists()
        {
            var sut = this.SetupMapper();
            List<Person> personList = SetupPersonList();

            var clone = sut.Clone(personList);

            Assert.AreNotSame(sut, clone);
        }

        [Test]
        public void CloneListItems()
        {
            var sut = this.SetupMapper();
            List<Person> personList = SetupPersonList();

            var clone = sut.Clone(personList);

            for (int i = 0; i < clone.Count; i++)
            {
                var person1 = personList[i];
                var clone1 = clone[i];

                Assert.AreNotSame(clone1, person1);
                Assert.AreEqual(clone1.FirstName, person1.FirstName);
                Assert.AreEqual(clone1.LastName, person1.LastName);
                Assert.AreEqual(clone1.Age, person1.Age);
            }
        }

        [Test]
        public void CloneNestedListsInNestedObject()
        {
            var sut = this.SetupMapper();

            var person1 = this.SetupPerson();
            var clone = sut.Clone(person1);

            Assert.AreNotSame(clone.Father.Children, person1.Father.Children);
            Assert.AreEqual(clone.Father.Children.Count, person1.Father.Children.Count);
        }

        [Test]
        public void CloneObjectAndReferenceCircularReferences()
        {
            var sut = this.SetupMapper();

            var person1 = this.SetupPerson();
            var clone = sut.Clone(person1);

            Assert.AreSame(clone, clone.Children[0].Father);
        }

        [Test]
        public void CloneObjectAndReferenceCircularReferencesWhenNestedInLists()
        {
            var sut = this.SetupMapper();

            var person1 = this.SetupPerson();
            var clone = sut.Clone(person1);

            Assert.AreSame(clone, clone.Father.Children[0]);
        }

        [Test]
        public void CloneObjectAndRefenceSameLists()
        {
            var sut = this.SetupMapper();

            var husband = new Person { Age = 46, FirstName = "Eddard", LastName = "Stark" };
            var wife = new Person { Age = 44, FirstName = "Catelyn", LastName = "Stark" };

            var children = this.SetupPersonList();

            husband.Children = children;
            wife.Children = children;

            var marriage = new Marriage { Husband = husband, Wife = wife };

            var clone = sut.Clone(marriage);

            Assert.AreSame(clone.Husband.Children, clone.Wife.Children);
        }

        [Test]
        public void CloneObjectAndRefenceListObjects()
        {
            var sut = this.SetupMapper();

            var husband = new Person { Age = 46, FirstName = "Eddard", LastName = "Stark" };
            var wife = new Person { Age = 44, FirstName = "Catelyn", LastName = "Stark" };

            var children = this.SetupPersonList();

            husband.Children = children;
            wife.Children = children;

            var marriage = new Marriage { Husband = husband, Wife = wife };

            var clone = sut.Clone(marriage);

            for (int i = 0; i < clone.Husband.Children.Count; i++)
            {
                var child1 = clone.Husband.Children[i];
                var child2 = clone.Wife.Children[i];

                Assert.AreSame(child1, child2);
            }
        }

        [Test]
        public void CloneNestedLists()
        {
            var sut = this.SetupMapper();

            var person1 = this.SetupPerson();
            var clone = sut.Clone(person1);

            Assert.AreNotSame(clone.Children, person1.Children);
            Assert.AreEqual(clone.Children.Count, person1.Children.Count);
        }

        [Test]
        public void CloneObjectsInsideLists()
        {
            var sut = this.SetupMapper();

            var person1 = this.SetupPerson();
            var clone = sut.Clone(person1);

            for (int i = 0; i < person1.Children.Count; i++)
            {
                var person1Child = person1.Children[i];
                var cloneChild = clone.Children[i];

                Assert.AreNotSame(cloneChild, person1Child);
                Assert.AreEqual(cloneChild.FirstName, person1Child.FirstName);
                Assert.AreEqual(cloneChild.LastName, person1Child.LastName);
                Assert.AreEqual(cloneChild.Children, person1Child.Children);
                Assert.AreEqual(cloneChild.Age, person1Child.Age);
            }
        }

        [Test]
        public void CloneListsOfLists()
        {
            var sut = this.SetupMapper();

            var listOfList = new List<List<Person>>();

            var subList1 = new List<Person>
                {
                    new Person { Age = 1, FirstName = "A", LastName = "B"},
                    new Person { Age = 2, FirstName = "C", LastName = "D"},
                    new Person { Age = 3, FirstName = "E", LastName = "F"},
                };

            var subList2 = new List<Person>
                {
                    new Person { Age = 4, FirstName = "F", LastName = "G"},
                    new Person { Age = 5, FirstName = "H", LastName = "I"},
                    new Person { Age = 6, FirstName = "J", LastName = "L"},
                };

            listOfList.Add(subList1);
            listOfList.Add(subList2);
            listOfList.Add(subList1);

            var clone = sut.Clone(listOfList);

            Assert.AreNotSame(listOfList, clone);
        }

        [Test]
        public void CloneListsOfListsAndPreserveListReference()
        {
            var sut = this.SetupMapper();

            var listOfList = new List<List<Person>>();

            var subList1 = new List<Person>
                {
                    new Person { Age = 1, FirstName = "A", LastName = "B"},
                    new Person { Age = 2, FirstName = "C", LastName = "D"},
                    new Person { Age = 3, FirstName = "E", LastName = "F"},
                };

            var subList2 = new List<Person>
                {
                    new Person { Age = 4, FirstName = "F", LastName = "G"},
                    new Person { Age = 5, FirstName = "H", LastName = "I"},
                    new Person { Age = 6, FirstName = "J", LastName = "L"},
                };

            listOfList.Add(subList1);
            listOfList.Add(subList2);
            listOfList.Add(subList1);

            var clone = sut.Clone(listOfList);

            Assert.AreNotSame(clone[0], clone[1]);
            Assert.AreSame(clone[0], clone[2]);
        }

        [Test]
        public void CloneArrayOfArrays()
        {
            var sut = this.SetupMapper();

            var person1 = new Person { Age = 8, FirstName = "A", LastName = "B" };
            var person2 = new Person { Age = 9, FirstName = "C", LastName = "D" };
            var person3 = new Person { Age = 10, FirstName = "E", LastName = "F" };

            var personArray = new Person[]
            {
                person1, person2, person3
            };

            var arrayOfArrays = new ArraysOfArrays
            {
                ArraysOfPersonArray = new Person[][]
                {
                    personArray
                }
            };

            var clone = sut.Clone(arrayOfArrays);
            Assert.AreNotSame(arrayOfArrays, clone);
            Assert.AreNotSame(arrayOfArrays.ArraysOfPersonArray, clone.ArraysOfPersonArray);
            Assert.NotNull(clone.ArraysOfPersonArray);
        }

        [Test]
        public void CloneArrayOfArraysAndPreserveRefences()
        {
            var sut = this.SetupMapper();

            var person1 = new Person { Age = 8, FirstName = "A", LastName = "B" };
            var person2 = new Person { Age = 9, FirstName = "C", LastName = "D" };
            var person3 = new Person { Age = 10, FirstName = "E", LastName = "F" };

            var personArray = new Person[]
            {
                person1, person2, person3
            };

            var arrayOfArrays = new ArraysOfArrays
            {
                ArraysOfPersonArray = new Person[][]
                {
                    personArray, personArray, personArray
                }
            };

            var clone = sut.Clone(arrayOfArrays);
            Assert.AreSame(clone.ArraysOfPersonArray[0], clone.ArraysOfPersonArray[1]);
        }

        [Test]
        public void CloneArrayOfLists()
        {
            var sut = this.SetupMapper();

            var person1 = new Person { Age = 8, FirstName = "A", LastName = "B" };
            var person2 = new Person { Age = 9, FirstName = "C", LastName = "D" };
            var person3 = new Person { Age = 10, FirstName = "E", LastName = "F" };

            var personArray = new List<Person>
            {
                person1, person2, person3
            };

            var arrayOfArrays = new ArrayOfLists
            {
                ArrayOfPersonList = new List<Person>[]
                {
                    personArray, personArray, personArray
                }
            };

            var clone = sut.Clone(arrayOfArrays);
            Assert.AreSame(clone.ArrayOfPersonList[0], clone.ArrayOfPersonList[1]);
        }

        [Test]
        public void CloneListOfPrimitives()
        {
            var sut = this.SetupMapper();

            var list = new List<int>
            {
                0, 1, 2, 3, 4, 5
            };

            var clone = sut.Clone(list);

            Assert.AreNotSame(list, clone);
        }

        [Test]
        public void CloneListOfPrimitiveValues()
        {
            var sut = this.SetupMapper();

            var list = new List<int>
            {
                0, 1, 2, 3, 4, 5
            };

            var clone = sut.Clone(list);

            for (int i = 0; i < list.Count; i++)
            {
                var n1 = list[i];
                var n2 = clone[i];

                Assert.AreEqual(n1, n2);
            }
        }

        [Test]
        public void CloneArrayOfPrimitives()
        {
            var sut = this.SetupMapper();

            var persons = new ArrayObject();
            persons.Array = new int[]
            {
                0, 1, 2, 3, 4, 5
            };

            var clone = sut.Clone(persons);

            Assert.IsNotNull(clone.Array);
            Assert.AreNotSame(persons.Array, clone.Array);
        }

        [Test]
        public void CloneArrayOfClasses()
        {
            var sut = this.SetupMapper();
            var persons = new ArrayObject()
            {
                ChildrenArray = new ArrayObject[]
                {
                    new ArrayObject(), new ArrayObject()
                }
            };

            var clone = sut.Clone(persons);

            Assert.IsNotNull(clone.ChildrenArray);
            Assert.AreNotSame(persons.ChildrenArray, clone.ChildrenArray);
        }

        [Test]
        public void CloneArrayOfClassesAndTheirClasses()
        {
            var sut = this.SetupMapper();
            var persons = new ArrayObject();

            persons.ChildrenArray = new ArrayObject[]
            {
                new ArrayObject(), new ArrayObject()
            };

            var clone = sut.Clone(persons);

            for (int i = 0; i < persons.ChildrenArray.Length; i++)
            {
                var n1 = persons.ChildrenArray[i];
                var n2 = clone.ChildrenArray[i];

                Assert.AreNotSame(n1, n2);
            }
        }

        [Test]
        public void CloneArrayOfClassesAndTheirClassesAndReferenceSameObjects()
        {
            var sut = this.SetupMapper();
            var persons = new ArrayObject();

            persons.ChildrenArray = new ArrayObject[]
            {
                persons
            };

            var clone = sut.Clone(persons);

            Assert.AreSame(clone, clone.ChildrenArray[0]);

            for (int i = 0; i < persons.ChildrenArray.Length; i++)
            {
                var n1 = persons.ChildrenArray[i];
                var n2 = clone.ChildrenArray[i];

                Assert.AreNotSame(n1, n2);
            }
        }

        [Test]
        public void CloneAndCopyObjects()
        {
            var sut = this.SetupMapper();
            
            var person1 = this.SetupPerson();
            var clone = sut.Clone(person1);

            //Assert.AreNotEqual(clone, person1);

            //Assert.That(clone.Age == person1.Age);
            //Assert.That(clone.FirstName == person1.FirstName);
            //Assert.That(clone.LastName == person1.LastName);
            //Assert.That(clone.Children != null);
            //Assert.That(clone.Children.Count == person1.Children.Count);

            //Assert.That(clone.Father != null);
            //Assert.That(clone.Father.Children != null);
            //Assert.That(clone.Father.Children.Count == person1.Father.Children.Count);
        }

        [Test]
        public void MapObjectProperties()
        {
            var sut = this.SetupMapper();
            
            var person1 = new Person { Age = 18, FirstName = "Robb", LastName = "Stark" };
            var person2 = new Person { Age = 14, FirstName = "Sansa", LastName = "Stark" };

            sut.Map(person1, person2);

            Assert.AreEqual(person1.FirstName, person2.FirstName);
            Assert.AreEqual(person1.LastName, person2.LastName);
            Assert.AreEqual(person1.Age, person2.Age);
        }

        [Test]
        public void MapObjectPropertiesAndPreserveReferences()
        {
            var sut = this.SetupMapper();

            var person1 = this.SetupPerson();

            var person2 = new Person { Age = 0, FirstName = "No one" };

            sut.Map(person1, person2);

            Assert.AreEqual(person2.Father.Children[0], person2);

            person2.Father.Children[0].FirstName = "Bob";

            Assert.AreEqual(person2.FirstName, "Bob");
        }

        [Test]
        public void MapComplexPropertyObjects()
        {
            var sut = this.SetupMapper();

            var person1 = this.SetupPerson();

            var person2 = new Person { Age = 0, FirstName = "No one" };

            sut.Map(person1, person2);

            Assert.AreNotEqual(person1.Father, person2.Father);
            Assert.AreEqual(person1.Father.FirstName, person2.Father.FirstName);
            Assert.AreEqual(person1.Father.LastName, person2.Father.LastName);
            Assert.AreEqual(person1.Father.Age, person2.Father.Age);
        }

        [Test]
        public void MapComplexPropertyLists()
        {
            var sut = this.SetupMapper();

            var person1 = this.SetupPerson();

            var person2 = new Person { Age = 0, FirstName = "No one" };

            sut.Map(person1, person2);

            Assert.AreNotSame(person1.Children, person2.Children);
            Assert.AreEqual(person1.Children.Count, person2.Children.Count);
        }

        [Test]
        public void MapComplexPropertyListAndItsItems()
        {
            var sut = this.SetupMapper();

            var person1 = this.SetupPerson();

            var person2 = new Person { Age = 0, FirstName = "No one" };

            sut.Map(person1, person2);

            for (int i = 0; i < person1.Children.Count; i++)
            {
                var children1 = person1.Children[i];
                var children2 = person2.Children[i];

                Assert.AreNotSame(children1, children2);
                Assert.AreEqual(children1.FirstName, children2.FirstName);
                Assert.AreEqual(children1.LastName, children2.LastName);
                Assert.AreEqual(children1.Age, children2.Age);
            }
        }

        [Test]
        public void NotCloneNullObjects()
        {
            var sut = this.SetupMapper();
            Person person1 = null;

            var clone = sut.Clone(person1);

            Assert.IsNull(clone);
        }

        [Test]
        public void NotMapNullObjects()
        {
            var sut = this.SetupMapper();

            Person person1 = null;
            var person2 = new Person { Age = 18, FirstName = "Robb", LastName = "Stark" };

            sut.Map(person1, person2);

            Assert.AreEqual(person2.FirstName, "Robb");
            Assert.AreEqual(person2.LastName, "Stark");
        }

        [Test]
        public void NotMapCreateLists()
        {
            var sut = this.SetupMapper();

            var person1 = new Person { Age = 20, FirstName = "Jon", LastName = "Snow", Children = null };
            var person2 = new Person();

            sut.Map(person1, person2);

            Assert.IsNull(person2.Children);
        }

        [Test]
        public void NotMapFromNullLists()
        {
            var sut = this.SetupMapper();

            var person1 = new Person { Age = 20, FirstName = "Jon", LastName = "Snow", Children = null };
            var person2 = new Person { Age = 20, FirstName = "Jonny", LastName = "", Children = new List<Person> { } };

            sut.Map(person1, person2);

            Assert.IsNotNull(person2.Children);
        }

        [Test]
        public void RunPostMapActionAfterMapping()
        {
            var ranAction = false;

            var mapperManager = new MapperActionManager();
            mapperManager.AddMappingPostAction<Person, Person>((p1, p2) =>
            {
                ranAction = true;
            });

            var mapperOperator = new MapperOperator(mapperManager);
            var sut = new Mapper(mapperOperator);

            var person1 = this.SetupPerson();
            var person2 = new Person();

            sut.Map(person1, person2);

            Assert.IsTrue(ranAction);
        }

        [Test]
        public void RunPostMapActionAfterCloning()
        {
            var ranAction = false;

            var mapperManager = new MapperActionManager();
            mapperManager.AddMappingPostAction<Person, Person>((p1, p2) =>
            {
                ranAction = true;
            });

            var mapperOperator = new MapperOperator(mapperManager);
            var sut = new Mapper(mapperOperator);

            var person1 = this.SetupPerson();

            sut.Clone(person1);

            Assert.IsTrue(ranAction);
        }

        [Test]
        public void RunPostMapActionAfterCloningNestedLists()
        {
            var ranAction = false;

            var mapperManager = new MapperActionManager();
            mapperManager.AddMappingPostAction<List<Person>, List<Person>>((p1, p2) =>
            {
                ranAction = true;
            });

            var mapperOperator = new MapperOperator(mapperManager);
            var sut = new Mapper(mapperOperator);

            var person1 = this.SetupPerson();

            sut.Clone(person1);

            Assert.IsTrue(ranAction);
        }

        [Test]
        public void RunPostMapActionAfterCloningNestedObject()
        {
            var ranAction = false;

            var mapperManager = new MapperActionManager();
            mapperManager.AddMappingPostAction<Pet, Pet>((p1, p2) =>
            {
                ranAction = true;
            });

            var mapperOperator = new MapperOperator(mapperManager);
            var sut = new Mapper(mapperOperator);


            var pet = new Pet { Name = "Ghost" };
            var person = new Person
            {
                FirstName = "Jon",
                LastName = "Snow",
                Age = 19,
                Pet = pet
            };
            pet.Owner = person;

            sut.Clone(person);

            Assert.IsTrue(ranAction);
        }

        [Test]
        public void DoNotMapDictionaries()
        {
            var sut = this.SetupMapper();

            var dict = new Dictionary<string, string>();

            Assert.Throws<System.InvalidCastException>(() => sut.Clone(dict));
        }
    }
}
