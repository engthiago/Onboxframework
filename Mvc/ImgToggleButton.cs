using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

namespace Onbox.Mvc.V2
{
    public class ImgToggleButton : ToggleButton
    {
        public BitmapSource Image
        {
            get { return (BitmapSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(BitmapSource), typeof(ImgToggleButton), 
                new PropertyMetadata(new BitmapImage(new Uri(@"pack://application:,,,/Onbox.Mvc.V2;component/Resources/tglButton/Add0.png"))));

        public ToggleImgButtonOptions Images
        {
            get { return (ToggleImgButtonOptions)GetValue(ImagesProperty); }
            set { SetValue(ImagesProperty, value); }
        }

        public static readonly DependencyProperty ImagesProperty =
            DependencyProperty.Register("Images", typeof(ToggleImgButtonOptions), typeof(ImgToggleButton), new PropertyMetadata(ToggleImgButtonOptions.Add, OnImageOptionsChanged));

        private static void OnImageOptionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImgToggleButton imgButton)
            {
                var name = e.NewValue.ToString();
                imgButton.Image = new BitmapImage(new Uri($@"pack://application:,,,/Onbox.Mvc.V2;component/Resources/tglButton/{name}0.png"));
            }
        }

        public bool HighlightBackgroundWhenChecked
        {
            get { return (bool)GetValue(HighlightBackgroundWhenCheckedProperty); }
            set { SetValue(HighlightBackgroundWhenCheckedProperty, value); }
        }

        public static readonly DependencyProperty HighlightBackgroundWhenCheckedProperty =
            DependencyProperty.Register("HighlightBackgroundWhenChecked", typeof(bool), typeof(ImgToggleButton), new PropertyMetadata(true));


        static ImgToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImgToggleButton), new FrameworkPropertyMetadata(typeof(ImgToggleButton)));
        }
    }

    public enum ToggleImgButtonOptions
    {
        Add,
        AddList,
        Arrow,
        Constraints,
        Confirm,
        PadLock,
        Pin,
        Undo
    }
}
