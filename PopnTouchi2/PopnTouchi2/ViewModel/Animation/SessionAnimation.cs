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
using System.Drawing.Imaging;
using System.Runtime.Serialization.Formatters.Binary;
using PopnTouchi2.Model;
using System.Windows.Input;
using System.Windows.Threading;

namespace PopnTouchi2.ViewModel.Animation
{
    /// <summary>
    /// Defines all animations used for the Session.
    /// </summary>
    public class SessionAnimation : Animation
    {
        /// <summary>
        /// Property.
        /// </summary>
        public SessionViewModel SessionVM { get; set; }
        
        /// <summary>
        /// Parameter.
        /// Instance of DesktopView
        /// </summary>
        private DesktopView MainDesktop { get; set; }

        /// <summary>
        /// Parameters.
        /// The DoubleAnimations used to reduce, enlarge the screen
        /// </summary>
        private DoubleAnimation initWidthAnimation;
        private DoubleAnimation OpacityAnimation;
        private DoubleAnimation ReduceWidthAnimation;
        private DoubleAnimation ReduceWidthAnimation2;
        private DoubleAnimation EnlargeWidthAnimation;

        /// <summary>
        /// Where user has started to touch a ScatterViewItem
        /// </summary>
        private TouchPoint startingTouchPoint;
        private TouchDevice touchDevice;

        /// <summary>
        /// FileStream used for SnapShots.
        /// </summary>
        public FileStream Fs { get; set; }

        /// <summary>
        /// Timers used for the animations
        /// </summary>
        private DispatcherTimer AutoDBRemoveDispatcherTimer = new DispatcherTimer();
        private DispatcherTimer TouchHoldDispatcherTimer = new DispatcherTimer();

        /// <summary>
        /// Constructor.
        /// Create a SessionAnimation with the current SessionViewModel
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
            SessionVM.SessionSVI.TouchEnter += new EventHandler<System.Windows.Input.TouchEventArgs>(SessionSVI_TouchEnter);

            InitStb.Begin();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="s">The current SessionViewModel</param>
        /// <param name="reduced">True if the session is reduced</param>
        /// <param name="desktop">The DesktopView</param>
        public SessionAnimation(SessionViewModel s, bool reduced, DesktopView desktop)
            : base()
        {
            MainDesktop = desktop;
            SessionVM = s;
            SessionVM.SessionSVI.TouchLeave += new EventHandler<TouchEventArgs>(svi_TouchLeave);
            SessionVM.SessionSVI.PreviewTouchDown += new EventHandler<TouchEventArgs>(SessionSVI_TouchEnter);
        }
        
        #region REDUCTION

        /// <summary>
        /// Reduce the screen with a snapshot
        /// </summary>
        public void Reduce()
        {
            if (SessionVM.Reduced) return;
            
            SessionVM.SessionSVI.MaxWidth = double.MaxValue;
            SessionVM.SessionSVI.MaxHeight = double.MaxValue;

            SessionVM.StopSound();
            SessionVM.StopPlayBar();

            foreach (MelodyBubbleViewModel mbvm in SessionVM.MbgVM.MelodyBubbleVMs)
            {
                mbvm.Animation.RemovePreview();
            }

            SessionVM.Session.StaveBottom.StopMelody();
            SessionVM.Session.StaveTop.StopMelody();

            SessionVM.IsPlaying = false;

            AudioController.PlaySoundWithString("flash");
            SessionVM.FullyEnlarged = false;

            MainDesktop = (DesktopView)((ScatterView)(SessionVM.SessionSVI.Parent)).Parent;

            stopAllBubblesAnimations();
            SessionVM.SaveSession();
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
        }

