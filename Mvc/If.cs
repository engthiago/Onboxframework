using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Onbox.Mvc.V7
{
    public static class If
    {
        public static bool GetOnErrorCollape(DependencyObject obj)
        {
            return (bool)obj.GetValue(OnErrorCollapeProperty);
        }

        public static void SetOnErrorCollape(DependencyObject obj, bool value)
        {
            obj.SetValue(OnErrorCollapeProperty, value);
        }

        public static readonly DependencyProperty OnErrorCollapeProperty =
            DependencyProperty.RegisterAttached("OnErrorCollape", typeof(bool), typeof(If), new PropertyMetadata(false, OnErrorCollapseChanged));

        private static void OnErrorCollapseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element && element.DataContext is ViewMvcBase viewMvc && (bool)e.NewValue == true)
            {
                Binding errorBinding = new Binding(nameof(viewMvc.Error));
                errorBinding.Source = viewMvc;
                errorBinding.Converter = new Converters.NullVisibilityCollapse();
                errorBinding.ConverterParameter = false;
                element.SetBinding(UIElement.VisibilityProperty, errorBinding);
            }
        }




        public static bool GetOnErrorHide(DependencyObject obj)
        {
            return (bool)obj.GetValue(OnErrorHideProperty);
        }

        public static void SetOnErrorHide(DependencyObject obj, bool value)
        {
            obj.SetValue(OnErrorHideProperty, value);
        }

        // Using a DependencyProperty as the backing store for OnErrorHide.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnErrorHideProperty =
            DependencyProperty.RegisterAttached("OnErrorHide", typeof(bool), typeof(If), new PropertyMetadata(false, OnErrorHideChanged));

        private static void OnErrorHideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element && element.DataContext is ViewMvcBase viewMvc && (bool)e.NewValue == true)
            {
                Binding errorBinding = new Binding(nameof(viewMvc.Error));
                errorBinding.Source = viewMvc;
                errorBinding.Converter = new Converters.NullVisibilityHide();
                errorBinding.ConverterParameter = false;
                element.SetBinding(UIElement.VisibilityProperty, errorBinding);
            }
        }




        public static bool GetOnErrorDisable(DependencyObject obj)
        {
            return (bool)obj.GetValue(OnErrorDisableProperty);
        }

        public static void SetOnErrorDisable(DependencyObject obj, bool value)
        {
            obj.SetValue(OnErrorDisableProperty, value);
        }

        // Using a DependencyProperty as the backing store for OnErrorDisable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnErrorDisableProperty =
            DependencyProperty.RegisterAttached("OnErrorDisable", typeof(bool), typeof(If), new PropertyMetadata(false, OnErroDisableChanged));

        private static void OnErroDisableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element && element.DataContext is ViewMvcBase viewMvc && (bool)e.NewValue == true)
            {
                Binding errorBinding = new Binding(nameof(viewMvc.Error));
                errorBinding.Source = viewMvc;
                errorBinding.Converter = new Converters.ValueIsNotNullConverter();
                errorBinding.ConverterParameter = false;
                element.SetBinding(UIElement.IsEnabledProperty, errorBinding);
            }
        }






        public static bool GetOnErrorOrLoadingCollapse(DependencyObject obj)
        {
            return (bool)obj.GetValue(OnErrorOrLoadingCollapseProperty);
        }

        public static void SetOnErrorOrLoadingCollapse(DependencyObject obj, bool value)
        {
            obj.SetValue(OnErrorOrLoadingCollapseProperty, value);
        }

        // Using a DependencyProperty as the backing store for OnErrorOrLoadingCollapse.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnErrorOrLoadingCollapseProperty =
            DependencyProperty.RegisterAttached("OnErrorOrLoadingCollapse", typeof(bool), typeof(If), new PropertyMetadata(false, OnErrorOrLoadingCollapseChanged));

        private static void OnErrorOrLoadingCollapseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element && element.DataContext is ViewMvcBase viewMvc && (bool)e.NewValue == true)
            {
                MultiBinding multiBinding = new MultiBinding();
                multiBinding.Bindings.Add(new Binding(nameof(viewMvc.Error)));
                multiBinding.Bindings.Add(new Binding(nameof(viewMvc.IsLoading)));
                multiBinding.Converter = new ErrorOrLoadingCollapseConverter();
                element.SetBinding(UIElement.VisibilityProperty, multiBinding);
            }
        }

        public class ErrorOrLoadingCollapseConverter : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                try
                {
                    var errorMessage = (string)values[0];
                    var loading = (bool)values[1];

                    if (errorMessage != null || loading == true)
                    {
                        return Visibility.Collapsed;
                    }

                }
                catch
                {
                }

                return Visibility.Visible;
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                return null;
            }
        }






        public static bool GetOnErrorOrLoadingHide(DependencyObject obj)
        {
            return (bool)obj.GetValue(OnErrorOrLoadingHideProperty);
        }

        public static void SetOnErrorOrLoadingHide(DependencyObject obj, bool value)
        {
            obj.SetValue(OnErrorOrLoadingHideProperty, value);
        }

        // Using a DependencyProperty as the backing store for OnErrorOrLoadingHide.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnErrorOrLoadingHideProperty =
            DependencyProperty.RegisterAttached("OnErrorOrLoadingHide", typeof(bool), typeof(If), new PropertyMetadata(false, OnErrorOrLoadingHideChanged));

        private static void OnErrorOrLoadingHideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element && element.DataContext is ViewMvcBase viewMvc && (bool)e.NewValue == true)
            {
                MultiBinding multiBinding = new MultiBinding();
                multiBinding.Bindings.Add(new Binding(nameof(viewMvc.Error)));
                multiBinding.Bindings.Add(new Binding(nameof(viewMvc.IsLoading)));
                multiBinding.Converter = new ErrorOrLoadingHideConverter();
                element.SetBinding(UIElement.VisibilityProperty, multiBinding);
            }
        }

        public class ErrorOrLoadingHideConverter : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                try
                {
                    var errorMessage = (string)values[0];
                    var loading = (bool)values[1];

                    if (errorMessage != null || loading == true)
                    {
                        return Visibility.Hidden;
                    }

                }
                catch
                {
                }

                return Visibility.Visible;
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                return null;
            }
        }






        public static bool GetOnErrorOrLoadingDisable(DependencyObject obj)
        {
            return (bool)obj.GetValue(OnErrorOrLoadingDisableProperty);
        }

        public static void SetOnErrorOrLoadingDisable(DependencyObject obj, bool value)
        {
            obj.SetValue(OnErrorOrLoadingDisableProperty, value);
        }

        // Using a DependencyProperty as the backing store for OnErrorOrLoadingDisable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnErrorOrLoadingDisableProperty =
            DependencyProperty.RegisterAttached("OnErrorOrLoadingDisable", typeof(bool), typeof(If), new PropertyMetadata(false, OnErrorOrLoadingDisableChanged));

        private static void OnErrorOrLoadingDisableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element && element.DataContext is ViewMvcBase viewMvc && (bool)e.NewValue == true)
            {
                MultiBinding multiBinding = new MultiBinding();
                multiBinding.Bindings.Add(new Binding(nameof(viewMvc.Error)));
                multiBinding.Bindings.Add(new Binding(nameof(viewMvc.IsLoading)));
                multiBinding.Converter = new ErrorOrLoadingDisableConverter();
                element.SetBinding(UIElement.IsEnabledProperty, multiBinding);
            }
        }

        public class ErrorOrLoadingDisableConverter : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                try
                {
                    var errorMessage = (string)values[0];
                    var loading = (bool)values[1];

                    if (errorMessage != null || loading == true)
                    {
                        return false;
                    }

                }
                catch
                {
                }

                return true;
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                return null;
            }
        }
    }
}
