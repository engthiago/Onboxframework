using System.Windows;
using System.Windows.Controls;

namespace Onbox.Mvc.V7
{
    /// <summary>
    /// Interaction logic for ErrorIcon.xaml
    /// </summary>
    public partial class ErrorIcon : UserControl
    {
        public ErrorIcon()
        {
            InitializeComponent();
        }

        private void OnRetryClicked(object sender, RoutedEventArgs e)
        {
            var window = VisualTreeHelpers.GetParent<Window>(this);
            if (window is ViewMvcBase viewMvc)
            {
                viewMvc.OnErrorRetry();
            }
        }
    }
}