        /// <summary>
        /// Replaces the SVI content with the ImageBrush given as an argument
        /// </summary>
        /// <param name="ss">ScreenShot to set as SVI content</param>
        private void ReplaceGridWithSnapShot(ImageBrush ss)
        {
            SessionVM.Grid.Children.Remove(SessionVM.Notes);
            SessionVM.Grid.Children.Remove(SessionVM.NbgVM.Grid);
            SessionVM.Grid.Children.Remove(SessionVM.MbgVM.Grid);
            SessionVM.Grid.Children.Remove(SessionVM.TreeUp.Grid);
            SessionVM.Grid.Children.Remove(SessionVM.TreeDown.Grid);
            SessionVM.Grid.Children.Remove(SessionVM.Play_Button);
            SessionVM.Grid.Children.Remove(SessionVM.Tempo_Button);
            SessionVM.Grid.Children.Remove(SessionVM.Theme_Button);
            switch (SessionVM.Session.ThemeID)
            {
                case 2: SessionVM.Grid.Children.Remove(SessionVM.ThemeChooser.Bird); break;
                case 3: SessionVM.Grid.Children.Remove(SessionVM.ThemeChooser.Dragon); break;
                case 4: SessionVM.Grid.Children.Remove(SessionVM.ThemeChooser.Cat); break;
                default: break;
            }

            SessionVM.EraseSession();

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

        /// <summary>
        /// Occurs when Flash's animation is over
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            /*if (SessionVM.Orientation == "left" || SessionVM.Orientation == "right")
                heightAnimation.To = ((MainDesktop.ActualHeight) / 4.0) / 0.5625;*/
            heightAnimation.To = (MainDesktop.ActualHeight) / 4.0;
            heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            heightAnimation.EasingFunction = ease;
            heightAnimation.AccelerationRatio = .4;
            heightAnimation.DecelerationRatio = .1;
            heightAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(heightAnimation);
            Storyboard.SetTarget(heightAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(ScatterViewItem.HeightProperty));

            ReduceWidthAnimation.From = SessionVM.SessionSVI.ActualWidth;
            /*if (SessionVM.Orientation == "left" || SessionVM.Orientation == "right")
                ReduceWidthAnimation.To = ((MainDesktop.ActualWidth) / 4.0) / 0.5625;*/
            ReduceWidthAnimation.To = (MainDesktop.ActualWidth) / 4.0;
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

        /// <summary>
        /// Removes useless Grid's element to take the SnapShot
        /// </summary>
        /// <param name="grid">Grid concerned</param>
        private void MakeReadyForSnapShot(Grid grid)
        {
            SessionVM.Play_Button.Opacity = 1;
            grid.Children.Remove(SessionVM.Bubbles);
            grid.Children.Remove(SessionVM.UpdateSound.Grid);
            grid.Children.Remove(SessionVM.PlayBar);
            grid.Children.Remove(SessionVM.PlayBarCache);
            grid.Children.Remove(SessionVM.StaveCache);
            grid.Children.Remove(SessionVM.previewGrid);
            grid.Children.Remove(SessionVM.topStaveHighlight);
            grid.Children.Remove(SessionVM.bottomStaveHighlight);
        }

        /// <summary>
        /// Stops all bubbles animations
        /// </summary>
        public void stopAllBubblesAnimations()
        {
            foreach (NoteBubbleViewModel nb in SessionVM.NbgVM.NoteBubbleVMs)
                nb.Animation.StopAnimation();
            foreach (MelodyBubbleViewModel mb in SessionVM.MbgVM.MelodyBubbleVMs)
                mb.Animation.StopAnimation();
        }

        /// <summary>
        /// Stops all bubbles animations
        /// </summary>
        public void resumeAllBubblesAnimations()
        {
            foreach (NoteBubbleViewModel nb in SessionVM.NbgVM.NoteBubbleVMs)
            {
                nb.Animation.canAnimate = true;
                nb.Animation.BeginBubbleAnimation();
            }
            foreach (MelodyBubbleViewModel mb in SessionVM.MbgVM.MelodyBubbleVMs)
            {
                mb.Animation.canAnimate = true;
                mb.Animation.BeginBubbleAnimation();
            }
        }

        /// <summary>
        /// Event occured when the reducing storyboard is completed
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

            borderAnimation.From = new Thickness(0.0);
            borderAnimation.To = new Thickness(15.0);
            borderAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            borderAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(borderAnimation);
            Storyboard.SetTarget(borderAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(borderAnimation, new PropertyPath(ScatterViewItem.BorderThicknessProperty));

            heightAnimation.From = SessionVM.SessionSVI.ActualHeight;
            heightAnimation.To = SessionVM.SessionSVI.ActualHeight + 30.0;
            heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            heightAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(heightAnimation);
            Storyboard.SetTarget(heightAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(ScatterViewItem.HeightProperty));

            ReduceWidthAnimation2.From = SessionVM.SessionSVI.ActualWidth;
            ReduceWidthAnimation2.To = SessionVM.SessionSVI.ActualWidth + 30.0;
            ReduceWidthAnimation2.Duration = new Duration(TimeSpan.FromSeconds(.5));
            ReduceWidthAnimation2.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(ReduceWidthAnimation2);
            Storyboard.SetTarget(ReduceWidthAnimation2, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(ReduceWidthAnimation2, new PropertyPath(ScatterViewItem.WidthProperty));

            ReduceWidthAnimation2.Completed += new EventHandler(stb_border_Completed);
            stb.Begin(SessionVM.SessionSVI);
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

            MainDesktop.CheckDesktopToDisplay();

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

            SessionVM.SessionSVI.CanMove = true;
            SessionVM.SessionSVI.CanRotate = true;
            SessionVM.SessionSVI.CanScale = false;

            SessionVM.SessionSVI.PreviewTouchDown += new EventHandler<System.Windows.Input.TouchEventArgs>(Session_PreviewTouchDown);
            SessionVM.Reduced = true;
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
        /// <summary>
        /// Starts the chrono to know if user is waiting for Deletion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SessionSVI_TouchEnter(object sender, TouchEventArgs e)
        {
            try
            {
                if (SessionVM.Reduced)
                {
                    if (MainDesktop.LeftSessionActive || MainDesktop.RightSessionActive) MainDesktop.DisplayPreviewZones(true, false);
                    else MainDesktop.DisplayPreviewZones(true, true);
                }
            }
            catch (Exception exc) { }

            if (!SessionVM.Reduced) return;
            startingTouchPoint = e.GetTouchPoint(MainDesktop);
            touchDevice = e.TouchDevice;

            TouchHoldDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            TouchHoldDispatcherTimer.Interval = TimeSpan.FromSeconds(1.0);
            TouchHoldDispatcherTimer.Start();
            TouchHoldDispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
        }

        /// <summary>
        /// Event from the DispatcherTimer
        /// Used for the delete button
        /// which appears after several secondes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (SessionVM == null) return;
            if (Math.Abs(touchDevice.GetTouchPoint(MainDesktop).Position.X - startingTouchPoint.Position.X) < 15.0 &&
                Math.Abs(touchDevice.GetTouchPoint(MainDesktop).Position.Y - startingTouchPoint.Position.Y) < 15.0)
            {
                SessionVM.DeleteButton.Visibility = Visibility.Visible;
                AutoDBRemoveDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                AutoDBRemoveDispatcherTimer.Interval = TimeSpan.FromSeconds(2);
                AutoDBRemoveDispatcherTimer.Tick += new EventHandler(autoDeleteRemove);
                AutoDBRemoveDispatcherTimer.Start();

            }
            else
            {
                startingTouchPoint = touchDevice.GetTouchPoint(MainDesktop);
            }
        }

        /// <summary>
        /// Stop the Timer and hide the DeleteButton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void autoDeleteRemove(object sender, EventArgs e)
        {
            AutoDBRemoveDispatcherTimer.Stop();
            if (SessionVM == null) return;
            SessionVM.DeleteButton.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Delete a session
        /// </summary>
        public void DeleteSession()
        {
            SessionVM.BeingDeleted = true;

            Storyboard DeleteStb = new Storyboard();
            DoubleAnimation endingAnimation = new DoubleAnimation();
            DoubleAnimation endingWidthAnimation = new DoubleAnimation();
            DoubleAnimation endingHeightAnimation = new DoubleAnimation();

            ExponentialEase ease = new ExponentialEase();
            ease.EasingMode = EasingMode.EaseOut;
            ease.Exponent = 2;

            endingAnimation.From = 1;
            endingAnimation.To = 0;
            endingAnimation.Duration = new Duration(TimeSpan.FromSeconds(.3));
            endingAnimation.FillBehavior = FillBehavior.HoldEnd;
            DeleteStb.Children.Add(endingAnimation);
            Storyboard.SetTarget(endingAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(endingAnimation, new PropertyPath(ScatterViewItem.OpacityProperty));

            endingWidthAnimation.From = SessionVM.SessionSVI.Width;
            endingWidthAnimation.To = SessionVM.SessionSVI.Width * 0.8;
            endingWidthAnimation.Duration = new Duration(TimeSpan.FromSeconds(.4));
            endingWidthAnimation.EasingFunction = ease;
            endingWidthAnimation.FillBehavior = FillBehavior.Stop;
            DeleteStb.Children.Add(endingWidthAnimation);
            Storyboard.SetTarget(endingWidthAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(endingWidthAnimation, new PropertyPath(ScatterViewItem.WidthProperty));

            endingHeightAnimation.From = SessionVM.SessionSVI.Height;
            endingHeightAnimation.To = SessionVM.SessionSVI.Height * 0.8;
            endingHeightAnimation.Duration = new Duration(TimeSpan.FromSeconds(.4));
            endingHeightAnimation.EasingFunction = ease;
            endingHeightAnimation.FillBehavior = FillBehavior.Stop;
            DeleteStb.Children.Add(endingHeightAnimation);
            Storyboard.SetTarget(endingHeightAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(endingHeightAnimation, new PropertyPath(ScatterViewItem.HeightProperty));

            endingWidthAnimation.Completed += new EventHandler(deleteAnimationCompleted);

            DeleteStb.Begin();
        }
        
        /// <summary>
        /// Event occured after the ScatterView of a session was deleted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void deleteAnimationCompleted(object sender, EventArgs e)
        {
            SessionVM.DeleteSession();
            MainDesktop.Photos.Items.Remove(SessionVM.SessionSVI);
            SessionVM = null;
        }

        /// <summary>
        /// Launched when user is done with messing with a SVI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void svi_TouchLeave(object sender, TouchEventArgs e)
        {
            TouchHoldDispatcherTimer.Stop();
            if (SessionVM == null) return;
            if (SessionVM.DeleteButton.Visibility == Visibility.Visible) return;
            if (SessionVM.BeingDeleted) return;
            if (!SessionVM.Reduced) return;

            SessionVM.SessionSVI = (ScatterViewItem)SessionVM.Grid.Parent;
            MainDesktop = (DesktopView)((ScatterView)SessionVM.SessionSVI.Parent).Parent;

            double Width = MainDesktop.ActualWidth;
            double Height = MainDesktop.ActualHeight;
            double Xpos = e.GetTouchPoint(MainDesktop).Position.X;
            double Ypos = e.GetTouchPoint(MainDesktop).Position.Y;

            if (Ypos > 4.0 / 18.0 * Height && Ypos < 14.0 / 18.0 * Height)
            {
                if (Xpos > 0 && Xpos < 3.0 / 32.0 * Width && !MainDesktop.LeftSessionActive)
                {
                    SessionVM.Reduced = false;
                    EnlargeForSide(true);
                    SessionVM.DeleteButton.Visibility = Visibility.Hidden;
                    SessionVM.Orientation = "left";
                }
                else if (Xpos > 29.0 / 32.0 * Width && Xpos < Width && !MainDesktop.RightSessionActive)
                {
                    SessionVM.Reduced = false;
                    EnlargeForSide(false);
                    SessionVM.DeleteButton.Visibility = Visibility.Hidden;
                    SessionVM.Orientation = "right";
                }
            }

            if (MainDesktop.LeftSessionActive || MainDesktop.RightSessionActive) return;
            if (Xpos > 4.0 / 32.0 * Width && Xpos < 28.0 / 32.0 * Width)
            {
                if (Ypos > 0 && Ypos < 3.0 / 18.0 * Height && MainDesktop.Sessions.Items.Count == 0)
                {
                    SessionVM.Reduced = false;
                    Enlarge(-180.0);
                    SessionVM.DeleteButton.Visibility = Visibility.Hidden;
                    SessionVM.Orientation = "top";
                }

                else if (Ypos > 15.0 / 18.0 * Height && Ypos < Height && MainDesktop.Sessions.Items.Count == 0)
                {
                    SessionVM.Reduced = false;
                    Enlarge(0.0);
                    SessionVM.DeleteButton.Visibility = Visibility.Hidden;
                    SessionVM.Orientation = "bottom";
                }
            }
        }
        
        /// <summary>
        /// Enlarges a SVI to go mid-screen
        /// </summary>
        /// <param name="left"></param>
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
            heightAnimation.To = (MainDesktop.ActualHeight + 120.0) * 0.5625;
            heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            heightAnimation.EasingFunction = new ExponentialEase();
            heightAnimation.AccelerationRatio = 1;
            heightAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(heightAnimation);
            Storyboard.SetTarget(heightAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(ScatterViewItem.HeightProperty));

            EnlargeWidthAnimation.From = SessionVM.SessionSVI.ActualWidth;
            EnlargeWidthAnimation.To = (MainDesktop.ActualWidth + 120.0) * 0.5625;
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

        /// <summary>
        /// Enlarges a SVI to go fullscreen
        /// </summary>
        /// <param name="orientation"></param>
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
            heightAnimation.To = MainDesktop.ActualHeight + 120.0;
            heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            heightAnimation.EasingFunction = new ExponentialEase();
            heightAnimation.AccelerationRatio = 1;
            heightAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(heightAnimation);
            Storyboard.SetTarget(heightAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(ScatterViewItem.HeightProperty));

            widthAnimation.From = SessionVM.SessionSVI.ActualWidth;
            widthAnimation.To = MainDesktop.ActualWidth + 120.0;
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
        /// Event occured when the session is totally enlarged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void stb_enlarge_Completed(object sender, EventArgs e)
        {

            MainDesktop = (DesktopView)((ScatterView)SessionVM.SessionSVI.Parent).Parent;

            SessionVM.LoadSession();
            SessionVM.UpdateSound.Grid.Opacity = 0.0;
            Fs.Close();
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
            heightAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(heightAnimation);
            Storyboard.SetTarget(heightAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(ScatterViewItem.HeightProperty));

            widthAnimation.From = SessionVM.SessionSVI.ActualWidth;
            if (SessionVM.Orientation == "left" || SessionVM.Orientation == "right") widthAnimation.To = SessionVM.SessionSVI.ActualWidth - 67.5;
            else widthAnimation.To = SessionVM.SessionSVI.ActualWidth - 120;
            widthAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            widthAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(widthAnimation);
            Storyboard.SetTarget(widthAnimation, SessionVM.SessionSVI);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(ScatterViewItem.WidthProperty));

            borderAnimation.Completed += new EventHandler(borderAnimation_Completed);

            stb.Begin(SessionVM.SessionSVI);
        }

        /// <summary>
        /// Event occured when the RemoveWhiteBorder Event is finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void borderAnimation_Completed(object sender, EventArgs e)
        {
            SessionVM.DisplayGrid(SessionVM.UpdateSound.Grid, true);

            SessionVM.originalRatio = SessionVM.SessionSVI.ActualWidth / 1920.0;
            SessionVM.SessionSVI.MaxWidth = SessionVM.SessionSVI.ActualWidth;
            SessionVM.SessionSVI.MaxHeight = SessionVM.SessionSVI.ActualHeight;

            SessionVM.SessionSVI.CanScale = true;
            SessionVM.FullyEnlarged = true;
        }
        #endregion

        /// <summary>
        /// Launched when session's opening animation ends
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void marginAnimation_Completed(object sender, EventArgs e)
        {
            SessionVM.SessionSVI.CanScale = true;
            SessionVM.FullyEnlarged = true;
            SessionVM.InitialScale = false;
            SessionVM.SessionSVI.MaxWidth = SessionVM.originalRatio * 1920.0;
            SessionVM.SessionSVI.MaxHeight = SessionVM.originalRatio * 1080.0;
            SessionVM.Grid.Children.Add(SessionVM.Theme_Button);
            Grid.SetZIndex(SessionVM.Theme_Button, 5);
        }
    }
}
