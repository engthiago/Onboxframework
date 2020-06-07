using System.Windows;
using System.Windows.Controls;

namespace Onbox.Mvc.V7
{
    /// <summary>
    /// Interaction logic for WarningIcon.xaml
    /// </summary>
    public partial class WarningIcon : UserControl
    {
        public WarningIcon()
        {
            InitializeComponent();
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
