using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Onbox.Mvc.V7.Animations
{
    public class SlideAnimations
    {
        public void AddSlideFromRight(Storyboard storyboard, double seconds, double offset, double decelerationRatio = 0.9)
        {
            AddSlideFrom(storyboard, seconds, offset, 0, decelerationRatio);
        }
        public void AddSlideFromLeft(Storyboard storyboard, double seconds, double offset, double decelerationRatio = 0.9)
        {
            AddSlideFrom(storyboard, seconds, -offset, 0, decelerationRatio);
        }

        public void AddSlideFromTop(Storyboard storyboard, double seconds, double offset, double decelerationRatio = 0.9)
        {
            AddSlideFrom(storyboard, seconds, 0, offset, decelerationRatio);
        }

        public void AddSlideFromBottom(Storyboard storyboard, double seconds, double offset, double decelerationRatio = 0.9)
        {
            AddSlideFrom(storyboard, seconds, 0, -offset, decelerationRatio);
        }

        public void AddSlideToRight(Storyboard storyboard, double seconds, double offset, double decelerationRatio = 0.9)
        {
            AddSlideTo(storyboard, seconds, offset, 0, decelerationRatio);
        }

        public void AddSlideToLeft(Storyboard storyboard, double seconds, double offset, double decelerationRatio = 0.9)
        {
            AddSlideTo(storyboard, seconds, -offset, 0, decelerationRatio);
        }

        public void AddSlideToTop(Storyboard storyboard, double seconds, double offset, double decelerationRatio = 0.9)
        {
            AddSlideFrom(storyboard, seconds, 0, offset, decelerationRatio);
        }
        public void AddSlideToBottom(Storyboard storyboard, double seconds, double offset, double decelerationRatio = 0.9)
        {
            AddSlideFrom(storyboard, seconds, 0, -offset, decelerationRatio);
        }


        private void AddSlideFrom(Storyboard storyboard, double seconds, double xOffset, double yOffset, double decelerationRatio = 0.9)
        {
            var animation = new ThicknessAnimation();
            animation.Duration = new Duration(TimeSpan.FromSeconds(seconds));
            animation.From = new Thickness(xOffset, yOffset, -xOffset, -yOffset);
            animation.To = new Thickness(0);
            animation.DecelerationRatio = decelerationRatio;

            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));
            storyboard.Children.Add(animation);
        }

        private void AddSlideTo(Storyboard storyboard, double seconds, double xOffset, double yOffset, double decelerationRatio = 0.9)
        {
            var animation = new ThicknessAnimation();
            animation.Duration = new Duration(TimeSpan.FromSeconds(seconds));
            animation.From = new Thickness(0);
            animation.To = new Thickness(xOffset, yOffset, -xOffset, -yOffset);
            animation.DecelerationRatio = decelerationRatio;

            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));
            storyboard.Children.Add(animation);
        }

    }
}
