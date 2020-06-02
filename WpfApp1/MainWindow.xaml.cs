using Onbox.Core.V7;
using Onbox.Mvc.V7;
using Onbox.Store.V7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ViewMvcBase
    {
        Onbox.Di.V7.Container container;
        public Pet Pet { get; set; }

        private IStorageSubscription subs;
        private IStore<Person> store;

        public List<StateEntry<Person>> History { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            BuildContainer();

            var person = new Person
            {
                FirstName = "Thiago",
                LastName = "Almeida",
                Pet = new Pet
                {
                    Name = "Kitty"
                }
            };

            Pet = person.Pet;

            store = container.Resolve<IStore<Person>>();
            store.EnableLogging();
            store.SetState(person, new PersonInitAction());
        }

        public override void OnInit()
        {
            subs = store.Subscribe(new EditPetAction(), pet =>
            {
                Pet = pet;
            });
        }

        public override void OnDestroy()
        {
            subs.Unsubscribe();
        }

        private void OnOpenPet(object sender, RoutedEventArgs e)
        {
            var dialog = this.container.Resolve<Window1>();
            dialog.ShowDialog();
        }
        private void OnShowHistory(object sender, RoutedEventArgs e)
        {
            History = this.store.GetHistory();
        }






        private void BuildContainer()
        {
            container = Onbox.Di.V7.Container.Default();
            container.AddOnboxCore();
            container.AddSingleton<IStore<Person>, Store<Person>>();
        }


    }
}
