using System;
using System.Threading.Tasks;

namespace Onbox.Mvc.V1
{
    public interface IViewBase
    {
        void RunInitFunc(Func<Task> func, Action<string> error = null);
        void SetOwner(object owner);
        void SetTitle(string title);
        bool? ShowDialog();
    }
}