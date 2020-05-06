using Onbox.Core.V6.ReactFactory;
using Onbox.Mvc.V6;
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

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ViewMvcBase
    {
        public string TextProp { get; set; }

        public Debouncer httpDebouncer => ReactFactory.Debouncer();
        public Interval interval;

        public MainWindow()
        {
            InitializeComponent();
        }

        public override void OnInit()
        {
            interval = ReactFactory.Interval(IntervalCalled, 500, 1000);
        }

        public override void OnDestroy()
        {
            interval.Stop();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            httpDebouncer.Debounce(HttpRequestMock);
        }

        private void IntervalCalled(int run)
        {
            Console.WriteLine("Interval called:" + run);
        }

        private void HttpRequestMock()
        {
            Console.WriteLine("Made Http Request for: " + TextProp);
        }
    }
}
