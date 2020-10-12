using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Onbox.Mvc.VDev
{
    /// <summary>
    /// Interaction logic for SearchControl.xaml
    /// </summary>
    public partial class SearchControl : UserControl
    {
        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof(string), typeof(SearchControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public SearchControl()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                PART_OverlayText.Opacity = 100;
                PART_SearchIcon.Opacity = 100;
                PART_ClearButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                PART_OverlayText.Opacity = 0;
                PART_SearchIcon.Opacity = 0;
                PART_ClearButton.Visibility = Visibility.Visible;
            }
        }

        private void PART_ClearButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SearchText = string.Empty;
        }

        private void SearchControlMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                SearchText = string.Empty;
            }
        }

        private void SearchControlMain_GotFocus(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate
            {
                Keyboard.Focus(SearchBox);
                FocusManager.SetFocusedElement(this, SearchBox);
                FocusManager.SetIsFocusScope(SearchBox, true);
                SearchBox.Focus();
            }));
        }
    }
}