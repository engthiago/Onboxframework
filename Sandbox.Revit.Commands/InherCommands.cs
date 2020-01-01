using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json;
using Onbox.Mvvm.V1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Onbox.Sandbox.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Inher : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var container = new Di.V1.Container();
            container.Register<IOrderView, OrderView>();
            container.Register<IServerService, MockServerService>();
            container.Register<IMessageService, MessageBoxService>();

            var newOrderViewModel = container.Resolve<IOrderView>();
            var result = newOrderViewModel.ShowDialog();

            //System.Windows.MessageBox.Show(result.ToString(), "Result");

            return Result.Succeeded;
        }

        public class User
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("role")]
            public string Role { get; set; }
        }

        public class MockServerService : IServerService
        {
            private static readonly string path = "C:/temp/Onbox/";
            private static readonly string userFile = path + "Users.json";

            private List<User> GetUsers()
            {
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                var json = System.IO.File.ReadAllText(userFile);
                return JsonConvert.DeserializeObject<List<User>>(json);
            }

            public async Task<List<User>> GetUsersAsync()
            {
                await Task.Delay(1000);
                return this.GetUsers();
            }

            public async Task<List<User>> SaveUsersAsync(List<User> users)
            {
                var json = JsonConvert.SerializeObject(users);
                System.IO.File.WriteAllText(userFile, json);
                await Task.Delay(1000);
                return this.GetUsers();
            }
        }

        public class MessageBoxService : IMessageService
        {
            public static string title = "";

            public void Show(string message)
            {
                System.Windows.MessageBox.Show(message);
            }

            public void Warning(string message)
            {
                System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }

            public void Error(string message)
            {
                System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }

            public bool Question(string message)
            {
                return System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes;
            }

            public void SetTitle(string newTitle)
            {
                title = newTitle;
            }
            
        }
    }
}
