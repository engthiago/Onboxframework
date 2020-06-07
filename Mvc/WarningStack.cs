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

namespace Onbox.Mvc.V7
{
    public class WarningStack : Control
    {
        private FrameworkElement clearWarningControl;

        static WarningStack()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WarningStack), new FrameworkPropertyMetadata(typeof(WarningStack)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.clearWarningControl != null)
            {
                this.clearWarningControl.MouseLeftButtonDown -= this.OnClearError;
            }
            this.clearWarningControl = this.GetTemplateChild("PART_BTNCLOSE") as FrameworkElement;
            if (this.clearWarningControl != null)
            {
                this.clearWarningControl.MouseLeftButtonDown += this.OnClearError;
            }
        }

        private void OnClearError(object sender, MouseButtonEventArgs e)
        {
            var component = VisualTreeHelpers.GetParentMvcComponent(this);
            if (component != null)
            {
                component.Warning = null;
            }
        }
    }
}
