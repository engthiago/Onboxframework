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
    /// Interaction logic for Spinner.xaml
    /// </summary>
    public partial class Spinner : UserControl
    {
        public Color Color1
        {
            get { return (Color)GetValue(Color1Property); }
            set { SetValue(Color1Property, value); }
        }

        public static readonly DependencyProperty Color1Property =
            DependencyProperty.Register("Color1", typeof(Color), typeof(Spinner), new PropertyMetadata(Color.FromRgb(150, 150, 150)));


        public Color Color2
        {
            get { return (Color)GetValue(Color2Property); }
            set { SetValue(Color2Property, value); }
        }

        // Using a DependencyProperty as the backing store for EndColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Color2Property =
            DependencyProperty.Register("Color2", typeof(Color), typeof(Spinner), new PropertyMetadata(Color.FromRgb(48,48,48)));





        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(Spinner), new PropertyMetadata("Loading..."));

        public Spinner()
        {
            InitializeComponent();
        }
    }
}
