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

        public Boolean LeftSessionActive { get; set; }

        public List<int> IDs { get; set; }

        /// <summary>
        /// temporary
        /// </summary>
        public SurfaceButton CreateDoubleSession_Button { get; set; }

        /// <summary>
        /// temporary
        /// </summary>
        public SurfaceButton CreateSession_Button { get; set; }
        
        /// <summary>
        /// Initializes few components including the session.
        /// </summary>
        public DesktopView()
        {
            IDs = new List<int>();

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
        }

        /// <summary>
        /// Event handling the Create Session Button Click Action.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CreateSession_Button_Click(object sender, RoutedEventArgs e)
        {
            CreateSession_Button.Visibility = Visibility.Hidden;
            CreateDoubleSession_Button.Visibility = Visibility.Hidden;

            if (Sessions.Items.Count == 0)
            {
                SessionVM = new SessionViewModel(ActualWidth, ActualHeight, new Session(), IDs);
                Sessions.Items.Add(SessionVM.SessionSVI);
            }
            else if (Sessions.Items.Count == 1)
            {
                if (LeftSessionActive)
                {
                    SessionVM = new SessionViewModel(false, ActualWidth, ActualHeight, new Session(), IDs);
                    Sessions.Items.Add(SessionVM.SessionSVI);
                }
                else
                {
                    SessionVM = new SessionViewModel(true, ActualWidth, ActualHeight, new Session(), IDs);
                    Sessions.Items.Add(SessionVM.SessionSVI);
                    LeftSessionActive = true;
                }
            }
        }

        /// <summary>
        /// Event handling the Create Session Button Click Action.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CreateDoubleSession_Button_Click(object sender, RoutedEventArgs e)
        {
            CreateSession_Button.Visibility = Visibility.Hidden;
            CreateDoubleSession_Button.Visibility = Visibility.Hidden;

            SessionVM = new SessionViewModel(true, ActualWidth, ActualHeight, new Session(), IDs);
            Sessions.Items.Add(SessionVM.SessionSVI);

            SessionVM = new SessionViewModel(false, ActualWidth, ActualHeight, new Session(), IDs);
            Sessions.Items.Add(SessionVM.SessionSVI);

            LeftSessionActive = true;
        }

        public void CheckDesktopToDisplay(Boolean left)
        {

        }

        public void HidePhotos()
        {
            if (Photos.Opacity != 1.0) return;

            CreateDoubleSession_Button.Visibility = Visibility.Hidden;
            CreateSession_Button.Visibility = Visibility.Hidden;

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

        public void UnhidePhotos()
        {
            if (Photos.Opacity != 0.0) return;

            CreateDoubleSession_Button.Visibility = Visibility.Visible;
            CreateSession_Button.Visibility = Visibility.Visible;

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
    }
}
