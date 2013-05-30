using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Surface.Presentation.Controls;
using PopnTouchi2.ViewModel;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using PopnTouchi2.Model;
using PopnTouchi2.Model.Enums;
using System.Windows.Threading;

namespace PopnTouchi2
{
    /// <summary>
    /// Grid contained by the SurfaceWindow.
    /// </summary>
    public class DesktopView : Grid
    {
        /// <summary>
        /// Property.
        /// The current SessionVM.
        /// </summary>
        public SessionViewModel SessionVM { get; set; }

        /// <summary>
        /// Idle Pictures
        /// </summary>
        public ScatterView Photos { get; set; }

        /// <summary>
        /// Active Sessions
        /// </summary>
        public ScatterView Sessions { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public bool LeftSessionActive { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public bool RightSessionActive { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public List<int> IDs { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        Grid MiddleCacheGrid { get; set; }

        private Grid topZonesPreview;
        private Grid sideZonesPreview;

        private bool simpleButtonDirection;
        private bool doubleButtonDirection;

        /// <summary>
        /// temporary
        /// </summary>
        public SurfaceButton CreateDoubleSession_Button { get; set; }

        /// <summary>
        /// temporary
        /// </summary>
        public SurfaceButton CreateSession_Button { get; set; }

        private List<SessionViewModel> openedSessions;

        /// <summary>
        /// Initializes few components including the session.
        /// </summary>
        public DesktopView()
        {
            simpleButtonDirection = false;
            doubleButtonDirection = false;
            IDs = new List<int>();
            openedSessions = new List<SessionViewModel>();

            Image MiddleCache = new Image();
            MiddleCache.Source = new BitmapImage(new Uri(@"../../Resources/Images/desktopSmall.jpg", UriKind.Relative));
            MiddleCache.Margin = new Thickness(607.5, 0.0, 607.5, 0.0);

            System.Windows.Shapes.Rectangle BlackBG = new System.Windows.Shapes.Rectangle();
            BlackBG.Fill = Brushes.Black;
            BlackBG.Margin = new Thickness(0.0);

            MiddleCacheGrid = new Grid();
            MiddleCacheGrid.Children.Add(MiddleCache);
            MiddleCacheGrid.Children.Add(BlackBG);
            MiddleCacheGrid.Opacity = 0;

            topZonesPreview = new Grid();
            ImageBrush previewTopGridImage = new ImageBrush();
            previewTopGridImage.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/ui_items/desktopTopZones.png", UriKind.Relative));
            topZonesPreview.Background = previewTopGridImage;
            topZonesPreview.Opacity = 0; 
            sideZonesPreview = new Grid();
            ImageBrush previewSideGridImage = new ImageBrush();
            previewSideGridImage.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/ui_items/desktopSideZones.png", UriKind.Relative));
            sideZonesPreview.Background = previewSideGridImage;
            sideZonesPreview.Opacity = 0;

            CreateSession_Button = new SurfaceButton();
            CreateSession_Button.Width = 85;
            CreateSession_Button.Height = 85;
            CreateSession_Button.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            CreateSession_Button.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CreateSession_Button.Margin = new Thickness(0.0, 0.0, 150.0, 150.0);
            ImageBrush OnePButtonBG = new ImageBrush();
            OnePButtonBG.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/ui_items/one_player.png", UriKind.Relative));
            CreateSession_Button.Background = OnePButtonBG;
            CreateSession_Button.Click += new RoutedEventHandler(CreateSession_Button_Click);

            CreateDoubleSession_Button = new SurfaceButton();
            CreateDoubleSession_Button.Width = 125;
            CreateDoubleSession_Button.Height = 125;
            CreateDoubleSession_Button.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            CreateDoubleSession_Button.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            CreateDoubleSession_Button.Margin = new Thickness(150.0, 150.0, 0.0, 0.0);
            ImageBrush TwoPButtonBG = new ImageBrush();
            TwoPButtonBG.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/ui_items/two_players.png", UriKind.Relative));
            CreateDoubleSession_Button.Background = TwoPButtonBG;
            CreateDoubleSession_Button.Click += new RoutedEventHandler(CreateDoubleSession_Button_Click);
            
            Children.Add(CreateSession_Button);
            Children.Add(CreateDoubleSession_Button);

            Photos = new ScatterView();
            Sessions = new ScatterView();
            Children.Add(Photos);
            Children.Add(Sessions);
            Children.Add(MiddleCacheGrid);
            Children.Add(topZonesPreview);
            Children.Add(sideZonesPreview);

            Grid.SetZIndex(BlackBG, 0);
            Grid.SetZIndex(MiddleCache, 1);
            Grid.SetZIndex(CreateSession_Button, 2);
            Grid.SetZIndex(CreateDoubleSession_Button, 2);
            Grid.SetZIndex(Photos, 3);
            Grid.SetZIndex(Sessions, 4);

            PreviewTouchDown += new EventHandler<TouchEventArgs>(DesktopView_PreviewTouchDown);
            PreviewTouchUp += new EventHandler<TouchEventArgs>(DesktopView_PreviewTouchUp);

            Loaded += new RoutedEventHandler(DesktopView_Loaded);
        }

        void DesktopView_Loaded(object sender, RoutedEventArgs e)
        {
            AnimateSimpleSessionButton();
            AnimateDoubleSessionButton();

            string sPath = "Sessions/";
            Random r = new Random();
            foreach (string sFileName in System.IO.Directory.GetFiles(sPath))
            {
                if (System.IO.Path.GetExtension(sFileName) == ".bin")
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    FileStream stream = File.Open(sFileName, FileMode.Open);
                    SessionData sd = (SessionData)formatter.Deserialize(stream);
                    stream.Close();

                    if (File.Exists("SnapShots/sc" + sd.SessionID + ".jpg"))
                    {
                        IDs.Add(sd.SessionID);

                        string path = "SnapShots/sc" + sd.SessionID + ".jpg";
                        ImageBrush ss = new ImageBrush();
                        BitmapImage bi = new BitmapImage();
                        FileStream Fs = new FileStream(path, FileMode.Open, FileAccess.Read);

                        SessionVM = new SessionViewModel(ActualWidth, ActualHeight, new Session(), IDs, sd.SessionID);
                        SessionVM.LoadReducedSession(Fs, this);

                        bi.BeginInit();
                        bi.StreamSource = Fs;
                        bi.EndInit();
                        ss.ImageSource = bi;

                        SessionVM.Grid.Background = ss;

                        Photos.Items.Add(SessionVM.SessionSVI);
                        SessionVM.SessionSVI.Center = new Point(r.Next((int)ActualWidth), r.Next((int)ActualHeight));
                        openedSessions.Add(SessionVM);
                    }
                    else
                    {
                        try { File.Delete(sFileName); }
                        catch (Exception exc) { }
                    }
                }
            }
        }

        void DesktopView_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            DisplayPreviewZones(false, true);
            foreach (SessionViewModel svm in openedSessions)
            {
                if (svm.removeDeleteButtonsOnTouchUp)
                {
                    svm.DeleteButton.Visibility = Visibility.Hidden;
                    svm.removeDeleteButtonsOnTouchUp = false;
                }
            }
        }

        void DesktopView_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            foreach (SessionViewModel svm in openedSessions)
            {
                if (svm.DeleteButton.Visibility == Visibility.Visible)
                    svm.removeDeleteButtonsOnTouchUp = true;
            }
        }

        /// <summary>
        /// Event handling the Create Session Button Click Action.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CreateSession_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Sessions.Items.Count == 0)
            {
                SessionVM = new SessionViewModel(ActualWidth, ActualHeight, new Session(), IDs, true);
                Sessions.Items.Add(SessionVM.SessionSVI);
            }
            else if (Sessions.Items.Count == 1)
            {
                if (LeftSessionActive)
                {
                    SessionVM = new SessionViewModel(false, ActualWidth, ActualHeight, new Session(), IDs);
                    Sessions.Items.Add(SessionVM.SessionSVI);
                    RightSessionActive = true;
                }
                else
                {
                    SessionVM = new SessionViewModel(true, ActualWidth, ActualHeight, new Session(), IDs);
                    Sessions.Items.Add(SessionVM.SessionSVI);
                    LeftSessionActive = true;
                }
            }

            openedSessions.Add(SessionVM);
            SessionVM.Session.PlayBackgroundSound();
            CheckDesktopToDisplay();
        }

