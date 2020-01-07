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
    /// <summary>
    /// Interaction logic for ErrorIcon.xaml
    /// </summary>
    public partial class ErrorIcon : UserControl
    {
        public event RoutedEventHandler OnRetry;
        public bool CanRetry
        {
            get { return (bool)GetValue(CanRetryProperty); }
            set { SetValue(CanRetryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanRetry.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanRetryProperty =
            DependencyProperty.Register("CanRetry", typeof(bool), typeof(ErrorIcon), new PropertyMetadata(false));


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
