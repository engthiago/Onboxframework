using Onbox.Mvc.V6;
using Onbox.Store.V6;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : ViewMvcBase
    {
        private readonly IStore<Person> store;

        public Pet Pet { get; set; }

        public Window1(IStore<Person> store)
        {
            InitializeComponent();
            this.store = store;

            this.Pet = this.store.Select(new SelectPetAction());
        }

        private void OnEdit(object sender, RoutedEventArgs e)
        {
            this.store.SetState(Pet, new EditPetAction());
        }

        private void OnAdd(object sender, RoutedEventArgs e)
        {
            this.store.SetState(Pet, new CreatePetAction());
        }
    }
}
