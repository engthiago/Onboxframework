using System.Windows;
using System.Windows.Controls;

namespace Onbox.Mvc.V1
{
    /// <summary>
    /// Interaction logic for WarningIcon.xaml
    /// </summary>
    public partial class WarningIcon : UserControl
    {
        public event RoutedEventHandler OnRetry;

        public WarningIcon()
        {
            InitializeComponent();
        }

        private void OnRetryClicked(object sender, RoutedEventArgs e)
        {
            OnRetry?.Invoke(sender, e);
        }
    }
}
