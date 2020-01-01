using System.Collections.Generic;
using System.Threading.Tasks;

namespace Onbox.Sandbox.Revit.Commands
{
    public interface IServerService
    {
        Task<List<User>> GetUsersAsync();
        Task<List<User>> SaveUsersAsync(List<User> users);
    }
}