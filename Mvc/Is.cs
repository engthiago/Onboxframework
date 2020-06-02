using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Onbox.Mvc.V7
{
    public static class Is
    {
        public static bool GetConfirm(DependencyObject obj)
        {
            return (bool)obj.GetValue(ConfirmProperty);
        }

        public static void SetConfirm(DependencyObject obj, bool value)
        {
            obj.SetValue(ConfirmProperty, value);
        }

        public static readonly DependencyProperty ConfirmProperty =
            DependencyProperty.RegisterAttached("Confirm", typeof(bool), typeof(Is), new PropertyMetadata(false, OnIsConfirmChanged));

        private static void OnIsConfirmChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Button button && (bool)e.NewValue == true)
            {
                button.Click += OnConfirmClicked;
            }
        }

        private static void OnConfirmClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var window = VisualTreeHelpers.GetParent<Window>(button);
                if (window != null)
                {
                    window.DialogResult = true;
                }
            }
        }
    }
}
