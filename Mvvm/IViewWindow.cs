using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Onbox.Mvvm.V1
{
    public interface IViewWindow
    {
        TitleVisibility TitleVisibility { get; set; }
        object DataContext { get; set; }

        Func<bool> CanCloseDialog { get; set; }
        Action OnInit { get; set; }
        Action OnDestroy { get; set; }
        void Close();
        bool? ShowDialog();
        void SetOwner(object owner);
    }
}
