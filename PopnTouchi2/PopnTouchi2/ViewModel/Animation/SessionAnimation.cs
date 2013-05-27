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
using System.Runtime.Serialization.Formatters.Binary;
using PopnTouchi2.Model;

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
        private DesktopView MainDesktop { get; set; }

        /// <summary>
        /// FileStream used for SnapShots.
        /// </summary>
        private FileStream Fs { get; set; }

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
            Storyboard.SetTarget(openingAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(openingAnimation, new PropertyPath(ScatterViewItem.OpacityProperty));

            marginAnimation.From = new Thickness(50);
            marginAnimation.To = new Thickness(0);
            marginAnimation.Duration = new Duration(TimeSpan.FromSeconds(.8));
            marginAnimation.EasingFunction = ease;
            marginAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(marginAnimation);
            Storyboard.SetTarget(marginAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(marginAnimation, new PropertyPath(ScatterViewItem.MarginProperty));

            marginAnimation.Completed += new EventHandler(marginAnimation_Completed);

            stb.Begin();
        }

        #region REDUCTION
        /// <summary>
        /// TODO Replace by gesture (Adrien's got an idea) AND organize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Reducer_Click(object sender, RoutedEventArgs e)
        {
            if (SessionVM.Reduced) return;

            MainDesktop = (DesktopView)((ScatterView)(SessionVM.SessionSVI.Parent)).Parent;
            SessionVM.SaveSession("test.bin");
            SessionVM.Session.StopBackgroundSound();

            MakeReadyForSnapShot(SessionVM.Grid);
            MemoryStream ms;
            if (SessionVM.Orientation == "left" || SessionVM.Orientation == "right") ms = new MemoryStream(Screenshot.GetSideSnapshot(SessionVM.Grid, .5, 100));
            else ms = new MemoryStream(Screenshot.GetSnapshot(SessionVM.Grid, .5, 100));
            System.Drawing.Image sc = System.Drawing.Image.FromStream(ms);

            string path = @"./SnapShots/sc" + SessionVM.SessionID.ToString() + ".jpg";

            if (File.Exists(path)) File.Delete(path);
            Fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            sc.Save(Fs, ImageFormat.Jpeg);
            Fs.Close();

            #region Sets the SnapShot as SVI content
            //ss for snapshot
            ImageBrush ss = new ImageBrush();
            BitmapImage bi = new BitmapImage();
            Fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            bi.BeginInit();
            bi.StreamSource = Fs;
            bi.EndInit();
            ss.ImageSource = bi;

            ReplaceGridWithSnapShot(ss);
            #endregion

            SessionVM.Session.StopBackgroundSound();
            stopAllBubblesAnimations();
        }

        private void ReplaceGridWithSnapShot(ImageBrush ss)
        {
            SessionVM.Grid.Children.Remove(SessionVM.Notes);
            SessionVM.Grid.Children.Remove(SessionVM.NbgVM.Grid);
            SessionVM.Grid.Children.Remove(SessionVM.MbgVM.Grid);
            SessionVM.Grid.Children.Remove(SessionVM.TreeUp.Grid);
            SessionVM.Grid.Children.Remove(SessionVM.TreeDown.Grid);

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

            da.Completed += new EventHandler(flash_Completed);
            stb.Begin();

            RotateTransform rt = new RotateTransform();
            rt.CenterX = 0.5;
            rt.CenterY = 0.5;
            switch (SessionVM.Orientation)
            {
                case "top": rt.Angle = 180; break;
                case "left": rt.Angle = -90; break;
                case "right": rt.Angle = 90; break;
                default: rt.Angle = 0; break;
            }
            ss.RelativeTransform = rt;
            SessionVM.Grid.Background = ss;
        }

        void flash_Completed(object sender, EventArgs e)
        {

            #region Animation Settings
            Storyboard stb = new Storyboard();
            DoubleAnimation heightAnimation = new DoubleAnimation();
            DoubleAnimation widthAnimation = new DoubleAnimation();

            ExponentialEase ease = new ExponentialEase();
            ease.EasingMode = EasingMode.EaseInOut;
            ease.Exponent = 1.5;

            heightAnimation.From = SessionVM.SessionSVI.ActualHeight;
            if (SessionVM.Orientation == "left" || SessionVM.Orientation == "right")
                heightAnimation.To = (SessionVM.SessionSVI.ActualHeight / 4) / 0.5625;
            else heightAnimation.To = SessionVM.SessionSVI.ActualHeight / 4;
            heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            heightAnimation.EasingFunction = ease;
            heightAnimation.AccelerationRatio = .4;
            heightAnimation.DecelerationRatio = .1;
            heightAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(heightAnimation);
            Storyboard.SetTarget(heightAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(ScatterViewItem.HeightProperty));

            widthAnimation.From = SessionVM.SessionSVI.ActualWidth;
            if (SessionVM.Orientation == "left" || SessionVM.Orientation == "right")
                widthAnimation.To = (SessionVM.SessionSVI.ActualWidth / 4) / 0.5625;
            else widthAnimation.To = SessionVM.SessionSVI.ActualWidth / 4;
            widthAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            widthAnimation.EasingFunction = ease;
            widthAnimation.AccelerationRatio = .4;
            widthAnimation.DecelerationRatio = .1;
            widthAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(widthAnimation);
            Storyboard.SetTarget(widthAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(ScatterViewItem.WidthProperty));

            if (SessionVM.Orientation == "left" || SessionVM.Orientation == "right")
            {
                SessionVM.SessionSVI.Height = SessionVM.SessionSVI.ActualHeight / 4 / 0.5625;
                SessionVM.SessionSVI.Width = SessionVM.SessionSVI.ActualWidth / 4 / 0.5625;
            }
            else
            {
                SessionVM.SessionSVI.Height = SessionVM.SessionSVI.ActualHeight / 4;
                SessionVM.SessionSVI.Width = SessionVM.SessionSVI.ActualWidth / 4;
            }
            #endregion

            widthAnimation.Completed += new EventHandler(stb_Completed);
            stb.Begin(SessionVM.SessionSVI);
        }

        private void MakeReadyForSnapShot(Grid grid)
        {
            grid.Children.Remove(SessionVM.Bubbles);
            grid.Children.Remove(SessionVM.Reducer);
            grid.Children.Remove(SessionVM.Play);
            grid.Children.Remove(SessionVM.Stop);
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
        public void stb_Completed(object sender, EventArgs e)
        {
            MainDesktop = (DesktopView)((ScatterView)(SessionVM.SessionSVI.Parent)).Parent;
            SessionVM.SessionSVI.TouchLeave += new EventHandler<System.Windows.Input.TouchEventArgs>(svi_TouchLeave);

            SessionVM.SessionSVI.BorderBrush = System.Windows.Media.Brushes.White;


            Storyboard stb = new Storyboard();
            ThicknessAnimation borderAnimation = new ThicknessAnimation();
            DoubleAnimation heightAnimation = new DoubleAnimation();
            DoubleAnimation widthAnimation = new DoubleAnimation();

            borderAnimation.From = new Thickness(0);
            borderAnimation.To = new Thickness(15);
            borderAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            borderAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(borderAnimation);
            Storyboard.SetTarget(borderAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(borderAnimation, new PropertyPath(ScatterViewItem.BorderThicknessProperty));

            heightAnimation.From = SessionVM.SessionSVI.ActualHeight;
            heightAnimation.To = SessionVM.SessionSVI.ActualHeight + 30;
            heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            heightAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(heightAnimation);
            Storyboard.SetTarget(heightAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(ScatterViewItem.HeightProperty));

            widthAnimation.From = SessionVM.SessionSVI.ActualWidth;
            widthAnimation.To = SessionVM.SessionSVI.ActualWidth + 30;
            widthAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            widthAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(widthAnimation);
            Storyboard.SetTarget(widthAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(ScatterViewItem.WidthProperty));

            SessionVM.SessionSVI.BorderThickness = new Thickness(15);
            SessionVM.SessionSVI.Width += 30;
            SessionVM.SessionSVI.Height += 30;

            widthAnimation.Completed += new EventHandler(stb_border_Completed);

            stb.Begin(SessionVM.SessionSVI);
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

            centerPosAnimation.From = SessionVM.SessionSVI.ActualCenter;
            centerPosAnimation.To = newCenter;
            centerPosAnimation.Duration = new Duration(TimeSpan.FromSeconds(5));
            centerPosAnimation.DecelerationRatio = .9;
            centerPosAnimation.EasingFunction = ease;
            centerPosAnimation.FillBehavior = FillBehavior.Stop;
            Storyboard.Children.Add(centerPosAnimation);
            Storyboard.SetTarget(centerPosAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(centerPosAnimation, new PropertyPath(ScatterViewItem.CenterProperty));

            orientationAnimation.From = SessionVM.SessionSVI.ActualOrientation;
            orientationAnimation.To = newOrientation;
            orientationAnimation.Duration = new Duration(TimeSpan.FromSeconds(5));
            orientationAnimation.DecelerationRatio = .9;
            orientationAnimation.EasingFunction = ease;
            orientationAnimation.FillBehavior = FillBehavior.Stop;
            Storyboard.Children.Add(orientationAnimation);
            Storyboard.SetTarget(orientationAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(orientationAnimation, new PropertyPath(ScatterViewItem.OrientationProperty));

            SessionVM.SessionSVI.Center = newCenter;
            SessionVM.SessionSVI.Orientation = newOrientation;

            Storyboard.Begin();
            SessionVM.SessionSVI.CanMove = true;
            SessionVM.SessionSVI.CanRotate = true;
            SessionVM.SessionSVI.PreviewTouchDown += new EventHandler<System.Windows.Input.TouchEventArgs>(Session_PreviewTouchDown);
        }

        /// <summary>
        /// Stops the animation and set coordinates to current location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Session_PreviewTouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            Storyboard.Pause();

            SessionVM.SessionSVI.Center = SessionVM.SessionSVI.ActualCenter;
            SessionVM.SessionSVI.Orientation = SessionVM.SessionSVI.ActualOrientation;
            Storyboard.Remove();
        }
        #endregion



        #region ENLARGEMENT
        void svi_TouchLeave(object sender, System.Windows.Input.TouchEventArgs e)
        {
            SessionVM.SessionSVI = (ScatterViewItem)SessionVM.Grid.Parent;
            MainDesktop = (DesktopView)((ScatterView)SessionVM.SessionSVI.Parent).Parent;

            double Width = MainDesktop.ActualWidth;
            double Height = MainDesktop.ActualHeight;
            double Xpos = e.GetTouchPoint(MainDesktop).Position.X;
            double Ypos = e.GetTouchPoint(MainDesktop).Position.Y;

            if (Ypos > 4.0 / 18.0 * Height && Ypos < 14.0 / 18.0 * Height)
            {
                if (Xpos > 0 && Xpos < 3.0 / 32.0 * Width)
                {
                    EnlargeForSide(true);
                    SessionVM.Orientation = "left";
                }
                else if (Xpos > 29.0 / 32.0 * Width && Xpos < Width)
                {
                    EnlargeForSide(false);
                    SessionVM.Orientation = "right";
                }
            }

            if (Xpos > 4.0 / 32.0 * Width && Xpos < 28.0 / 32.0 * Width)
            {
                if (Ypos > 0 && Ypos < 3.0 / 18.0 * Height)
                {
                    if (SessionVM.Orientation == "bottom") Enlarge(-180.0);
                    if (SessionVM.Orientation == "top") Enlarge(0.0);
                    SessionVM.Orientation = "top";
                }

                else if (Ypos > 15.0 / 18.0 * Height && Ypos < Height)
                {
                    if (SessionVM.Orientation == "top") Enlarge(-180.0);
                    if (SessionVM.Orientation == "bottom") Enlarge(0.0);
                    SessionVM.Orientation = "bottom";
                }
            }
        }

        private void MakeReadyForDisplay()
        {
            Fs.Close();
            switch (SessionVM.Session.ThemeID)
            {
                default:
                    SessionVM.Grid.Background = (new Theme1ViewModel(SessionVM.Session.Theme, SessionVM)).BackgroundImage;
                    break;
                case 2:
                    SessionVM.Grid.Background = (new Theme2ViewModel(SessionVM.Session.Theme, SessionVM)).BackgroundImage;
                    break;
                case 3:
                    SessionVM.Grid.Background = (new Theme3ViewModel(SessionVM.Session.Theme, SessionVM)).BackgroundImage;
                    break;
                case 4:
                    SessionVM.Grid.Background = (new Theme4ViewModel(SessionVM.Session.Theme, SessionVM)).BackgroundImage;
                    break;
            }

            SessionVM.Grid.Children.Add(SessionVM.Bubbles);
            SessionVM.Grid.Children.Add(SessionVM.Notes);
            SessionVM.Grid.Children.Add(SessionVM.Reducer);
            SessionVM.Grid.Children.Add(SessionVM.Play);
            SessionVM.Grid.Children.Add(SessionVM.Stop);
            SessionVM.Grid.Children.Add(SessionVM.TreeUp.Grid);
            SessionVM.Grid.Children.Add(SessionVM.TreeDown.Grid);
            SessionVM.NbgVM = new NoteBubbleGeneratorViewModel(SessionVM.Session.NoteBubbleGenerator, SessionVM);
            SessionVM.MbgVM = new MelodyBubbleGeneratorViewModel(SessionVM.Session.MelodyBubbleGenerator, SessionVM);

            SessionVM.Grid.Children.Add(SessionVM.NbgVM.Grid);
            SessionVM.Grid.Children.Add(SessionVM.MbgVM.Grid);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void stb_enlarge_Completed(object sender, EventArgs e)
        {
            SessionVM.SessionSVI = (ScatterViewItem)SessionVM.Grid.Parent;
            MainDesktop = (DesktopView)((ScatterView)SessionVM.SessionSVI.Parent).Parent;

            MainDesktop.Children.Remove(SessionVM.SessionSVI);
            SessionVM.LoadSession("test.bin");
            SessionVM.Session.PlayBackgroundSound();

            RotateTransform rt = new RotateTransform();
            TranslateTransform tt;
            HorizontalAlignment ha;

            switch (SessionVM.Orientation)
            {
                case "top":
                    rt.Angle = 180;
                    rt.CenterX = (int)(SessionVM.Grid.Width / 2 + 0.5);
                    rt.CenterY = (int)(SessionVM.Grid.Height / 2 + 0.5);
                    tt = new TranslateTransform(0.0, 0.0);
                    ha = HorizontalAlignment.Center;
                    break;
                case "left":
                    rt.Angle = 90;
                    rt.CenterX = (int)(SessionVM.Grid.Width / 2 + 0.5);
                    rt.CenterY = (int)(SessionVM.Grid.Height / 2 + 0.5);
                    tt = new TranslateTransform(-236.25, 0.0);
                    ha = HorizontalAlignment.Left;
                    break;
                case "right":
                    rt.Angle = -90;
                    rt.CenterX = (int)(SessionVM.Grid.Width / 2 + 0.5);
                    rt.CenterY = (int)(SessionVM.Grid.Height / 2 + 0.5);
                    tt = new TranslateTransform(236.25, 0.0);
                    ha = HorizontalAlignment.Right;
                    break;
                default: //bottom
                    rt.Angle = 0;
                    rt.CenterX = (int)(SessionVM.Grid.Width / 2 + 0.5);
                    rt.CenterY = (int)(SessionVM.Grid.Height / 2 + 0.5);
                    tt = new TranslateTransform(0.0, 0.0);
                    ha = HorizontalAlignment.Center;
                    break;
            }
            TransformGroup group = new TransformGroup();
            group.Children.Add(rt);
            group.Children.Add(tt);
            SessionVM.Grid.LayoutTransform = group;

            MakeReadyForDisplay();
            SessionVM.SessionSVI.Content = null;
            ((ScatterView)SessionVM.SessionSVI.Parent).Items.Remove(SessionVM.SessionSVI);
            MainDesktop.Children.Add(SessionVM.Grid);
            SessionVM.Grid.HorizontalAlignment = ha;
            if (SessionVM.Orientation == "left" || SessionVM.Orientation == "right")
            {
                SessionVM.NbgVM.Grid.Width = SessionVM.NbgVM.Grid.Width * 0.5625;
                SessionVM.NbgVM.Grid.Height = SessionVM.NbgVM.Grid.Height * 0.5625;
                SessionVM.MbgVM.Grid.Width = SessionVM.MbgVM.Grid.Width * 0.5625;
                SessionVM.MbgVM.Grid.Height = SessionVM.MbgVM.Grid.Height * 0.5625;
            }

        }

        private void EnlargeForSide(Boolean left)
        {
            #region Animation Settings
            Storyboard stb = new Storyboard();
            PointAnimation centerPosAnimation = new PointAnimation();
            DoubleAnimation heightAnimation = new DoubleAnimation();
            DoubleAnimation widthAnimation = new DoubleAnimation();
            DoubleAnimation gridHeightAnimation = new DoubleAnimation();
            DoubleAnimation gridWidthAnimation = new DoubleAnimation();
            DoubleAnimation orientationAnimation = new DoubleAnimation();
            ThicknessAnimation borderAnimation = new ThicknessAnimation();

            centerPosAnimation.From = SessionVM.SessionSVI.ActualCenter;
            if(left) centerPosAnimation.To = new System.Windows.Point(MainDesktop.ActualWidth / 6.0 - 16.875, MainDesktop.ActualHeight / 2.0);
            else centerPosAnimation.To = new System.Windows.Point(5.0 * MainDesktop.ActualWidth / 6.0 + 16.875, MainDesktop.ActualHeight / 2.0);
            centerPosAnimation.Duration = new Duration(TimeSpan.FromSeconds(.75));
            centerPosAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(centerPosAnimation);
            Storyboard.SetTarget(centerPosAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(centerPosAnimation, new PropertyPath(ScatterViewItem.CenterProperty));

            if (left)
            {
                orientationAnimation.From = SessionVM.SessionSVI.ActualOrientation;
                if (SessionVM.SessionSVI.ActualOrientation <= 180)
                    orientationAnimation.To = 90;
                else
                    orientationAnimation.To = 90 + 360;
            }
            else
            {
                orientationAnimation.From = SessionVM.SessionSVI.ActualOrientation;
                if (SessionVM.SessionSVI.ActualOrientation <= 180)
                    orientationAnimation.To = -90;
                else
                    orientationAnimation.To = -90 + 360;
            }
            orientationAnimation.Duration = new Duration(TimeSpan.FromSeconds(.75));
            orientationAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(orientationAnimation);
            Storyboard.SetTarget(orientationAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(orientationAnimation, new PropertyPath(ScatterViewItem.OrientationProperty));

            heightAnimation.From = SessionVM.SessionSVI.ActualHeight;
            heightAnimation.To = SessionVM.SessionSVI.ActualHeight * 4 * 0.5625;
            heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            heightAnimation.EasingFunction = new ExponentialEase();
            heightAnimation.AccelerationRatio = 1;
            heightAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(heightAnimation);
            Storyboard.SetTarget(heightAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(ScatterViewItem.HeightProperty));

            widthAnimation.From = SessionVM.SessionSVI.ActualWidth;
            widthAnimation.To = SessionVM.SessionSVI.ActualWidth * 4 * 0.5625;
            widthAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            widthAnimation.EasingFunction = new ExponentialEase();
            widthAnimation.AccelerationRatio = 1;
            widthAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(widthAnimation);
            Storyboard.SetTarget(widthAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(ScatterViewItem.WidthProperty));

            borderAnimation.From = new Thickness(15.0);
            borderAnimation.To = new Thickness(60.0 * 0.5625);
            borderAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            borderAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(borderAnimation);
            Storyboard.SetTarget(borderAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(borderAnimation, new PropertyPath(ScatterViewItem.BorderThicknessProperty));

            gridHeightAnimation.From = SessionVM.Grid.ActualHeight;
            gridHeightAnimation.To = SessionVM.Grid.ActualHeight * 4 * 0.5625;
            gridHeightAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            gridHeightAnimation.EasingFunction = new ExponentialEase();
            gridHeightAnimation.AccelerationRatio = 1;
            gridHeightAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(gridHeightAnimation);
            Storyboard.SetTarget(gridHeightAnimation, SessionVM.Grid);
            Storyboard.SetTargetProperty(gridHeightAnimation, new PropertyPath(Grid.HeightProperty));

            gridWidthAnimation.From = SessionVM.Grid.ActualWidth;
            gridWidthAnimation.To = SessionVM.Grid.ActualWidth * 4 * 0.5625;
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

            SessionVM.Reduced = false;
        }

        private void Enlarge(double orientation)
        {

            #region Animation Settings
            Storyboard stb = new Storyboard();
            PointAnimation centerPosAnimation = new PointAnimation();
            DoubleAnimation heightAnimation = new DoubleAnimation();
            DoubleAnimation widthAnimation = new DoubleAnimation();
            DoubleAnimation gridHeightAnimation = new DoubleAnimation();
            DoubleAnimation gridWidthAnimation = new DoubleAnimation();
            DoubleAnimation orientationAnimation = new DoubleAnimation();
            ThicknessAnimation borderAnimation = new ThicknessAnimation();

            centerPosAnimation.From = SessionVM.SessionSVI.ActualCenter;
            centerPosAnimation.To = new System.Windows.Point(MainDesktop.ActualWidth / 2, MainDesktop.ActualHeight / 2);
            centerPosAnimation.Duration = new Duration(TimeSpan.FromSeconds(.75));
            centerPosAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(centerPosAnimation);
            Storyboard.SetTarget(centerPosAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(centerPosAnimation, new PropertyPath(ScatterViewItem.CenterProperty));

            orientationAnimation.From = SessionVM.SessionSVI.ActualOrientation;
            if (SessionVM.SessionSVI.ActualOrientation <= 180)
                orientationAnimation.To = orientation;
            else
                orientationAnimation.To = orientation + 360;
            orientationAnimation.Duration = new Duration(TimeSpan.FromSeconds(.75));
            orientationAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(orientationAnimation);
            Storyboard.SetTarget(orientationAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(orientationAnimation, new PropertyPath(ScatterViewItem.OrientationProperty));

            heightAnimation.From = SessionVM.SessionSVI.ActualHeight;
            heightAnimation.To = SessionVM.SessionSVI.ActualHeight * 4;
            heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            heightAnimation.EasingFunction = new ExponentialEase();
            heightAnimation.AccelerationRatio = 1;
            heightAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(heightAnimation);
            Storyboard.SetTarget(heightAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(ScatterViewItem.HeightProperty));

            widthAnimation.From = SessionVM.SessionSVI.ActualWidth;
            widthAnimation.To = SessionVM.SessionSVI.ActualWidth * 4;
            widthAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            widthAnimation.EasingFunction = new ExponentialEase();
            widthAnimation.AccelerationRatio = 1;
            widthAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(widthAnimation);
            Storyboard.SetTarget(widthAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(ScatterViewItem.WidthProperty));

            borderAnimation.From = new Thickness(15);
            borderAnimation.To = new Thickness(60);
            borderAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            borderAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(borderAnimation);
            Storyboard.SetTarget(borderAnimation, SessionVM.SessionSVI);
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
        #endregion

        void marginAnimation_Completed(object sender, EventArgs e)
        {
            SessionVM.Grid.Children.Add(SessionVM.Reducer);
        }
    }
}
