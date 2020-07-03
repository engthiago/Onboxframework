using Onbox.Mvc.V7.Animations;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Onbox.Mvc.V7
{
    public class NavigatorComponent : Control
    {
        private Frame controlA;
        private Frame controlB;

        private bool isControlACurrent;

        public INavigatorSubscription NavigatorSubscription { get; set; }

        public NavigatorComponent()
        {
            ClipToBounds = true;
            isControlACurrent = false;
        }

        public override void OnApplyTemplate()
        {
            controlA = this.GetTemplateChild("PART_ControlA") as Frame;
            controlB = this.GetTemplateChild("PART_ControlB") as Frame;
        }

        public object CurrentComponent
        {
            get { return (object)GetValue(CurrentComponentProperty); }
            set { SetValue(CurrentComponentProperty, value); }
        }

        public static readonly DependencyProperty CurrentComponentProperty =
            DependencyProperty.Register("CurrentComponent", typeof(object), typeof(NavigatorComponent), new UIPropertyMetadata(null, OnCurrentComponentChanged));

        private static void OnCurrentComponentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NavigatorComponent navComponent)
            {
                navComponent.ChangeContent(e.NewValue);
            }
        }

        private async void ChangeContent(object newContent)
        {
            if (!AreControlsValid())
            {
                return;
            }

            if (isControlACurrent)
            {
                controlB.Content = newContent;
                controlA.Content = null;
                isControlACurrent = false;
            }
            else
            {
                controlA.Content = newContent;
                controlB.Content = null;
                isControlACurrent = true;
            }

            //var slideAnimations = new SlideAnimations();

            //var slideOut = new Storyboard();
            //slideAnimations.AddSlideToLeft(slideOut, 1, this.ActualWidth);

            //var slideIn = new Storyboard();
            //slideAnimations.AddSlideFromRight(slideIn, 1, this.ActualWidth);

            //if (isControlACurrent)
            //{
            //    controlB.Content = newContent;
            //    slideOut.Begin(controlA);
            //    slideIn.Begin(controlB);
            //    isControlACurrent = false;
            //    await Task.Delay(1000);
            //}
            //else
            //{
            //    controlA.Content = newContent;
            //    slideOut.Begin(controlB);
            //    slideIn.Begin(controlA);
            //    isControlACurrent = true;
            //    await Task.Delay(1000);
            //}

        }

        private bool AreControlsValid()
        {
            return controlA != null && controlB != null;
        }

        static NavigatorComponent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigatorComponent), new FrameworkPropertyMetadata(typeof(NavigatorComponent)));
        }


    }
}
