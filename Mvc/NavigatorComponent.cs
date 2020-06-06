using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Onbox.Mvc.V7
{
    public class NavigatorComponent : Frame
    {
        public INavigatorSubscription NavigatorSubscription { get; set; }

        public NavigatorComponent()
        {
            this.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }
    }
}
