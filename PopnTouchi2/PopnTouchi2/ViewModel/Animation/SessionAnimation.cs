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

        private DoubleAnimation initWidthAnimation;
        private DoubleAnimation OpacityAnimation;
        private DoubleAnimation ReduceWidthAnimation;
        private DoubleAnimation ReduceWidthAnimation2;
        private DoubleAnimation EnlargeWidthAnimation;

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

            Storyboard InitStb = new Storyboard();
            DoubleAnimation openingAnimation = new DoubleAnimation();
            initWidthAnimation = new DoubleAnimation();
            DoubleAnimation initHeightAnimation = new DoubleAnimation();

            ExponentialEase ease = new ExponentialEase();
            ease.EasingMode = EasingMode.EaseOut;
            ease.Exponent = 2;

            openingAnimation.From = 0;
            openingAnimation.To = 1;
            openingAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            openingAnimation.FillBehavior = FillBehavior.HoldEnd;
            InitStb.Children.Add(openingAnimation);
            Storyboard.SetTarget(openingAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(openingAnimation, new PropertyPath(ScatterViewItem.OpacityProperty));

            initWidthAnimation.From = s.SessionSVI.Width * 0.8;
            initWidthAnimation.To = s.SessionSVI.Width;
            initWidthAnimation.Duration = new Duration(TimeSpan.FromSeconds(.6));
            initWidthAnimation.EasingFunction = ease;
            initWidthAnimation.FillBehavior = FillBehavior.Stop;
            InitStb.Children.Add(initWidthAnimation);
            Storyboard.SetTarget(initWidthAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(initWidthAnimation, new PropertyPath(ScatterViewItem.WidthProperty));

            initHeightAnimation.From = s.SessionSVI.Height * 0.8;
            initHeightAnimation.To = s.SessionSVI.Height;
            initHeightAnimation.Duration = new Duration(TimeSpan.FromSeconds(.6));
            initHeightAnimation.EasingFunction = ease;
            initHeightAnimation.FillBehavior = FillBehavior.Stop;
            InitStb.Children.Add(initHeightAnimation);
            Storyboard.SetTarget(initHeightAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(initHeightAnimation, new PropertyPath(ScatterViewItem.HeightProperty));

            initWidthAnimation.Completed += new EventHandler(marginAnimation_Completed);
            SessionVM.SessionSVI.TouchLeave += new EventHandler<System.Windows.Input.TouchEventArgs>(svi_TouchLeave);

            InitStb.Begin();
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


            AudioController.PlaySoundWithString("flash");

            MainDesktop = (DesktopView)((ScatterView)(SessionVM.SessionSVI.Parent)).Parent;

            SessionVM.SaveSession("test.bin");
            SessionVM.Session.StopBackgroundSound();

            MakeReadyForSnapShot(SessionVM.Grid);
            MemoryStream ms;
            if (SessionVM.Orientation == "left" || SessionVM.Orientation == "right") ms = new MemoryStream(Screenshot.GetSnapshot(SessionVM.Grid, 1, 100));
            else ms = new MemoryStream(Screenshot.GetSnapshot(SessionVM.Grid, 1, 100));
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
            OpacityAnimation = new DoubleAnimation();

            OpacityAnimation.From = 1;
            OpacityAnimation.To = 0;
            OpacityAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            OpacityAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(OpacityAnimation);
            Storyboard.SetTarget(OpacityAnimation, rect);
            Storyboard.SetTargetProperty(OpacityAnimation, new PropertyPath(System.Windows.Shapes.Rectangle.OpacityProperty));

            OpacityAnimation.Completed += new EventHandler(flash_Completed);

            stb.Begin();
            SessionVM.Grid.Background = ss;
        }

        void flash_Completed(object sender, EventArgs e)
        {

            #region Animation Settings
            Storyboard stb = new Storyboard();
            DoubleAnimation heightAnimation = new DoubleAnimation();
            ReduceWidthAnimation = new DoubleAnimation();

            ExponentialEase ease = new ExponentialEase();
            ease.EasingMode = EasingMode.EaseInOut;
            ease.Exponent = 1.5;

            heightAnimation.From = SessionVM.SessionSVI.ActualHeight;
            if (SessionVM.Orientation == "left" || SessionVM.Orientation == "right")
                heightAnimation.To = (SessionVM.SessionSVI.ActualHeight / 4.0) / 0.5625;
            else heightAnimation.To = SessionVM.SessionSVI.ActualHeight / 4.0;
            heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            heightAnimation.EasingFunction = ease;
            heightAnimation.AccelerationRatio = .4;
            heightAnimation.DecelerationRatio = .1;
            heightAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(heightAnimation);
            Storyboard.SetTarget(heightAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(ScatterViewItem.HeightProperty));

            ReduceWidthAnimation.From = SessionVM.SessionSVI.ActualWidth;
            if (SessionVM.Orientation == "left" || SessionVM.Orientation == "right")
                ReduceWidthAnimation.To = (SessionVM.SessionSVI.ActualWidth / 4.0) / 0.5625;
            else ReduceWidthAnimation.To = SessionVM.SessionSVI.ActualWidth / 4.0;
            ReduceWidthAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            ReduceWidthAnimation.EasingFunction = ease;
            ReduceWidthAnimation.AccelerationRatio = .4;
            ReduceWidthAnimation.DecelerationRatio = .1;
            ReduceWidthAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(ReduceWidthAnimation);
            Storyboard.SetTarget(ReduceWidthAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(ReduceWidthAnimation, new PropertyPath(ScatterViewItem.WidthProperty));

            if (SessionVM.Orientation == "left" || SessionVM.Orientation == "right")
            {
                SessionVM.SessionSVI.Height = SessionVM.SessionSVI.ActualHeight / 4.0 / 0.5625;
                SessionVM.SessionSVI.Width = SessionVM.SessionSVI.ActualWidth / 4.0 / 0.5625;
            }
            else
            {
                SessionVM.SessionSVI.Height = SessionVM.SessionSVI.ActualHeight / 4.0;
                SessionVM.SessionSVI.Width = SessionVM.SessionSVI.ActualWidth / 4.0;
            }
            #endregion

            ReduceWidthAnimation.Completed += new EventHandler(stb_Completed);
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
            MainDesktop.UnhidePhotos();

            SessionVM.SessionSVI.BorderBrush = System.Windows.Media.Brushes.White;


            Storyboard stb = new Storyboard();
            ThicknessAnimation borderAnimation = new ThicknessAnimation();
            DoubleAnimation heightAnimation = new DoubleAnimation();
            ReduceWidthAnimation2 = new DoubleAnimation();

            borderAnimation.From = new Thickness(0);
            borderAnimation.To = new Thickness(15);
            borderAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            borderAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(borderAnimation);
            Storyboard.SetTarget(borderAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(borderAnimation, new PropertyPath(ScatterViewItem.BorderThicknessProperty));

            heightAnimation.From = SessionVM.SessionSVI.ActualHeight;
            heightAnimation.To = SessionVM.SessionSVI.ActualHeight + 30;
            heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            heightAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(heightAnimation);
            Storyboard.SetTarget(heightAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(ScatterViewItem.HeightProperty));

            ReduceWidthAnimation2.From = SessionVM.SessionSVI.ActualWidth;
            ReduceWidthAnimation2.To = SessionVM.SessionSVI.ActualWidth + 30;
            ReduceWidthAnimation2.Duration = new Duration(TimeSpan.FromSeconds(.5));
            ReduceWidthAnimation2.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(ReduceWidthAnimation2);
            Storyboard.SetTarget(ReduceWidthAnimation2, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(ReduceWidthAnimation2, new PropertyPath(ScatterViewItem.WidthProperty));

            ReduceWidthAnimation2.Completed += new EventHandler(stb_border_Completed);
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
            MainDesktop.Sessions.Items.Remove(SessionVM.SessionSVI);
            MainDesktop.Photos.Items.Add(SessionVM.SessionSVI);

            if (SessionVM.Orientation == "left") MainDesktop.LeftSessionActive = false;
            if (SessionVM.Orientation == "right") MainDesktop.RightSessionActive = false;
            
            Storyboard = new Storyboard();
            PointAnimation centerPosAnimation = new PointAnimation();
            DoubleAnimation orientationAnimation = new DoubleAnimation();

            ExponentialEase ease = new ExponentialEase();
            ease.EasingMode = EasingMode.EaseOut;
            ease.Exponent = 1.5;

            Random r = new Random();
            Point newCenter = new Point(r.Next((int)MainDesktop.Photos.ActualWidth), r.Next((int)MainDesktop.Photos.ActualHeight));
            Double newOrientation = r.Next(-180, 180);

            if (SessionVM.Orientation == "right" && MainDesktop.LeftSessionActive)
                centerPosAnimation.From = new Point(SessionVM.SessionSVI.ActualCenter.X - 607.5, SessionVM.SessionSVI.ActualCenter.Y);
            else centerPosAnimation.From = SessionVM.SessionSVI.ActualCenter;
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
            MainDesktop.CheckDesktopToDisplay();

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
            if (!SessionVM.Reduced) return;

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
                    SessionVM.Reduced = false;
                    EnlargeForSide(true);
                    SessionVM.Orientation = "left";
                }
                else if (Xpos > 29.0 / 32.0 * Width && Xpos < Width)
                {
                    SessionVM.Reduced = false;
                    EnlargeForSide(false);
                    SessionVM.Orientation = "right";
                }
            }

            if (MainDesktop.LeftSessionActive || MainDesktop.RightSessionActive) return;
            if (Xpos > 4.0 / 32.0 * Width && Xpos < 28.0 / 32.0 * Width)
            {
                if (Ypos > 0 && Ypos < 3.0 / 18.0 * Height)
                {
                    SessionVM.Reduced = false;
                    Enlarge(-180.0);
                    SessionVM.Orientation = "top";
                }

                else if (Ypos > 15.0 / 18.0 * Height && Ypos < Height)
                {
                    SessionVM.Reduced = false;
                    Enlarge(0.0);
                    SessionVM.Orientation = "bottom";
                }
            }
        }

        private void MakeReadyForDisplay()
        {
            Fs.Close();

            SessionVM.Grid.Background = (new ThemeViewModel(SessionVM.Session.Theme, SessionVM)).BackgroundImage;
            
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

            SessionVM.SetDimensions(SessionVM.Grid.ActualWidth, SessionVM.Grid.ActualHeight);
        }
        
        private void EnlargeForSide(Boolean left)
        {
            MainDesktop.Photos.Items.Remove(SessionVM.SessionSVI);
            MainDesktop.Sessions.Items.Add(SessionVM.SessionSVI);
            if (left) MainDesktop.LeftSessionActive = true;
            else MainDesktop.RightSessionActive = true;
            MainDesktop.CheckDesktopToDisplay();

            #region Animation Settings
            Storyboard stb = new Storyboard();
            PointAnimation centerPosAnimation = new PointAnimation();
            DoubleAnimation heightAnimation = new DoubleAnimation();
            EnlargeWidthAnimation = new DoubleAnimation();
            DoubleAnimation orientationAnimation = new DoubleAnimation();
            ThicknessAnimation borderAnimation = new ThicknessAnimation();

            if (MainDesktop.LeftSessionActive && SessionVM.Orientation == "right")
                centerPosAnimation.From = new Point(SessionVM.SessionSVI.ActualCenter.X + 607.5, SessionVM.SessionSVI.ActualCenter.Y);
            else
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
            heightAnimation.To = SessionVM.SessionSVI.ActualHeight * 4.0 * 0.5625;
            heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            heightAnimation.EasingFunction = new ExponentialEase();
            heightAnimation.AccelerationRatio = 1;
            heightAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(heightAnimation);
            Storyboard.SetTarget(heightAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(ScatterViewItem.HeightProperty));

            EnlargeWidthAnimation.From = SessionVM.SessionSVI.ActualWidth;
            EnlargeWidthAnimation.To = SessionVM.SessionSVI.ActualWidth * 4.0 * 0.5625;
            EnlargeWidthAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            EnlargeWidthAnimation.EasingFunction = new ExponentialEase();
            EnlargeWidthAnimation.AccelerationRatio = 1;
            EnlargeWidthAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(EnlargeWidthAnimation);
            Storyboard.SetTarget(EnlargeWidthAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(EnlargeWidthAnimation, new PropertyPath(ScatterViewItem.WidthProperty));

            borderAnimation.From = new Thickness(15.0);
            borderAnimation.To = new Thickness(60.0 * 0.5625);
            borderAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            borderAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(borderAnimation);
            Storyboard.SetTarget(borderAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(borderAnimation, new PropertyPath(ScatterViewItem.BorderThicknessProperty));
            
            #endregion

            EnlargeWidthAnimation.Completed += new EventHandler(stb_enlarge_Completed);
            stb.Begin(SessionVM.Grid);
        }

        private void Enlarge(double orientation)
        {
            MainDesktop.Photos.Items.Remove(SessionVM.SessionSVI);
            MainDesktop.Sessions.Items.Add(SessionVM.SessionSVI);
            MainDesktop.CheckDesktopToDisplay();

            #region Animation Settings
            Storyboard stb = new Storyboard();
            PointAnimation centerPosAnimation = new PointAnimation();
            DoubleAnimation heightAnimation = new DoubleAnimation();
            DoubleAnimation widthAnimation = new DoubleAnimation();
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
            heightAnimation.To = SessionVM.SessionSVI.ActualHeight * 4.0;
            heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            heightAnimation.EasingFunction = new ExponentialEase();
            heightAnimation.AccelerationRatio = 1;
            heightAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(heightAnimation);
            Storyboard.SetTarget(heightAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(ScatterViewItem.HeightProperty));

            widthAnimation.From = SessionVM.SessionSVI.ActualWidth;
            widthAnimation.To = SessionVM.SessionSVI.ActualWidth * 4.0;
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
            #endregion

            widthAnimation.Completed += new EventHandler(stb_enlarge_Completed);
            stb.Begin(SessionVM.SessionSVI);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void stb_enlarge_Completed(object sender, EventArgs e)
        {

            MainDesktop = (DesktopView)((ScatterView)SessionVM.SessionSVI.Parent).Parent;

            SessionVM.LoadSession("test.bin");
            SessionVM.Session.PlayBackgroundSound();

            MakeReadyForDisplay();
            RemoveWhiteBorder();
        }

        /// <summary>
        /// Removes a SVI's White borders
        /// </summary>
        public void RemoveWhiteBorder()
        {
            Storyboard stb = new Storyboard();
            ThicknessAnimation borderAnimation = new ThicknessAnimation();
            DoubleAnimation heightAnimation = new DoubleAnimation();
            DoubleAnimation widthAnimation = new DoubleAnimation();

            borderAnimation.From = SessionVM.SessionSVI.BorderThickness;
            borderAnimation.To = new Thickness(0);
            borderAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            borderAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(borderAnimation);
            Storyboard.SetTarget(borderAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(borderAnimation, new PropertyPath(ScatterViewItem.BorderThicknessProperty));

            heightAnimation.From = SessionVM.SessionSVI.ActualHeight;
            if (SessionVM.Orientation == "left" || SessionVM.Orientation == "right") heightAnimation.To = SessionVM.SessionSVI.ActualHeight - 67.5;
            else heightAnimation.To = SessionVM.SessionSVI.ActualHeight - 120;
            heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            heightAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(heightAnimation);
            Storyboard.SetTarget(heightAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(ScatterViewItem.HeightProperty));

            widthAnimation.From = SessionVM.SessionSVI.ActualWidth;
            if (SessionVM.Orientation == "left" || SessionVM.Orientation == "right") widthAnimation.To = SessionVM.SessionSVI.ActualWidth - 67.5;
            else widthAnimation.To = SessionVM.SessionSVI.ActualWidth - 120;
            widthAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            widthAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(widthAnimation);
            Storyboard.SetTarget(widthAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(ScatterViewItem.WidthProperty));

            stb.Begin(SessionVM.SessionSVI);
        }
        #endregion

        void marginAnimation_Completed(object sender, EventArgs e)
        {
            SessionVM.Grid.Children.Add(SessionVM.Reducer);
        }
    }
}
