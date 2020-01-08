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

namespace Onbox.Mvc.V1
{
    public class WarningStack : Control
    {
        private FrameworkElement clearErrorsControl;

        static WarningStack()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WarningStack), new FrameworkPropertyMetadata(typeof(WarningStack)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.clearErrorsControl != null)
            {
                this.clearErrorsControl.MouseLeftButtonDown -= this.OnClearErrors;
            }
            this.clearErrorsControl = this.GetTemplateChild("PART_BTNCLOSE") as FrameworkElement;
            if (this.clearErrorsControl != null)
            {
                this.clearErrorsControl.MouseLeftButtonDown += this.OnClearErrors;
            }
        }

        private void OnClearErrors(object sender, MouseButtonEventArgs e)
        {
            var window = VisualTreeHelpers.GetParent<Window>(this);
            if (window != null && window.DataContext is ViewMvcBase viewMvcBase)
            {
                viewMvcBase.Warning = null;
            }
        }
    }
}
