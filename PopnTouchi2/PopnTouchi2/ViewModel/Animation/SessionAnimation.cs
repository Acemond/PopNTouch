﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Media.Animation;
using System.Threading;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace PopnTouchi2.ViewModel.Animation
{
    /// <summary>
    /// Defines all animations used for the Session.
    /// </summary>
    public class SessionAnimation : Animation
    {
        /// <summary>
        /// 
        /// </summary>
        public SessionViewModel SessionVM { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public SessionAnimation(SessionViewModel s) 
            : base()
        {
            SessionVM = s;

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
            Storyboard.SetTarget(openingAnimation, SessionVM.Grid);
            Storyboard.SetTargetProperty(openingAnimation, new PropertyPath(Grid.OpacityProperty));

            marginAnimation.From = new Thickness(50);
            marginAnimation.To = new Thickness(0);
            marginAnimation.Duration = new Duration(TimeSpan.FromSeconds(.8));
            marginAnimation.EasingFunction = ease;
            marginAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(marginAnimation);
            Storyboard.SetTarget(marginAnimation, SessionVM.Grid);
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
            if (SessionVM.Reduced)
            {
                SessionVM.LoadSession("test.bin");
                SessionVM.Session.StopBackgroundSound();

                SessionVM.Reducer.Content = "Reduce !";
                SessionVM.Reducer.Background = System.Windows.Media.Brushes.Red;
                
                ScatterViewItem svi = (ScatterViewItem)SessionVM.Grid.Parent;
                DesktopView mainDesktop = (DesktopView)((ScatterView)svi.Parent).Parent;

                #region Animation Settings
                Storyboard stb = new Storyboard();
                PointAnimation centerPosAnimation = new PointAnimation();
                DoubleAnimation heightAnimation = new DoubleAnimation();
                DoubleAnimation widthAnimation = new DoubleAnimation();
                DoubleAnimation gridHeightAnimation = new DoubleAnimation();
                DoubleAnimation gridWidthAnimation = new DoubleAnimation();
                DoubleAnimation orientationAnimation = new DoubleAnimation();
                ThicknessAnimation borderAnimation = new ThicknessAnimation();

                centerPosAnimation.From = svi.ActualCenter;
                centerPosAnimation.To = new System.Windows.Point(mainDesktop.ActualWidth / 2, mainDesktop.ActualHeight / 2);
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

                gridHeightAnimation.From = SessionVM.Grid.ActualHeight;
                gridHeightAnimation.To = SessionVM.Grid.ActualHeight * 4;
                gridHeightAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                gridHeightAnimation.EasingFunction = new ExponentialEase();
                gridHeightAnimation.AccelerationRatio = 1;
                gridHeightAnimation.FillBehavior = FillBehavior.HoldEnd;
                stb.Children.Add(gridHeightAnimation);
                Storyboard.SetTarget(gridHeightAnimation, SessionVM.Grid);
                Storyboard.SetTargetProperty(gridHeightAnimation, new PropertyPath(Grid.HeightProperty));

                gridWidthAnimation.From = SessionVM.Grid.ActualWidth;
                gridWidthAnimation.To = SessionVM.Grid.ActualWidth * 4;
                gridWidthAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                gridWidthAnimation.EasingFunction = new ExponentialEase();
                gridWidthAnimation.AccelerationRatio = 1;
                gridWidthAnimation.FillBehavior = FillBehavior.HoldEnd;
                stb.Children.Add(gridWidthAnimation);
                Storyboard.SetTarget(gridWidthAnimation, SessionVM.Grid);
                Storyboard.SetTargetProperty(gridWidthAnimation, new PropertyPath(Grid.WidthProperty));

                widthAnimation.Completed += new EventHandler(stb_enlarge_Completed);
                #endregion

                stb.Begin(SessionVM.Grid);

                SessionVM.NbgVM.Grid.Width = SessionVM.NbgVM.Grid.ActualWidth * 4;
                SessionVM.NbgVM.Grid.Height = SessionVM.NbgVM.Grid.ActualHeight * 4;
                SessionVM.MbgVM.Grid.Width = SessionVM.MbgVM.Grid.ActualWidth * 4;
                SessionVM.MbgVM.Grid.Height = SessionVM.MbgVM.Grid.ActualHeight * 4;

                SessionVM.Reduced = false;
            }
            else
            {
                SessionVM.SaveSession("test.bin");

                SessionVM.Grid.Children.Remove(SessionVM.Bubbles);
                ScatterViewItem svii = new ScatterViewItem();
                MemoryStream ms = new MemoryStream(Screenshot.GetSnapshot(SessionVM.Grid, .5, 100));
                System.Drawing.Image sc = System.Drawing.Image.FromStream(ms);
                sc.Save("sc.jpg");
                //Graphics.DrawImage(sc);

                SessionVM.Session.StopBackgroundSound();

                stopAllBubblesAnimations();
                SessionVM.Reducer.Content = "Enlarge !";
                SessionVM.Reducer.Background = System.Windows.Media.Brushes.Green;

                #region Animation Settings
                Storyboard stb = new Storyboard();
                DoubleAnimation heightAnimation = new DoubleAnimation();
                DoubleAnimation widthAnimation = new DoubleAnimation();

                ExponentialEase ease = new ExponentialEase();
                ease.EasingMode = EasingMode.EaseInOut;
                ease.Exponent = 1.5;

                heightAnimation.From = SessionVM.Grid.ActualHeight;
                heightAnimation.To = SessionVM.Grid.ActualHeight / 4;
                heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                heightAnimation.EasingFunction = ease;
                heightAnimation.AccelerationRatio = .4;
                heightAnimation.DecelerationRatio = .1;
                heightAnimation.FillBehavior = FillBehavior.Stop;
                stb.Children.Add(heightAnimation);
                Storyboard.SetTarget(heightAnimation, SessionVM.Grid);
                Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(Grid.HeightProperty));

                widthAnimation.From = SessionVM.Grid.ActualWidth;
                widthAnimation.To = SessionVM.Grid.ActualWidth / 4;
                widthAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                widthAnimation.EasingFunction = ease;
                widthAnimation.AccelerationRatio = .4;
                widthAnimation.DecelerationRatio = .1;
                widthAnimation.FillBehavior = FillBehavior.Stop;
                stb.Children.Add(widthAnimation);
                Storyboard.SetTarget(widthAnimation, SessionVM.Grid);
                Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Grid.WidthProperty));

                SessionVM.Grid.Height = SessionVM.Grid.ActualHeight / 4;
                SessionVM.Grid.Width = SessionVM.Grid.ActualWidth / 4;

                widthAnimation.Completed += new EventHandler(stb_Completed);
                #endregion

                stb.Begin(SessionVM.Grid);

                SessionVM.NbgVM.Grid.Width = SessionVM.NbgVM.Grid.ActualWidth / 4;
                SessionVM.NbgVM.Grid.Height = SessionVM.NbgVM.Grid.ActualHeight / 4;
                SessionVM.MbgVM.Grid.Width = SessionVM.MbgVM.Grid.ActualWidth / 4;
                SessionVM.MbgVM.Grid.Height = SessionVM.MbgVM.Grid.ActualHeight / 4;
            }
        }

        /// <summary>
        /// Stops all bubbles animations
        /// </summary>
        public void stopAllBubblesAnimations()
        {
            foreach (NoteBubbleViewModel nb in SessionVM.NbgVM.NoteBubbleVMs)
                nb.Animation.StopAnimation();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void stb_enlarge_Completed(object sender, EventArgs e)
        {
            ScatterViewItem svi = (ScatterViewItem)SessionVM.Grid.Parent;
            DesktopView mainDesktop = (DesktopView)((ScatterView)svi.Parent).Parent;
            svi.Content = null;

            ((ScatterView)svi.Parent).Items.Remove(svi);
            mainDesktop.Children.Add(SessionVM.Grid);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void stb_Completed(object sender, EventArgs e)
        {
            ScatterViewItem svi = new ScatterViewItem();
            DesktopView mainDesktop = (DesktopView)SessionVM.Grid.Parent;
            mainDesktop.Children.Remove(SessionVM.Grid);
            svi.Width = SessionVM.Grid.Width;
            svi.Height = SessionVM.Grid.Height;
            svi.CanScale = false;
            svi.Content = SessionVM.Grid;
            svi.BorderBrush = System.Windows.Media.Brushes.White;
            svi.Orientation = 0;
            svi.Center = new System.Windows.Point(mainDesktop.ActualWidth / 2, mainDesktop.ActualHeight / 2);
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

            heightAnimation.From = SessionVM.Grid.Height;
            heightAnimation.To = SessionVM.Grid.Height + 30;
            heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            heightAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(heightAnimation);
            Storyboard.SetTarget(heightAnimation, svi);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(ScatterViewItem.HeightProperty));

            widthAnimation.From = SessionVM.Grid.Width;
            widthAnimation.To = SessionVM.Grid.Width + 30;
            widthAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            widthAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(widthAnimation);
            Storyboard.SetTarget(widthAnimation, svi);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(ScatterViewItem.WidthProperty));

            svi.BorderThickness = new Thickness(15);
            svi.Width += 30;
            svi.Height += 30;

            widthAnimation.Completed += new EventHandler(stb_border_Completed);

            stb.Begin(SessionVM.Grid);
            SessionVM.Reduced = true;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void stb_border_Completed(object sender, EventArgs e)
        {
            ScatterViewItem svi = (ScatterViewItem)SessionVM.Grid.Parent;
            DesktopView mainDesktop = (DesktopView)((ScatterView)svi.Parent).Parent;

            Storyboard = new Storyboard();
            PointAnimation centerPosAnimation = new PointAnimation();
            DoubleAnimation orientationAnimation = new DoubleAnimation();


            ExponentialEase ease = new ExponentialEase();
            ease.EasingMode = EasingMode.EaseOut;
            ease.Exponent = 1.5;

            Random r = new Random();
            System.Windows.Point newCenter = new System.Windows.Point(r.Next((int)mainDesktop.ActualWidth), r.Next((int)mainDesktop.ActualHeight));
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
        public void Session_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            Storyboard.Pause();
            ScatterViewItem svi = (ScatterViewItem)SessionVM.Grid.Parent;
            DesktopView mainDesktop = (DesktopView)((ScatterView)svi.Parent).Parent;

            svi.Center = svi.ActualCenter;
            svi.Orientation = svi.ActualOrientation;
            Storyboard.Remove();
        }
    }
}
