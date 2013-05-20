using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Media.Animation;
using System.Threading;

namespace PopnTouchi2.ViewModel.Animation
{
    class SessionAnimation : Animation
    {
        /// <summary>
        /// 
        /// </summary>
        private SessionViewModel sessionVM;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public SessionAnimation(SessionViewModel s) 
            : base()
        {
            sessionVM = s;

            DoubleAnimation openingAnimation = new DoubleAnimation();
            ThicknessAnimation marginAnimation = new ThicknessAnimation();
            ExponentialEase ease = new ExponentialEase();
            ease.EasingMode = EasingMode.EaseOut;
            ease.Exponent = 2;

            openingAnimation.From = 0;
            openingAnimation.To = 1;
            openingAnimation.Duration = new Duration(TimeSpan.FromSeconds(.8));
            openingAnimation.FillBehavior = FillBehavior.HoldEnd;
            Storyboard.Children.Add(openingAnimation);
            Storyboard.SetTarget(openingAnimation, sessionVM.Grid);
            Storyboard.SetTargetProperty(openingAnimation, new PropertyPath(Grid.OpacityProperty));

            marginAnimation.From = new Thickness(50);
            marginAnimation.To = new Thickness(0);
            marginAnimation.Duration = new Duration(TimeSpan.FromSeconds(.8));
            marginAnimation.EasingFunction = ease;
            marginAnimation.FillBehavior = FillBehavior.Stop;
            Storyboard.Children.Add(marginAnimation);
            Storyboard.SetTarget(marginAnimation, sessionVM.Grid);
            Storyboard.SetTargetProperty(marginAnimation, new PropertyPath(Grid.MarginProperty));

            Storyboard.Begin();
        }

        //TODO Replace by gesture (Adrien's got an idea) AND organize
        void Reducer_Click(object sender, RoutedEventArgs e)
        {
            if (sessionVM.Reduced)
            {
                AudioController.FadeInBackgroundSound();
                sessionVM.Reducer.Content = "Reduce !";
                sessionVM.Reducer.Background = Brushes.Red;

                ScatterViewItem svi = (ScatterViewItem)sessionVM.Grid.Parent;
                Desktop mainDesktop = (Desktop)((ScatterView)svi.Parent).Parent;

                Storyboard stb = new Storyboard();
                PointAnimation centerPosAnimation = new PointAnimation();
                DoubleAnimation heightAnimation = new DoubleAnimation();
                DoubleAnimation widthAnimation = new DoubleAnimation();
                DoubleAnimation gridHeightAnimation = new DoubleAnimation();
                DoubleAnimation gridWidthAnimation = new DoubleAnimation();
                DoubleAnimation orientationAnimation = new DoubleAnimation();
                ThicknessAnimation borderAnimation = new ThicknessAnimation();

                centerPosAnimation.From = svi.ActualCenter;
                centerPosAnimation.To = new Point(mainDesktop.ActualWidth / 2, mainDesktop.ActualHeight / 2);
                centerPosAnimation.Duration = new Duration(TimeSpan.FromSeconds(.75));
                centerPosAnimation.FillBehavior = FillBehavior.HoldEnd;
                stb.Children.Add(centerPosAnimation);
                Storyboard.SetTarget(centerPosAnimation, svi);
                Storyboard.SetTargetProperty(centerPosAnimation, new PropertyPath(ScatterViewItem.CenterProperty));

                orientationAnimation.From = svi.ActualOrientation;
                if (svi.ActualOrientation <= 180)
                    orientationAnimation.To = 0;
                else
                    orientationAnimation.To = 360;
                orientationAnimation.Duration = new Duration(TimeSpan.FromSeconds(.75));
                orientationAnimation.FillBehavior = FillBehavior.HoldEnd;
                stb.Children.Add(orientationAnimation);
                Storyboard.SetTarget(orientationAnimation, svi);
                Storyboard.SetTargetProperty(orientationAnimation, new PropertyPath(ScatterViewItem.OrientationProperty));

                heightAnimation.From = svi.ActualHeight;
                heightAnimation.To = svi.ActualHeight * 4;
                heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                heightAnimation.EasingFunction = new ExponentialEase();
                heightAnimation.AccelerationRatio = 1;
                heightAnimation.FillBehavior = FillBehavior.HoldEnd;
                stb.Children.Add(heightAnimation);
                Storyboard.SetTarget(heightAnimation, svi);
                Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(ScatterViewItem.HeightProperty));

                widthAnimation.From = svi.ActualWidth;
                widthAnimation.To = svi.ActualWidth * 4;
                widthAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                widthAnimation.EasingFunction = new ExponentialEase();
                widthAnimation.AccelerationRatio = 1;
                widthAnimation.FillBehavior = FillBehavior.HoldEnd;
                stb.Children.Add(widthAnimation);
                Storyboard.SetTarget(widthAnimation, svi);
                Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(ScatterViewItem.WidthProperty));

