using System.Windows;
using System.Windows.Controls;

namespace Onbox.Mvc.V1
{
    /// <summary>
    /// Interaction logic for ErrorIcon.xaml
    /// </summary>
    public partial class ErrorIcon : UserControl
    {
        public event RoutedEventHandler OnRetry;

        public ErrorIcon()
        {
            InitializeComponent();
        }

        private void OnRetryClicked(object sender, RoutedEventArgs e)
        {
            OnRetry?.Invoke(sender, e);
        }
    }
}
