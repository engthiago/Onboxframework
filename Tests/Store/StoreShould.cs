using NUnit.Framework;
using Onbox.Core.V7;
using Onbox.Mvc.V7;
using Onbox.Store.V7;
using System;
using System.Reflection;

namespace Store
{
    [TestFixture]
    public class StoreShould
    {
        public class Pet
        {
            public string Name { get; set; }
        }

        public class Cat : Pet
        {
            public string Breed { get; set; }
        }

        public class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public Pet Pet { get; set; }

            public Person Child { get; set; }
        }

        public StoreShould()
        {
        }

        public class PersonAction : IStoreAction<Person>
        {
            public string GetActionName()
            {
                return "Edit Person";
            }

            public string GetActionPath()
            {
                return null;
            }
        }

        public class ChildAction : IStoreAction<Person>
        {
            public string GetActionName()
            {
                return "Edit Child";
            }

            public string GetActionPath()
            {
                return "Child";
            }
        }

        public class PetAction : IStoreAction<Pet>
        {
            public string GetActionName()
            {
                return "Edit Pet";
            }

            public string GetActionPath()
            {
                return "Pet";
            }
        }

        public class ChildChildAction : IStoreAction<Person>
        {
            public string GetActionName()
            {
                return "Edit Grandchild";
            }

            public string GetActionPath()
            {
                return "Child.Child";
            }
        }

        [Test]
        public void SetItsState()
        {
            var container = Onbox.Di.V7.Container.Default();
            container.AddOnboxCore();

            container.AddSingleton<Store<Person>>();

            var store = container.Resolve<Store<Person>>();


            var subs3 = store.Subscribe(new PetAction(), (pet) =>
            {
                Console.WriteLine("Updated cat");
            });

            //store.PropertyChanged += this.Store_PropertyChanged;

            store.SetState(new Person { FirstName = "Thiago", LastName = "Almeida" }, new PersonAction());
            store.SetState(new Person { FirstName = "Arnold", LastName = "Almeida" }, new ChildAction());
            store.SetState(new Person { FirstName = "Bruno", LastName = "Almeida" }, new ChildChildAction());
            store.SetState(new Cat { Name = "Caetano", Breed = "AA" }, new PetAction());


            //Assert.That(store.State.Child.Child.FirstName == "Bruno");


            subs3.Unsubscribe();
        }

        private void Store_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine("State changed...");
        }
    }
}