                borderAnimation.From = new Thickness(15);
                borderAnimation.To = new Thickness(60);
                borderAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                borderAnimation.FillBehavior = FillBehavior.HoldEnd;
                stb.Children.Add(borderAnimation);
                Storyboard.SetTarget(borderAnimation, svi);
                Storyboard.SetTargetProperty(borderAnimation, new PropertyPath(ScatterViewItem.BorderThicknessProperty));

                gridHeightAnimation.From = sessionVM.Grid.ActualHeight;
                gridHeightAnimation.To = sessionVM.Grid.ActualHeight * 4;
                gridHeightAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                gridHeightAnimation.EasingFunction = new ExponentialEase();
                gridHeightAnimation.AccelerationRatio = 1;
                gridHeightAnimation.FillBehavior = FillBehavior.HoldEnd;
                stb.Children.Add(gridHeightAnimation);
                Storyboard.SetTarget(gridHeightAnimation, sessionVM.Grid);
                Storyboard.SetTargetProperty(gridHeightAnimation, new PropertyPath(Grid.HeightProperty));

                gridWidthAnimation.From = sessionVM.Grid.ActualWidth;
                gridWidthAnimation.To = sessionVM.Grid.ActualWidth * 4;
                gridWidthAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                gridWidthAnimation.EasingFunction = new ExponentialEase();
                gridWidthAnimation.AccelerationRatio = 1;
                gridWidthAnimation.FillBehavior = FillBehavior.HoldEnd;
                stb.Children.Add(gridWidthAnimation);
                Storyboard.SetTarget(gridWidthAnimation, sessionVM.Grid);
                Storyboard.SetTargetProperty(gridWidthAnimation, new PropertyPath(Grid.WidthProperty));

                widthAnimation.Completed += new EventHandler(stb_enlarge_Completed);

                Storyboard.Begin(sessionVM.Grid);


                (new NoteBubbleGeneratorViewModel(sessionVM.Session.NoteBubbleGenerator, sessionVM.Session)).Grid.Width = NoteBubbleGenerator.ActualWidth * 4;
                this.NoteBubbleGenerator.Height = NoteBubbleGenerator.ActualHeight * 4;
                this.MelodyBubbleGenerator.Width = MelodyBubbleGenerator.ActualWidth * 4;
                this.MelodyBubbleGenerator.Height = MelodyBubbleGenerator.ActualHeight * 4;

                sessionVM.Reduced = false;
            }
            else
            {
                AudioController.FadeOutBackgroundSound();
                stopAllAnimations();
                sessionVM.Reducer.Content = "Enlarge !";
                sessionVM.Reducer.Background = Brushes.Green;

                Storyboard stb = new Storyboard();
                DoubleAnimation heightAnimation = new DoubleAnimation();
                DoubleAnimation widthAnimation = new DoubleAnimation();

                ExponentialEase ease = new ExponentialEase();
                ease.EasingMode = EasingMode.EaseInOut;
                ease.Exponent = 1.5;

                heightAnimation.From = sessionVM.Grid.ActualHeight;
                heightAnimation.To = sessionVM.Grid.ActualHeight / 4;
                heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                heightAnimation.EasingFunction = ease;
                heightAnimation.AccelerationRatio = .4;
                heightAnimation.DecelerationRatio = .1;
                heightAnimation.FillBehavior = FillBehavior.Stop;
                stb.Children.Add(heightAnimation);
                Storyboard.SetTarget(heightAnimation, sessionVM.Grid);
                Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(Grid.HeightProperty));

                widthAnimation.From = sessionVM.Grid.ActualWidth;
                widthAnimation.To = sessionVM.Grid.ActualWidth / 4;
                widthAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                widthAnimation.EasingFunction = ease;
                widthAnimation.AccelerationRatio = .4;
                widthAnimation.DecelerationRatio = .1;
                widthAnimation.FillBehavior = FillBehavior.Stop;
                stb.Children.Add(widthAnimation);
                Storyboard.SetTarget(widthAnimation, sessionVM.Grid);
                Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Grid.WidthProperty));

                sessionVM.Grid.Height = sessionVM.Grid.ActualHeight / 4;
                sessionVM.Grid.Width = sessionVM.Grid.ActualWidth / 4;

                widthAnimation.Completed += new EventHandler(stb_Completed);

                Storyboard.Begin(sessionVM.Grid);

                this.NoteBubbleGenerator.Width = NoteBubbleGenerator.ActualWidth / 4;
                this.NoteBubbleGenerator.Height = NoteBubbleGenerator.ActualHeight / 4;
                this.MelodyBubbleGenerator.Width = MelodyBubbleGenerator.ActualWidth / 4;
                this.MelodyBubbleGenerator.Height = MelodyBubbleGenerator.ActualHeight / 4;
            }
        }
    }
}
