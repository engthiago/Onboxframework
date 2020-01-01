using System.Collections.Generic;

namespace Onbox.Sandbox.Revit.Commands
{
    public interface IOrderView
    {
        List<Inher.User> Users { get; set; }

        bool? ShowDialog();
    }
}