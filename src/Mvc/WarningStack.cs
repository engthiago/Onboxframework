using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Onbox.Mvc.VDev
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