        /// <summary>
        /// Event handling the Create Session Button Click Action.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CreateDoubleSession_Button_Click(object sender, RoutedEventArgs e)
        {
            HideDesktop();
            SessionVM = new SessionViewModel(true, ActualWidth, ActualHeight, new Session(), IDs);
            Sessions.Items.Add(SessionVM.SessionSVI);
            openedSessions.Add(SessionVM);
            SessionVM.Session.PlayBackgroundSound();

            SessionVM = new SessionViewModel(false, ActualWidth, ActualHeight, new Session(), IDs);
            Sessions.Items.Add(SessionVM.SessionSVI);
            openedSessions.Add(SessionVM);
            SessionVM.Session.PlayBackgroundSound();

            LeftSessionActive = true;
            RightSessionActive = true;
            CheckDesktopToDisplay();
        }

        /// <summary>
        /// TODO
        /// </summary>
        public void CheckDesktopToDisplay()
        {
            if (Sessions.Items.Count == 0)
                DisplayFullDesktop();
            else if (Sessions.Items.Count == 1 && LeftSessionActive)
            {
                DisplayRightDesktop();
            }
            else if (Sessions.Items.Count == 1 && RightSessionActive)
            {
                DisplayLeftDesktop();
            }
            else
            {
                HideDesktop();
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        private void HideDesktop()
        {
            CreateSession_Button.Visibility = Visibility.Hidden;
            CreateDoubleSession_Button.Visibility = Visibility.Hidden;
            HidePhotos();
            if (LeftSessionActive) UnhideCache();
        }

        /// <summary>
        /// TODO
        /// </summary>
        private void DisplayFullDesktop()
        {
            CreateSession_Button.Visibility = Visibility.Visible;
            CreateDoubleSession_Button.Visibility = Visibility.Visible;
            UnhidePhotos();
            HideCache();

            Photos.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);
        }

        /// <summary>
        /// TODO
        /// </summary>
        private void DisplayRightDesktop()
        {
            CreateSession_Button.Visibility = Visibility.Visible;
            CreateDoubleSession_Button.Visibility = Visibility.Hidden;
            UnhidePhotos();
            UnhideCache();

            Photos.Margin = new Thickness(607.5, 0.0, 0.0, 0.0);
        }
        
        /// <summary>
        /// TODO
        /// </summary>
        private void DisplayLeftDesktop()
        {
            CreateSession_Button.Visibility = Visibility.Visible;
            CreateDoubleSession_Button.Visibility = Visibility.Hidden;
            UnhidePhotos();
            UnhideCache();

            Photos.Margin = new Thickness(0.0, 0.0, 607.5, 0.0);
        }

        /// <summary>
        /// TODO
        /// </summary>
        public void HidePhotos()
        {
            if (Photos.Opacity != 1.0) return;

            Storyboard OpacitySTB = new Storyboard();
            DoubleAnimation OpacityAnimation = new DoubleAnimation();

            OpacityAnimation.From = 1;
            OpacityAnimation.To = 0;
            OpacityAnimation.Duration = new Duration(TimeSpan.FromSeconds(.2));
            OpacityAnimation.FillBehavior = FillBehavior.HoldEnd;
            OpacitySTB.Children.Add(OpacityAnimation);
            Storyboard.SetTarget(OpacityAnimation, Photos);
            Storyboard.SetTargetProperty(OpacityAnimation, new PropertyPath(ScatterView.OpacityProperty));

            OpacitySTB.Begin();
        }

        /// <summary>
        /// TODO
        /// </summary>
        public void UnhidePhotos()
        {
            if (Photos.Opacity != 0.0) return;
            
            Storyboard OpacitySTB = new Storyboard();
            DoubleAnimation OpacityAnimation = new DoubleAnimation();

            OpacityAnimation.From = 0;
            OpacityAnimation.To = 1;
            OpacityAnimation.Duration = new Duration(TimeSpan.FromSeconds(.2));
            OpacityAnimation.FillBehavior = FillBehavior.HoldEnd;
            OpacitySTB.Children.Add(OpacityAnimation);
            Storyboard.SetTarget(OpacityAnimation, Photos);
            Storyboard.SetTargetProperty(OpacityAnimation, new PropertyPath(ScatterView.OpacityProperty));

            OpacitySTB.Begin();
        }

        /// <summary>
        /// TODO
        /// </summary>
        public void HideCache()
        {
            if (MiddleCacheGrid.Opacity != 1.0) return;

            Storyboard OpacitySTB = new Storyboard();
            DoubleAnimation OpacityAnimation = new DoubleAnimation();

            OpacityAnimation.From = 1;
            OpacityAnimation.To = 0;
            OpacityAnimation.Duration = new Duration(TimeSpan.FromSeconds(.2));
            OpacityAnimation.FillBehavior = FillBehavior.HoldEnd;
            OpacitySTB.Children.Add(OpacityAnimation);
            Storyboard.SetTarget(OpacityAnimation, MiddleCacheGrid);
            Storyboard.SetTargetProperty(OpacityAnimation, new PropertyPath(Grid.OpacityProperty));

            OpacitySTB.Begin();
        }

        /// <summary>
        /// TODO
        /// </summary>
        public void UnhideCache()
        {
            if (MiddleCacheGrid.Opacity != 0.0) return;

            Storyboard OpacitySTB = new Storyboard();
            DoubleAnimation OpacityAnimation = new DoubleAnimation();

            OpacityAnimation.From = 0;
            OpacityAnimation.To = 1;
            OpacityAnimation.Duration = new Duration(TimeSpan.FromSeconds(.2));
            OpacityAnimation.FillBehavior = FillBehavior.HoldEnd;
            OpacitySTB.Children.Add(OpacityAnimation);
            Storyboard.SetTarget(OpacityAnimation, MiddleCacheGrid);
            Storyboard.SetTargetProperty(OpacityAnimation, new PropertyPath(Grid.OpacityProperty));

            OpacitySTB.Begin();
        }

        public void DisplayPreviewZones(bool appear, bool fourZones)
        {
            Storyboard pGSTB = new Storyboard();
            DoubleAnimation previewGridAnimation = new DoubleAnimation();

            if (appear)
            {
                previewGridAnimation.From = sideZonesPreview.Opacity;
                previewGridAnimation.To = 0.2;
            }
            else
            {
                previewGridAnimation.From = sideZonesPreview.Opacity;
                previewGridAnimation.To = 0;
            }
            previewGridAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            previewGridAnimation.FillBehavior = FillBehavior.HoldEnd;
            pGSTB.Children.Add(previewGridAnimation);
            Storyboard.SetTarget(previewGridAnimation, sideZonesPreview);
            Storyboard.SetTargetProperty(previewGridAnimation, new PropertyPath(Grid.OpacityProperty));

            pGSTB.Begin();

            if (fourZones || !appear)
            {
                Storyboard pGSTB2 = new Storyboard();
                DoubleAnimation previewGridAnimation2 = new DoubleAnimation();

                if (appear)
                {
                    previewGridAnimation2.From = topZonesPreview.Opacity;
                    previewGridAnimation2.To = 0.2;
                }
                else
                {
                    previewGridAnimation2.From = topZonesPreview.Opacity;
                    previewGridAnimation2.To = 0;
                }
                previewGridAnimation2.Duration = new Duration(TimeSpan.FromSeconds(.5));
                previewGridAnimation2.FillBehavior = FillBehavior.HoldEnd;
                pGSTB2.Children.Add(previewGridAnimation2);
                Storyboard.SetTarget(previewGridAnimation2, topZonesPreview);
                Storyboard.SetTargetProperty(previewGridAnimation2, new PropertyPath(Grid.OpacityProperty));

                pGSTB2.Begin();
            }
        }

        private void AnimateSimpleSessionButton()
        {
            Random r = GlobalVariables.GlobalRandom;
            simpleButtonDirection = !simpleButtonDirection;
            Storyboard buttonsSTB = new Storyboard();
            ThicknessAnimation marginAnimation = new ThicknessAnimation();
            SineEase ease = new SineEase();
            ease.EasingMode = EasingMode.EaseInOut;
            Double yOffset;
            if (simpleButtonDirection) yOffset = (30.0 / 1920.0) * ActualWidth;
            else yOffset = -(30.0 / 1920.0) * ActualWidth;

            marginAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(r.Next(1500, 2000)));
            /*marginAnimation.AccelerationRatio = .3;
            marginAnimation.DecelerationRatio = .7;*/

            marginAnimation.From = CreateSession_Button.Margin;
            marginAnimation.To = new Thickness(0, 0, CreateSession_Button.Margin.Right, CreateSession_Button.Margin.Bottom + yOffset);
            marginAnimation.FillBehavior = FillBehavior.HoldEnd;
            marginAnimation.EasingFunction = ease;
            Storyboard.SetTarget(marginAnimation, CreateSession_Button);
            Storyboard.SetTargetProperty(marginAnimation, new PropertyPath(SurfaceButton.MarginProperty));

            buttonsSTB.Children.Add(marginAnimation);
            marginAnimation.Completed += new EventHandler(marginAnimation_Completed);

            buttonsSTB.Begin();
        }

