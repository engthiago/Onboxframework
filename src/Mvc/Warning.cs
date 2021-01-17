using System.Windows;
using System.Windows.Controls;

namespace Onbox.Mvc.VDev
{
    public class Warning : Control
    {
        Button button;


        public bool CanRetry
        {
            get { return (bool)GetValue(CanRetryProperty); }
            set { SetValue(CanRetryProperty, value); }
        }

        public static readonly DependencyProperty CanRetryProperty =
            DependencyProperty.Register("CanRetry", typeof(bool), typeof(Control), new PropertyMetadata(false));


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
