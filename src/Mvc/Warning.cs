using System.Windows;
using System.Windows.Controls;

namespace Onbox.Mvc.VDev
{
    public class Warning : Control
    {
        Button button;

        static Warning()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Warning), new FrameworkPropertyMetadata(typeof(Warning)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.button != null)
            {
                this.button.Click -= OnRetryClicked;
            }

            this.button = this.GetTemplateChild("PART_Retry") as Button;
            if (this.button != null)
            {
                this.button.Click += OnRetryClicked;
            }
        }

        private void OnRetryClicked(object sender, RoutedEventArgs e)
        {
            var component = VisualTreeHelpers.GetParentMvcComponent(this);
            if (component != null)
            {
                component.OnWarningRetry();
            }
        }
    }
}
