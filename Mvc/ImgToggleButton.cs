using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

namespace Onbox.Mvc.V1
{
    public class ImgToggleButton : ToggleButton
    {
        public BitmapSource Image
        {
            get { return (BitmapSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(BitmapSource), typeof(ImgToggleButton), new PropertyMetadata(null));


        static ImgToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImgToggleButton), new FrameworkPropertyMetadata(typeof(ImgToggleButton)));
        }
    }
}
