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
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;

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
        private ScatterViewItem Svi { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private DesktopView MainDesktop { get; set; }

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
                SessionVM.Reducer.Content = "Reduce !";
                SessionVM.Reducer.Background = System.Windows.Media.Brushes.Red;
                
                Svi = (ScatterViewItem)SessionVM.Grid.Parent;
                MainDesktop = (DesktopView)((ScatterView)Svi.Parent).Parent;

                #region Animation Settings
                Storyboard stb = new Storyboard();
                PointAnimation centerPosAnimation = new PointAnimation();
                DoubleAnimation heightAnimation = new DoubleAnimation();
                DoubleAnimation widthAnimation = new DoubleAnimation();
                DoubleAnimation gridHeightAnimation = new DoubleAnimation();
                DoubleAnimation gridWidthAnimation = new DoubleAnimation();
                DoubleAnimation orientationAnimation = new DoubleAnimation();
                ThicknessAnimation borderAnimation = new ThicknessAnimation();

                centerPosAnimation.From = Svi.ActualCenter;
                centerPosAnimation.To = new System.Windows.Point(MainDesktop.ActualWidth / 2, MainDesktop.ActualHeight / 2);
                centerPosAnimation.Duration = new Duration(TimeSpan.FromSeconds(.75));
                centerPosAnimation.FillBehavior = FillBehavior.HoldEnd;
                stb.Children.Add(centerPosAnimation);
                Storyboard.SetTarget(centerPosAnimation, Svi);
                Storyboard.SetTargetProperty(centerPosAnimation, new PropertyPath(ScatterViewItem.CenterProperty));

                orientationAnimation.From = Svi.ActualOrientation;
                if (Svi.ActualOrientation <= 180)
                    orientationAnimation.To = 0;
                else
                    orientationAnimation.To = 360;
                orientationAnimation.Duration = new Duration(TimeSpan.FromSeconds(.75));
                orientationAnimation.FillBehavior = FillBehavior.HoldEnd;
                stb.Children.Add(orientationAnimation);
                Storyboard.SetTarget(orientationAnimation, Svi);
                Storyboard.SetTargetProperty(orientationAnimation, new PropertyPath(ScatterViewItem.OrientationProperty));

                heightAnimation.From = Svi.ActualHeight;
                heightAnimation.To = Svi.ActualHeight * 4;
                heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                heightAnimation.EasingFunction = new ExponentialEase();
                heightAnimation.AccelerationRatio = 1;
                heightAnimation.FillBehavior = FillBehavior.HoldEnd;
                stb.Children.Add(heightAnimation);
                Storyboard.SetTarget(heightAnimation, Svi);
                Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(ScatterViewItem.HeightProperty));

                widthAnimation.From = Svi.ActualWidth;
                widthAnimation.To = Svi.ActualWidth * 4;
                widthAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                widthAnimation.EasingFunction = new ExponentialEase();
                widthAnimation.AccelerationRatio = 1;
                widthAnimation.FillBehavior = FillBehavior.HoldEnd;
                stb.Children.Add(widthAnimation);
                Storyboard.SetTarget(widthAnimation, Svi);
                Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(ScatterViewItem.WidthProperty));

                borderAnimation.From = new Thickness(15);
                borderAnimation.To = new Thickness(60);
                borderAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                borderAnimation.FillBehavior = FillBehavior.HoldEnd;
                stb.Children.Add(borderAnimation);
                Storyboard.SetTarget(borderAnimation, Svi);
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
                MainDesktop = (DesktopView)SessionVM.Grid.Parent;
                SessionVM.SaveSession("test.bin");
                SessionVM.Session.StopBackgroundSound();

                MakeReadyForSnapShot(SessionVM.Grid);
                MemoryStream ms = new MemoryStream(Screenshot.GetSnapshot(SessionVM.Grid, .5, 100));
                System.Drawing.Image sc = System.Drawing.Image.FromStream(ms);
                FileStream fs = File.Create(@"./SnapShots/sc" + SessionVM.SessionID.ToString() + ".jpg");
                sc.Save(fs, ImageFormat.Jpeg);
                fs.Close();

                #region Sets the SnapShot as SVI content
                //ss for snapshot
                ImageBrush ss = new ImageBrush();
                ss.ImageSource = new BitmapImage(new Uri(@"./SnapShots/sc" + SessionVM.SessionID.ToString() + ".jpg", UriKind.Relative));

                ReplaceGridWithSnapShot(ss);
                #endregion

                //SessionVM.Grid.Children.Remove(SessionVM.Notes);
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
                #endregion

                widthAnimation.Completed += new EventHandler(stb_Completed);
                stb.Begin(SessionVM.Grid);

                SessionVM.NbgVM.Grid.Width = SessionVM.NbgVM.Grid.ActualWidth / 4;
                SessionVM.NbgVM.Grid.Height = SessionVM.NbgVM.Grid.ActualHeight / 4;
                SessionVM.MbgVM.Grid.Width = SessionVM.MbgVM.Grid.ActualWidth / 4;
                SessionVM.MbgVM.Grid.Height = SessionVM.MbgVM.Grid.ActualHeight / 4;
            }
        }

        private void ReplaceGridWithSnapShot(ImageBrush ss)
        {
            SessionVM.Grid.Children.Remove(SessionVM.Notes);
            SessionVM.Grid.Children.Remove(SessionVM.NbgVM.Grid);
            SessionVM.Grid.Children.Remove(SessionVM.MbgVM.Grid);

            System.Windows.Shapes.Rectangle rect = new System.Windows.Shapes.Rectangle();
            rect.Fill = Brushes.White;
            rect.Opacity = 1;
            SessionVM.Grid.Children.Add(rect);
            rect.Margin = new Thickness(0);

            Storyboard stb = new Storyboard();
            DoubleAnimation da = new DoubleAnimation();

            da.From = 1;
            da.To = 0;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(da);
            Storyboard.SetTarget(da, rect);
            Storyboard.SetTargetProperty(da, new PropertyPath(System.Windows.Shapes.Rectangle.OpacityProperty));

            stb.Begin();

            SessionVM.Grid.Background = ss;
        }

        private void MakeReadyForSnapShot(Grid grid)
        {
            grid.Children.Remove(SessionVM.Bubbles);
            grid.Children.Remove(SessionVM.Reducer);
            grid.Children.Remove(SessionVM.Play);
            grid.Children.Remove(SessionVM.Stop);
        }

        private void MakeReadyForDisplay(Grid grid)
        {
            grid.Children.Add(SessionVM.Bubbles);
            grid.Children.Add(SessionVM.Notes);
            grid.Children.Add(SessionVM.Reducer);
            grid.Children.Add(SessionVM.Play);
            grid.Children.Add(SessionVM.Stop);
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
            SessionVM.LoadSession("test.bin");
            SessionVM.Session.PlayBackgroundSound();

            MakeReadyForDisplay(SessionVM.Grid);
            Svi = (ScatterViewItem)SessionVM.Grid.Parent;
            MainDesktop = (DesktopView)((ScatterView)Svi.Parent).Parent;
            Svi.Content = null;

            ((ScatterView)Svi.Parent).Items.Remove(Svi);
            MainDesktop.Children.Add(SessionVM.Grid);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void stb_Completed(object sender, EventArgs e)
        {
            Svi = new ScatterViewItem();
            MainDesktop = (DesktopView)SessionVM.Grid.Parent;
            MainDesktop.Children.Remove(SessionVM.Grid);
            Svi.Width = SessionVM.Grid.Width;
            Svi.Height = SessionVM.Grid.Height;
            Svi.CanScale = false;
            Svi.Content = SessionVM.Grid;
            
            Svi.BorderBrush = System.Windows.Media.Brushes.White;
            Svi.Orientation = 0;
            Svi.Center = new System.Windows.Point(MainDesktop.ActualWidth / 2, MainDesktop.ActualHeight / 2);
            MainDesktop.Photos.Items.Add(Svi);


            Storyboard stb = new Storyboard();
            ThicknessAnimation borderAnimation = new ThicknessAnimation();
            DoubleAnimation heightAnimation = new DoubleAnimation();
            DoubleAnimation widthAnimation = new DoubleAnimation();

            borderAnimation.From = new Thickness(0);
            borderAnimation.To = new Thickness(15);
            borderAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            borderAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(borderAnimation);
            Storyboard.SetTarget(borderAnimation, Svi);
            Storyboard.SetTargetProperty(borderAnimation, new PropertyPath(ScatterViewItem.BorderThicknessProperty));

            heightAnimation.From = SessionVM.Grid.Height;
            heightAnimation.To = SessionVM.Grid.Height + 30;
            heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            heightAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(heightAnimation);
            Storyboard.SetTarget(heightAnimation, Svi);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(ScatterViewItem.HeightProperty));

            widthAnimation.From = SessionVM.Grid.Width;
            widthAnimation.To = SessionVM.Grid.Width + 30;
            widthAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            widthAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(widthAnimation);
            Storyboard.SetTarget(widthAnimation, Svi);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(ScatterViewItem.WidthProperty));

            Svi.BorderThickness = new Thickness(15);
            Svi.Width += 30;
            Svi.Height += 30;

            widthAnimation.Completed += new EventHandler(stb_border_Completed);

            stb.Begin(SessionVM.Grid);
            SessionVM.Reduced = true;
        }

        /// <summary>
        /// Event launched when ScatterViewItem's white borders are fully visible.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void stb_border_Completed(object sender, EventArgs e)
        {
            Storyboard = new Storyboard();
            PointAnimation centerPosAnimation = new PointAnimation();
            DoubleAnimation orientationAnimation = new DoubleAnimation();


            ExponentialEase ease = new ExponentialEase();
            ease.EasingMode = EasingMode.EaseOut;
            ease.Exponent = 1.5;

            Random r = new Random();
            System.Windows.Point newCenter = new System.Windows.Point(r.Next((int)MainDesktop.ActualWidth), r.Next((int)MainDesktop.ActualHeight));
            Double newOrientation = r.Next(-180, 180);

            centerPosAnimation.From = Svi.ActualCenter;
            centerPosAnimation.To = newCenter;
            centerPosAnimation.Duration = new Duration(TimeSpan.FromSeconds(5));
            centerPosAnimation.DecelerationRatio = .9;
            centerPosAnimation.EasingFunction = ease;
            centerPosAnimation.FillBehavior = FillBehavior.Stop;
            Storyboard.Children.Add(centerPosAnimation);
            Storyboard.SetTarget(centerPosAnimation, Svi);
            Storyboard.SetTargetProperty(centerPosAnimation, new PropertyPath(ScatterViewItem.CenterProperty));

            orientationAnimation.From = Svi.ActualOrientation;
            orientationAnimation.To = newOrientation;
            orientationAnimation.Duration = new Duration(TimeSpan.FromSeconds(5));
            orientationAnimation.DecelerationRatio = .9;
            orientationAnimation.EasingFunction = ease;
            orientationAnimation.FillBehavior = FillBehavior.Stop;
            Storyboard.Children.Add(orientationAnimation);
            Storyboard.SetTarget(orientationAnimation, Svi);
            Storyboard.SetTargetProperty(orientationAnimation, new PropertyPath(ScatterViewItem.OrientationProperty));

            Svi.Center = newCenter;
            Svi.Orientation = newOrientation;

            Storyboard.Begin();
            Svi.PreviewTouchDown += new EventHandler<System.Windows.Input.TouchEventArgs>(Session_TouchDown);
        }

        /// <summary>
        /// Stops the animation and set coordinates to current location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Session_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            Storyboard.Pause();

            Svi.Center = Svi.ActualCenter;
            Svi.Orientation = Svi.ActualOrientation;
            Storyboard.Remove();
        }
    }
}
