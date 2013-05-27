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
        /// Property.
        /// TODO
        /// </summary>
        public ScatterView Photos { get; set; }

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
            Children.Add(Photos);
        }

        /// <summary>
        /// Event handling the Create Session Button Click Action.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CreateSession_Button_Click(object sender, RoutedEventArgs e)
        {
            SessionVM = new SessionViewModel(ActualWidth, ActualHeight, new Session(), IDs);
            Photos.Items.Add(SessionVM.SessionSVI);
        }

        /// <summary>
        /// Event handling the Create Session Button Click Action.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CreateDoubleSession_Button_Click(object sender, RoutedEventArgs e)
        {
            SessionVM = new SessionViewModel(ActualWidth, ActualHeight, new Session(), IDs);
            Photos.Items.Add(SessionVM.SessionSVI);
        }

    }
}
