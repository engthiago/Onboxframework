using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Onbox.Mvc.V7
{
    public class VisualTreeHelpers
    {
        public static T GetVisualParent<T>(DependencyObject d) where T : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(d) as FrameworkElement;
            Type type = typeof(T);
            while (parent != null && parent.GetType() != type)
            {
                parent = VisualTreeHelper.GetParent(parent) as FrameworkElement;
            }

            return parent as T;
        }

        public static T GetParent<T>(DependencyObject d) where T : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(d) as FrameworkElement;
            Type targetType = typeof(T);
            while (parent != null)
            {
                Type type = parent.GetType();
                if (type == targetType || type.IsSubclassOf(targetType))
                {
                    break;
                }

                parent = VisualTreeHelper.GetParent(parent) as FrameworkElement;
            }

            return parent as T;
        }

        public static IMvcLifecycleComponent GetParentMvcComponent(DependencyObject d)
        {
            var parent = VisualTreeHelper.GetParent(d) as FrameworkElement;
            Type targetType = typeof(IMvcLifecycleComponent);
            while (parent != null)
            {
                Type type = parent.GetType();
                if (type.GetInterfaces().Contains(targetType))
                {
                    break;
                }

                parent = VisualTreeHelper.GetParent(parent) as FrameworkElement;
            }

            return parent as IMvcLifecycleComponent;
        }

        public static DependencyObject GetParent(string typeName, DependencyObject d)
        {
            var parent = VisualTreeHelper.GetParent(d);
            while (parent != null && parent.GetType().Name != typeName)
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent;
        }

        //public static T GetVisualChild<T>(DependencyObject d) where T : FrameworkElement
        //{
        //    var child = VisualTreeHelper.GetParent(d) as FrameworkElement;
        //    Type type = typeof(T);
        //    while (child != null && child.GetType() != type)
        //    {
        //        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(d); i++)
        //        {
        //            var subChild = VisualTreeHelper.GetChild(d, i);
        //            if (subChild.GetType() == type)
        //            {
        //                child = subChild as FrameworkElement;
        //                break;
        //            }
        //        }
        //    }

        //    return child as T;
        //}
    }
}
