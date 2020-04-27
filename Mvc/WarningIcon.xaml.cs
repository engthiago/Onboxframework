using System.Windows;
using System.Windows.Controls;

namespace Onbox.Mvc.V2
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
            var window = VisualTreeHelpers.GetParent<Window>(this);
            if (window is ViewMvcBase viewMvc)
            {
                viewMvc.OnWarningRetry();
            }
        }
    }
}
