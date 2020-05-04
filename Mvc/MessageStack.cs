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

namespace Onbox.Mvc.V6
{
    public class MessageStack : Control
    {
        private FrameworkElement clearMessageControl;

        static MessageStack()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageStack), new FrameworkPropertyMetadata(typeof(MessageStack)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.clearMessageControl != null)
            {
                this.clearMessageControl.MouseLeftButtonDown -= this.OnClearMessage;
            }
            this.clearMessageControl = this.GetTemplateChild("PART_BTNCLOSE") as FrameworkElement;
            if (this.clearMessageControl != null)
            {
                this.clearMessageControl.MouseLeftButtonDown += this.OnClearMessage;
            }
        }

        private void OnClearMessage(object sender, MouseButtonEventArgs e)
        {
            var window = VisualTreeHelpers.GetParent<Window>(this);
            if (window != null && window.DataContext is ViewMvcBase viewMvcBase)
            {
                viewMvcBase.Message = null;
            }
        }
    }
}
