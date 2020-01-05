using Onbox.Mvc.V1;
using System.Collections.Generic;

namespace Onbox.Sandbox.Revit.Commands
{
    public interface IOrderView : IViewMvc
    {
        List<User> Users { get; set; }
    }
}