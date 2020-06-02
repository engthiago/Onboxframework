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

namespace Onbox.Mvc.V7
{
    public class Spinner : Control
    {


        public Color Color1
        {
            get { return (Color)GetValue(Color1Property); }
            set { SetValue(Color1Property, value); }
        }

        public static readonly DependencyProperty Color1Property =
            DependencyProperty.Register("Color1", typeof(Color), typeof(Spinner), new UIPropertyMetadata(Color.FromRgb(140,140,140)));


        public Color Color2
        {
            get { return (Color)GetValue(Color2Property); }
            set { SetValue(Color2Property, value); }
        }

        public static readonly DependencyProperty Color2Property =
            DependencyProperty.Register("Color2", typeof(Color), typeof(Spinner), new UIPropertyMetadata(Color.FromRgb(84, 84, 84)));




        public Color Color3
        {
            get { return (Color)GetValue(Color3Property); }
            set { SetValue(Color3Property, value); }
        }

        // Using a DependencyProperty as the backing store for Color3.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Color3Property =
            DependencyProperty.Register("Color3", typeof(Color), typeof(Spinner), new PropertyMetadata(Colors.Transparent));



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(Spinner), new PropertyMetadata("Loading..."));





        static Spinner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Spinner), new FrameworkPropertyMetadata(typeof(Spinner)));
        }
    }
}
