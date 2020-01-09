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
    /// Interaction logic for ImgButton.xaml
    /// </summary>
    public partial class ImgButton : UserControl
    {

        public ImgButtonOptions Images
        {
            get { return (ImgButtonOptions)GetValue(ImagesProperty); }
            set { SetValue(ImagesProperty, value); }
        }

        public static readonly DependencyProperty ImagesProperty =
            DependencyProperty.Register("Images", typeof(ImgButtonOptions), typeof(ImgButton), new PropertyMetadata(ImgButtonOptions.Add, OnImageTypeChanged));

        private static void OnImageTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImgButton imgButton)
            {
                var newType = e.NewValue.ToString();
                imgButton.Image = new BitmapImage(new Uri($@"pack://application:,,,/Onbox.Mvc.V1;component/Resources/imgButton/{newType}.png"));
            }
        }

        public BitmapSource Image
        {
            get { return (BitmapSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(BitmapSource), typeof(ImgButton), 
                new PropertyMetadata(new BitmapImage(new Uri($@"pack://application:,,,/Onbox.Mvc.V1;component/Resources/imgButton/Add.png"))));


        public ImgButton()
        {
            InitializeComponent();
        }
    }

    public enum ImgButtonOptions
    {
        Add,
        Add_To_Right,
        Apply,
        Arrow_Left_Green,
        Arrow_Right,Green,
        Clear,
        Close,
        Cloud_Down,
        Cloud_Up,
        Duplicate,
        Edit,
        Move_Down,
        Move_Up,
        New,
        Open,
        Remove_top_Left,
        Rename,
        Save,
        Search,
        Sort_Name_Down,
        Sort_Name_Up,
        Sync
    }
}
