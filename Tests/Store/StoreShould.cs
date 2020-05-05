using NUnit.Framework;
using Onbox.Core.V6;
using Onbox.Mvc.V6;
using Onbox.Store.V6;
using System;

namespace Tests.Store
{
    [TestFixture]
    public class StoreShould
    {
        public class Person : NotifyPropertyBase
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public Person Child { get; set; }
        }

        public StoreShould()
        {
        }

        [Test]
        public void SetItsState()
        {
            var container = Onbox.Di.V6.Container.Default();
            container.AddOnboxCore();

            container.AddSingleton<Store<Person>>();

            var store = container.Resolve<Store<Person>>();

            var subs = store.Subscribe<Person>("Child.Child", (person) =>
            {
               Console.WriteLine("Updated person: Child child");
            });

            var subs2 = store.Subscribe<Person>("Child", (person) =>
            {
                Console.WriteLine("Updated person: Child");
            });


            store.SetState(new Person { FirstName = "Thiago", LastName = "Almeida" }, null);
            store.SetState(new Person { FirstName = "Arnold", LastName = "Almeida" }, "Child");
            store.SetState(new Person { FirstName = "Bruno", LastName = "Almeida" }, "Child.Child");


            Assert.That(store.State.Child.Child.FirstName == "Bruno");

            subs.Unsubscribe();
            subs2.Unsubscribe();
        }

        private void Store_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine(e.PropertyName);
        }
    }
}
