using System;
using System.Threading.Tasks;

namespace Onbox.Mvc.V5
{
    public interface IViewMvc
    {
        void RunOnInitFunc(Func<Task> func, Action<string> error = null, Action complete = null);
        void SetOwner(object owner);
        void SetTitle(string title);
        void SetTitleVisibility(TitleVisibility titleVisibility);
        bool? ShowDialog();
    }
}