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
    /// Interaction logic for SpinnerDashed.xaml
    /// </summary>
    public partial class SpinnerDashed : UserControl
    {


        public Brush Color1
        {
            get { return (Brush)GetValue(Color1Property); }
            set { SetValue(Color1Property, value); }
        }

        // Using a DependencyProperty as the backing store for Color1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Color1Property =
            DependencyProperty.Register("Color1", typeof(Brush), typeof(SpinnerDashed), new PropertyMetadata(new SolidColorBrush(Colors.Black)));




        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SpinnerDashed), new PropertyMetadata("Loading..."));

        public SpinnerDashed()
        {
            InitializeComponent();
        }
    }
}
