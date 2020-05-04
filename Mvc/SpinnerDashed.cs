using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Onbox.Mvc.V6
{
    public class SpinnerDashed : Spinner
    {
        static SpinnerDashed()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SpinnerDashed), new FrameworkPropertyMetadata(typeof(SpinnerDashed)));
        }
    }
}
