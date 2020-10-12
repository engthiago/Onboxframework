using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace Onbox.Mvc.VDev
{
    public class ColorPicker : Control
    {
        public string Color
        {
            get { return (string)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(string), typeof(ColorPicker), new FrameworkPropertyMetadata("#ffffff", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));

        public ColorPicker()
        {
            this.PreviewMouseDown += ColorPickerControl_PreviewMouseDown;
        }

        private void ColorPickerControl_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                System.Windows.Forms.ColorDialog colorPicker = new System.Windows.Forms.ColorDialog
                {
                    AllowFullOpen = true,
                    SolidColorOnly = true,
                    FullOpen = true,
                    Color = ColorTranslator.FromHtml(this.Color)
                };

                System.Windows.Forms.DialogResult result = colorPicker.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    if (colorPicker.Color != System.Drawing.Color.Empty)
                    {
                        this.Color = ColorTranslator.ToHtml(colorPicker.Color);
                    }
                }
            }
        }


        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
        }
    }
}