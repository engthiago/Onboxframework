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

namespace Onbox.Mvc.VDev
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
            var component = VisualTreeHelpers.GetParentMvcComponent(this);
            if (component != null)
            {
                component.Message = null;
            }
        }
    }
}