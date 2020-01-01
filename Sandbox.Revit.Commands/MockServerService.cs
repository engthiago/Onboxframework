using System.Collections.Generic;
using System.Threading.Tasks;

namespace Onbox.Sandbox.Revit.Commands
{
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
            return this.jsonService.Deserialize<List<User>>(json);
        }

        public async Task<List<User>> GetUsersAsync()
        {
            await Task.Delay(1000);
            return this.GetUsers();
        }

        public async Task<List<User>> SaveUsersAsync(List<User> users)
        {
            var json = this.jsonService.Serialize(users);
            System.IO.File.WriteAllText(userFile, json);
            await Task.Delay(1000);
            return this.GetUsers();
        }
    }
}
