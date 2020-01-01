using System.Collections.ObjectModel;

namespace Onbox.Sandbox.Revit.Commands
{
    public interface IOrderViewModel
    {
        string Title { get; set; }
        ObservableCollection<Inher.User> Users { get; set; }

        bool CanCloseDialog();
        void OnDestroy();
        void OnInit();
        bool ShowDialog();
    }
}