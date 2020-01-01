using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Onbox.Di.V1;
using Onbox.Mvvm.V1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using static Onbox.Sandbox.Revit.Commands.Inher;

namespace Onbox.Sandbox.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Inher : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var container = new Container();
            container.Register<IOrderView, OrderView>();
            container.Register<IServerService, MockServerService>();
            container.Register<IMessageService, MessageBoxService>();

            container.AddJson(config =>
            {
                config.ContractResolver = new CamelCasePropertyNamesContractResolver();
                config.NullValueHandling = NullValueHandling.Ignore;
                config.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            var newOrderViewModel = container.Resolve<IOrderView>();
            var result = newOrderViewModel.ShowDialog();

            //System.Windows.MessageBox.Show(result.ToString(), "Result");

            return Result.Succeeded;
        }

        public class User
        {
            public string Name { get; set; }

            public string Role { get; set; }
        }

        public class JsonService : IJsonService
        {
            internal static JsonSerializerSettings settings;

            public JsonService()
            {
            }

            public T Deserialize<T>(string json)
            {
                return JsonConvert.DeserializeObject<T>(json, settings);
            }

            public string Serialize(object instance)
            {
                var json = JsonConvert.SerializeObject(instance, settings);
                return json;
            }
        }

        public interface IJsonService
        {
            T Deserialize<T>(string json);
            string Serialize(object instance);
        }

        public class MockServerService : IServerService
        {
            private static readonly string path = "C:/temp/Onbox/";
            private static readonly string userFile = path + "Users.json";
            private readonly IJsonService jsonService;

            public MockServerService(IJsonService jsonService)
            {
                this.jsonService = jsonService;
            }

            private List<User> GetUsers()
            {
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                var json = System.IO.File.ReadAllText(userFile);
                return jsonService.Deserialize<List<User>>(json);
            }

            public async Task<List<User>> GetUsersAsync()
            {
                await Task.Delay(1000);
                return this.GetUsers();
            }

            public async Task<List<User>> SaveUsersAsync(List<User> users)
            {
                var json = jsonService.Serialize(users);
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

    public static class JsonSettings
    {
        public static Container AddJson(this Container container, Action<JsonSerializerSettings> config)
        {
            container.Register<IJsonService, JsonService>();
            var settings = new JsonSerializerSettings();
            config.Invoke(settings);
            JsonService.settings = settings;
            return container;
        }
    }
}
