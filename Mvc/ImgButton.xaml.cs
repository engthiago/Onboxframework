using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Onbox.Mvc.V4
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
                imgButton.Image = new BitmapImage(new Uri($@"pack://application:,,,/Onbox.Mvc.V4;component/Resources/imgButton/{newType}.png"));
            }
        }

        public BitmapSource Image
        {
            get { return (BitmapSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(BitmapSource), typeof(ImgButton), 
                new PropertyMetadata(new BitmapImage(new Uri($@"pack://application:,,,/Onbox.Mvc.V4;component/Resources/imgButton/Add.png"))));


        public ImgButton()
        {
            InitializeComponent();
        }
    }

    public enum ImgButtonOptions
    {
        Add,
        Add_To_Right,
        Adsk,
        Apply,
        Arrow_Left_Green,
        Arrow_Right_Green,
        Clear,
        Close,
        Cloud,
        Cloud_Down,
        Cloud_Up,
        Copy,
        Cut,
        Delete,
        Duplicate,
        Edit,
        Element_Align,
        Element_Array,
        Element_Mirror,
        Element_Move,
        Element_Pin,
        Element_Rotate,
        Element_Scale,
        Element_Select,
        Find,
        Folder,
        Gear,
        Image,
        Info,
        Light,
        Location,
        Move_Down,
        Move_Up,
        New,
        Onbox,
        Open,
        Paste,
        Print,
        Question,
        Remove,
        Redo,
        Remove_To_Left,
        Rename,
        Revit,
        Revit_Link,
        Save,
        Save_As,
        Search,
        Settings,
        Snap,
        Sort_Name_Down,
        Sort_Name_Up,
        Sync,
        Undo,
        Warnings
    }
}
