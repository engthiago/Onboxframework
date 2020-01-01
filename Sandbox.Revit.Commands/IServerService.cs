using System.Collections.Generic;
using System.Threading.Tasks;

namespace Onbox.Sandbox.Revit.Commands
{
    public interface IServerService
    {
        Task<List<Inher.User>> GetUsersAsync();
        Task<List<Inher.User>> SaveUsersAsync(List<Inher.User> users);
    }
}