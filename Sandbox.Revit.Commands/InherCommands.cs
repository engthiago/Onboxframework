using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Mvvm.V1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Onbox.Sandbox.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Inher : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var container = new Di.V1.Container();
            container.Register<IOrderViewModel, NewOrderViewModel>();
            container.Register<ServerService>();

            var newOrderViewModel = container.Resolve<IOrderViewModel>();
            var result = newOrderViewModel.ShowDialog();

            System.Windows.MessageBox.Show(result.ToString(), "Result");

            return Result.Succeeded;
        }

        public class ViewWindowMockBase : ViewWindowBase, IViewWindow
        {
        }

        public class User
        {
            public string Name { get; set; }
            public string Role { get; set; }
        }

        public class ServerService
        {
            public List<User> GetUsers()
            {
                return new List<User>()
                {
                    new User { Name= "Thiago", Role = "Admin"},
                    new User { Name= "Raphel", Role = "Programmer" },
                    new User { Name= "Eduardo", Role = "Programmer" },
                    new User { Name= "Ramoon", Role = "Programmer" },
                };
            }
        }

        public class MockViewModel
        {
            public virtual bool CanCloseDialog()
            {
                return true;
            }

            public virtual void OnDestroy()
            {
            }

            public virtual void OnInit()
            {
            }

            public void ShowDialog()
            {
                OnInit();
                if (CanCloseDialog())
                {
                    OnDestroy();
                }
            }
        }

        public class NewOrderViewModel : ViewModelBase<OrderView>, IOrderViewModel
        {
            private readonly ServerService server;

            public string Title { get; set; }
            public ObservableCollection<User> Users { get; set; }


            public NewOrderViewModel(ServerService server)
            {
                this.Title = "New View Order";
                this.TitleVisibility = TitleVisibility.HideMinimizeAndMaximize;
                this.server = server;
            }

            public override void OnInit()
            {
                this.Users = new ObservableCollection<User>(this.server.GetUsers());
                System.Windows.MessageBox.Show("View Init...");
            }

            public override void OnDestroy()
            {
                System.Windows.MessageBox.Show("View Destroyed...");
            }

            public override bool CanCloseDialog()
            {
                if (this.View.DialogResult == true)
                {
                    return true;
                }

                var result = System.Windows.MessageBox.Show("Are you sure?", "Test", System.Windows.MessageBoxButton.YesNo);
                return result == System.Windows.MessageBoxResult.Yes;
            }
        }
    }
}
