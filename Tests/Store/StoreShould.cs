using NUnit.Framework;
using Onbox.Core.V6;
using Onbox.Mvc.V6;
using Onbox.Store.V6;
using System;
using System.Reflection;

namespace Tests.Store
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
        }

        public class Person : NotifyPropertyBase
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
            public string GetAction()
            {
                return null;
            }
        }

        public class ChildAction : IStoreAction<Person>
        {
            public string GetAction()
            {
                return "Child";
            }
        }

        public class PetAction : IStoreAction<Pet>
        {
            public string GetAction()
            {
                return "Pet";
            }
        }

        public class ChildChildAction : IStoreAction<Person>
        {
            public string GetAction()
            {
                return "Child.Child";
            }
        }

        [Test]
        public void SetItsState()
        {
            var container = Onbox.Di.V6.Container.Default();
            container.AddOnboxCore();

            container.AddSingleton<Store<Person>>();

            var store = container.Resolve<Store<Person>>();

            var subs = store.Subscribe(new ChildChildAction(), (person) =>
            {
                Console.WriteLine("Updated person: Child child");
            });

            var subs2 = store.Subscribe(new ChildAction(), (person) =>
            {
                Console.WriteLine("Updated person: Child");
            });

            var subs3 = store.Subscribe(new PetAction(), (pet) =>
            {
                Console.WriteLine("Updated person: Child");
            });


            store.SetState(new Person { FirstName = "Thiago", LastName = "Almeida" }, new PersonAction());
            store.SetState(new Person { FirstName = "Arnold", LastName = "Almeida" }, new ChildAction());
            store.SetState(new Person { FirstName = "Bruno", LastName = "Almeida" }, new ChildChildAction());
            store.SetState(new Cat { Name = "Caetano" }, new PetAction());


            Assert.That(store.State.Child.Child.FirstName == "Bruno");

            subs.Unsubscribe();
            subs2.Unsubscribe();
            subs3.Unsubscribe();
        }

        private void Store_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine(e.PropertyName);
        }
    }
}