        void marginAnimation_Completed(object sender, EventArgs e)
        {
            AnimateSimpleSessionButton();
        }

        private void AnimateDoubleSessionButton()
        {
            Random r = GlobalVariables.GlobalRandom;
            doubleButtonDirection = !doubleButtonDirection;
            Storyboard buttonsSTB = new Storyboard();
            ThicknessAnimation marginAnimation = new ThicknessAnimation();
            SineEase ease = new SineEase();
            ease.EasingMode = EasingMode.EaseInOut;
            Double yOffset;
            if (doubleButtonDirection) yOffset = (30.0 / 1920.0) * ActualWidth;
            else yOffset = -(30.0 / 1920.0) * ActualWidth;

            marginAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(r.Next(1500, 2000)));
            /*marginAnimation.AccelerationRatio = .3;
            marginAnimation.DecelerationRatio = .7;*/

            marginAnimation.From = CreateDoubleSession_Button.Margin;
            marginAnimation.To = new Thickness(CreateDoubleSession_Button.Margin.Left, CreateDoubleSession_Button.Margin.Top + yOffset, 0, 0);
            marginAnimation.FillBehavior = FillBehavior.HoldEnd;
            marginAnimation.EasingFunction = ease;
            Storyboard.SetTarget(marginAnimation, CreateDoubleSession_Button);
            Storyboard.SetTargetProperty(marginAnimation, new PropertyPath(SurfaceButton.MarginProperty));

            buttonsSTB.Children.Add(marginAnimation);
            marginAnimation.Completed += new EventHandler(doubleMarginAnimation_Completed);

            buttonsSTB.Begin();
        }

        void doubleMarginAnimation_Completed(object sender, EventArgs e)
        {
            AnimateDoubleSessionButton();
        }
    }
}
