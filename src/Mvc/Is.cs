using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Onbox.Mvc.VDev
{
    public static class Is
    {
        /// CLOSE ///

        public static bool GetClose(DependencyObject obj)
        {
            return (bool)obj.GetValue(CloseProperty);
        }

        public static void SetClose(DependencyObject obj, bool value)
        {
            obj.SetValue(CloseProperty, value);
        }

        public static readonly DependencyProperty CloseProperty =
            DependencyProperty.RegisterAttached("Close", typeof(bool), typeof(Is), new PropertyMetadata(false, OnIsClosedChanged));

        private static void OnIsClosedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Button button)
            {
                button.Click -= OnCloseClicked;
                if ((bool)e.NewValue == true)
                {
                    button.Click += OnCloseClicked;
                }
            }
        }

        private static void OnCloseClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var window = VisualTreeHelpers.GetParent<Window>(button);
                if (window != null)
                {
                    try
                    {
                        window.Close();
                    }
                    catch
                    {
                    }
                }
            }
        }


        /// CONFIRM ///

        public static bool GetModalConfirm(DependencyObject obj)
        {
            return (bool)obj.GetValue(ModalConfirmProperty);
        }

        public static void SetModalConfirm(DependencyObject obj, bool value)
        {
            obj.SetValue(ModalConfirmProperty, value);
        }

        public static readonly DependencyProperty ModalConfirmProperty =
            DependencyProperty.RegisterAttached("ModalConfirm", typeof(bool), typeof(Is), new PropertyMetadata(false, OnIsModalConfirmChanged));

        private static void OnIsModalConfirmChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Button button)
            {
                button.Click -= OnModalConfirmClicked;
                if ((bool)e.NewValue == true)
                {
                    button.Click += OnModalConfirmClicked;
                }
            }
        }

        private static void OnModalConfirmClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var window = VisualTreeHelpers.GetParent<Window>(button);
                if (window != null)
                {
                    // Modal Windows can set DialogResult, Modeless will raise an exception, so we just close it in this case
                    try
                    {
                        window.DialogResult = true;
                    }
                    catch
                    {
                        try
                        {
                            window.Close();
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        /// CANCEL ///

        public static bool GetModalCancel(DependencyObject obj)
        {
            return (bool)obj.GetValue(ModalCancelProperty);
        }

        public static void SetModalCancel(DependencyObject obj, bool value)
        {
            obj.SetValue(ModalCancelProperty, value);
        }

        public static readonly DependencyProperty ModalCancelProperty =
            DependencyProperty.RegisterAttached("ModalCancel", typeof(bool), typeof(Is), new PropertyMetadata(false, OnIsModalCancelChanged));

        private static void OnIsModalCancelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Button button)
            {
                button.Click -= OnModalCancelClicked;
                if ((bool)e.NewValue == true)
                {
                    button.Click += OnModalCancelClicked;
                }
            }
        }

        private static void OnModalCancelClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var window = VisualTreeHelpers.GetParent<Window>(button);
                if (window != null)
                {
                    // Modal Windows can set DialogResult, Modeless will raise an exception, so we just close it in this case
                    try
                    {
                        window.DialogResult = false;
                    }
                    catch
                    {
                        try
                        {
                            window.Close();
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

    }
}