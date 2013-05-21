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
    public class SessionAnimation : Animation
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

            Storyboard stb = new Storyboard();
            DoubleAnimation openingAnimation = new DoubleAnimation();
            ThicknessAnimation marginAnimation = new ThicknessAnimation();
            ExponentialEase ease = new ExponentialEase();
            ease.EasingMode = EasingMode.EaseOut;
            ease.Exponent = 2;

            openingAnimation.From = 0;
            openingAnimation.To = 1;
            openingAnimation.Duration = new Duration(TimeSpan.FromSeconds(.8));
            openingAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(openingAnimation);
            Storyboard.SetTarget(openingAnimation, sessionVM.Grid);
            Storyboard.SetTargetProperty(openingAnimation, new PropertyPath(Grid.OpacityProperty));

            marginAnimation.From = new Thickness(50);
            marginAnimation.To = new Thickness(0);
            marginAnimation.Duration = new Duration(TimeSpan.FromSeconds(.8));
            marginAnimation.EasingFunction = ease;
            marginAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(marginAnimation);
            Storyboard.SetTarget(marginAnimation, sessionVM.Grid);
            Storyboard.SetTargetProperty(marginAnimation, new PropertyPath(Grid.MarginProperty));

            stb.Begin();
        }

        
        /// <summary>
        /// TODO Replace by gesture (Adrien's got an idea) AND organize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Reducer_Click(object sender, RoutedEventArgs e)
        {
            if (sessionVM.Reduced)
            {
                AudioController.FadeInBackgroundSound();
                sessionVM.Reducer.Content = "Reduce !";
                sessionVM.Reducer.Background = Brushes.Red;

                ScatterViewItem svi = (ScatterViewItem)sessionVM.Grid.Parent;
                DesktopViewModel mainDesktop = new DesktopViewModel();
                mainDesktop.Grid = (Grid)((ScatterView)svi.Parent).Parent;

                Storyboard stb = new Storyboard();
                PointAnimation centerPosAnimation = new PointAnimation();
                DoubleAnimation heightAnimation = new DoubleAnimation();
                DoubleAnimation widthAnimation = new DoubleAnimation();
                DoubleAnimation gridHeightAnimation = new DoubleAnimation();
                DoubleAnimation gridWidthAnimation = new DoubleAnimation();
                DoubleAnimation orientationAnimation = new DoubleAnimation();
                ThicknessAnimation borderAnimation = new ThicknessAnimation();

                centerPosAnimation.From = svi.ActualCenter;
                centerPosAnimation.To = new Point(mainDesktop.Grid.ActualWidth / 2, mainDesktop.Grid.ActualHeight / 2);
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


                sessionVM.NbgVM.Grid.Width = sessionVM.NbgVM.Grid.ActualWidth * 4;
                sessionVM.NbgVM.Grid.Height = sessionVM.NbgVM.Grid.ActualHeight * 4;
                sessionVM.MbgVM.Grid.Width = sessionVM.MbgVM.Grid.ActualWidth * 4;
                sessionVM.MbgVM.Grid.Height = sessionVM.MbgVM.Grid.ActualHeight * 4;

                sessionVM.Reduced = false;
            }
            else
            {
                AudioController.FadeOutBackgroundSound();
                stopAllBubblesAnimations();
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

                stb.Begin(sessionVM.Grid);

                sessionVM.NbgVM.Grid.Width = sessionVM.NbgVM.Grid.ActualWidth / 4;
                sessionVM.NbgVM.Grid.Height = sessionVM.NbgVM.Grid.ActualHeight / 4;
                sessionVM.MbgVM.Grid.Width = sessionVM.MbgVM.Grid.ActualWidth / 4;
                sessionVM.MbgVM.Grid.Height = sessionVM.MbgVM.Grid.ActualHeight / 4;
            }
        }

        /// <summary>
        /// Stops all bubbles animations
        /// </summary>
        private void stopAllBubblesAnimations()
        {
            foreach (NoteBubbleViewModel nb in sessionVM.Bubbles.Items)
                nb.Animation.StopAnimation();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void stb_enlarge_Completed(object sender, EventArgs e)
        {
            ScatterViewItem svi = (ScatterViewItem)sessionVM.Grid.Parent;
            DesktopViewModel mainDesktop = new DesktopViewModel();
            mainDesktop.Grid = (Grid)((ScatterView)svi.Parent).Parent;
            svi.Content = null;

            ((ScatterView)svi.Parent).Items.Remove(svi);
            mainDesktop.Grid.Children.Add(sessionVM.Grid);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void stb_Completed(object sender, EventArgs e)
        {
            ScatterViewItem svi = new ScatterViewItem();
            DesktopViewModel mainDesktop = new DesktopViewModel();
            mainDesktop.Grid = (Grid)sessionVM.Grid.Parent;
            mainDesktop.Grid.Children.Remove(sessionVM.Grid);
            svi.Width = sessionVM.Grid.Width;
            svi.Height = sessionVM.Grid.Height;
            svi.CanScale = false;
            svi.Content = this;
            svi.BorderBrush = Brushes.White;
            svi.Orientation = 0;
            svi.Center = new Point(mainDesktop.Grid.ActualWidth / 2, mainDesktop.Grid.ActualHeight / 2);
            mainDesktop.Photos.Items.Add(svi);


            Storyboard stb = new Storyboard();
            ThicknessAnimation borderAnimation = new ThicknessAnimation();
            DoubleAnimation heightAnimation = new DoubleAnimation();
            DoubleAnimation widthAnimation = new DoubleAnimation();

            borderAnimation.From = new Thickness(0);
            borderAnimation.To = new Thickness(15);
            borderAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            borderAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(borderAnimation);
            Storyboard.SetTarget(borderAnimation, svi);
            Storyboard.SetTargetProperty(borderAnimation, new PropertyPath(ScatterViewItem.BorderThicknessProperty));

            heightAnimation.From = sessionVM.Grid.Height;
            heightAnimation.To = sessionVM.Grid.Height + 30;
            heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            heightAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(heightAnimation);
            Storyboard.SetTarget(heightAnimation, svi);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(ScatterViewItem.HeightProperty));

            widthAnimation.From = sessionVM.Grid.Width;
            widthAnimation.To = sessionVM.Grid.Width + 30;
            widthAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            widthAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(widthAnimation);
            Storyboard.SetTarget(widthAnimation, svi);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(ScatterViewItem.WidthProperty));

            svi.BorderThickness = new Thickness(15);
            svi.Width += 30;
            svi.Height += 30;

            widthAnimation.Completed += new EventHandler(stb_border_Completed);

            stb.Begin(sessionVM.Grid);
            sessionVM.Reduced = true;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void stb_border_Completed(object sender, EventArgs e)
        {
            ScatterViewItem svi = (ScatterViewItem)sessionVM.Grid.Parent;
            DesktopViewModel mainDesktop = new DesktopViewModel();
            mainDesktop.Grid = (Grid)((ScatterView)svi.Parent).Parent;

            Storyboard = new Storyboard();
            PointAnimation centerPosAnimation = new PointAnimation();
            DoubleAnimation orientationAnimation = new DoubleAnimation();


            ExponentialEase ease = new ExponentialEase();
            ease.EasingMode = EasingMode.EaseOut;
            ease.Exponent = 1.5;

            Random r = new Random();
            Point newCenter = new Point(r.Next((int)mainDesktop.Grid.ActualWidth), r.Next((int)mainDesktop.Grid.ActualHeight));
            Double newOrientation = r.Next(-180, 180);

            centerPosAnimation.From = svi.ActualCenter;
            centerPosAnimation.To = newCenter;
            centerPosAnimation.Duration = new Duration(TimeSpan.FromSeconds(5));
            centerPosAnimation.DecelerationRatio = .9;
            centerPosAnimation.EasingFunction = ease;
            centerPosAnimation.FillBehavior = FillBehavior.Stop;
            Storyboard.Children.Add(centerPosAnimation);
            Storyboard.SetTarget(centerPosAnimation, svi);
            Storyboard.SetTargetProperty(centerPosAnimation, new PropertyPath(ScatterViewItem.CenterProperty));

            orientationAnimation.From = svi.ActualOrientation;
            orientationAnimation.To = newOrientation;
            orientationAnimation.Duration = new Duration(TimeSpan.FromSeconds(5));
            orientationAnimation.DecelerationRatio = .9;
            orientationAnimation.EasingFunction = ease;
            orientationAnimation.FillBehavior = FillBehavior.Stop;
            Storyboard.Children.Add(orientationAnimation);
            Storyboard.SetTarget(orientationAnimation, svi);
            Storyboard.SetTargetProperty(orientationAnimation, new PropertyPath(ScatterViewItem.OrientationProperty));

            svi.Center = newCenter;
            svi.Orientation = newOrientation;

            Storyboard.Begin();
            svi.PreviewTouchDown += new EventHandler<System.Windows.Input.TouchEventArgs>(Session_TouchDown);
        }

        /// <summary>
        /// Stops the animation and set coordinates to current location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Session_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            Storyboard.Pause();
            ScatterViewItem svi = (ScatterViewItem)sessionVM.Grid.Parent;
            DesktopViewModel mainDesktop = new DesktopViewModel();
            mainDesktop.Grid = (Grid)((ScatterView)svi.Parent).Parent;

            svi.Center = svi.ActualCenter;
            svi.Orientation = svi.ActualOrientation;
            Storyboard.Remove();
        }
    }
}
