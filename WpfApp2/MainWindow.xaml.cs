using Onbox.Core.V7.ReactFactory;
using Onbox.Mvc.V7;
using System;
using System.Windows.Controls;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ViewMvcBase
    {
        public string TextProp { get; set; }

        public Debouncer httpDebouncer = ReactFactory.Debouncer();

        public MainWindow()
        {
            InitializeComponent();
        }

        public override void OnInit()
        {
        }


        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            httpDebouncer.Debounce(HttpRequestMock, 500);
        }

        private void HttpRequestMock()
        {
            Console.WriteLine("Made Http Request for: " + TextProp);
        }
    }
}
