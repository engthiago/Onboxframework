using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Onbox.Mvc.V7.CustomControls
{
    public class EmptyButton : Button
    {
        static EmptyButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EmptyButton), new FrameworkPropertyMetadata(typeof(EmptyButton)));
        }
    }
}
