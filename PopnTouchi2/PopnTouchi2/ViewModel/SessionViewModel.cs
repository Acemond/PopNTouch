using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Infrastructure;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using PopnTouchi2.ViewModel.Animation;
using System.Windows;
using System.Windows.Controls;
using System.Threading;

namespace PopnTouchi2.ViewModel
{
    /// <summary>
    /// Binds Session's properties to the View.
    /// </summary>
    public class SessionViewModel : ViewModelBase
    {
        /// <summary>
        /// Parameter.
        /// Session's element from the Model.
        /// </summary>
        private Session session;
        /// <summary>
        /// Property.
        /// TODO
        /// </summary>
        public Session Session
        {
            get
            {
                return session;
            }
            set
            {
                session = value;
                NotifyPropertyChanged("Session");
            }
        }
        /// <summary>
        /// Property.
        /// Session's Bubbles' ScatterView instance.
        /// </summary>
        public ScatterView Bubbles { get; set; }

        /// <summary>
        /// Property.
        /// Session's Notes' ScatterView instance.
        /// </summary>
        public ScatterView Notes { get; set; }

        /// <summary>
        /// Parameter.
        /// The SessionAnimation instance handling all animations related to the session.
        /// </summary>
        public SessionAnimation Animation { get; set; }

        /// <summary>
        /// Property.
        /// Grid containing the session.
        /// </summary>
        public Grid Grid { get; set; }

        /// <summary>
        /// Property.
        /// ViewModel of the MelodyBubbleGenerator attached to the session.
        /// </summary>
        public MelodyBubbleGeneratorViewModel MbgVM { get; set; }

        /// <summary>
        /// Property.
        /// ViewModel of the NoteBubbleGenerator attached to the session.
        /// </summary>
        public NoteBubbleGeneratorViewModel NbgVM { get; set; }

        /// <summary>
        /// Property.
        /// Temporary Reduce Button.
        /// </summary>
        public SurfaceButton Reducer { get; set; }

        /// <summary>
        /// Property.
        /// Play Button.
        /// </summary>
        public SurfaceButton Play { get; set; }

        /// <summary>
        /// Property.
        /// Stop Button.
        /// </summary>
        public SurfaceButton Stop { get; set; }

        /// <summary>
        /// Parameter.
        /// True if the session is reduced on the background.
        /// </summary>
        public bool Reduced { get; set; }

        /// <summary>
        /// SessionViewModel Construtor.
        /// Initializes all SessionViewModel components.
        /// </summary>
        /// <param name="s">The Session to link with its ViewModel</param>
        public SessionViewModel(Session s) : base()
        {
            SessionVM = this;
            Session = s;
            Grid = new Grid();
            Bubbles = new ScatterView();
            Notes = new ScatterView();
            NbgVM = new NoteBubbleGeneratorViewModel(Session.NoteBubbleGenerator, this);
            MbgVM = new MelodyBubbleGeneratorViewModel(Session.MelodyBubbleGenerator, this);

            Grid.Opacity = 0;
            Bubbles.Visibility = Visibility.Visible;
            Notes.Visibility = Visibility.Visible;
            Grid.Children.Add(Bubbles);
            Grid.Children.Add(Notes);

            Grid.Children.Add(NbgVM.Grid);
            Grid.Children.Add(MbgVM.Grid);

            switch (Session.ThemeID)
            {
                case 1:
                    Grid.Background = (new Theme1ViewModel(Session.Theme, this)).BackgroundImage;
                    break;
                case 2:
                    Grid.Background = (new Theme2ViewModel(Session.Theme, this)).BackgroundImage;
                    break;
                case 3:
                    Grid.Background = (new Theme3ViewModel(Session.Theme, this)).BackgroundImage;
                    break;
                case 4:
                    Grid.Background = (new Theme4ViewModel(Session.Theme, this)).BackgroundImage;
                    break;
            }

            Reducer = new SurfaceButton();
            Reduced = false;
            Reducer.Width = 100;
            Reducer.Height = 25;
            Reducer.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            Reducer.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Reducer.Background = Brushes.Red;
            Reducer.Content = "Reduce !";
            Grid.Children.Add(Reducer);

            Animation = new SessionAnimation(this);

            Reducer.Click += new RoutedEventHandler(Animation.Reducer_Click);

            Play = new SurfaceButton();
            Play.Width = 100;
            Play.Height = 25;
            Play.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Play.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Play.Background = Brushes.Red;
            Play.Content = "Play";
            Grid.Children.Add(Play);
            Play.Visibility = Visibility.Visible;

            Play.Click+=new RoutedEventHandler(Play_Click);

            Stop = new SurfaceButton();
            Stop.Width = 100;
            Stop.Height = 25;
            Stop.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            Stop.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Stop.Background = Brushes.Blue;
            Stop.Content = "Stop";
            Grid.Children.Add(Stop);
            Stop.Visibility = Visibility.Hidden;

            Stop.Click += new RoutedEventHandler(Stop_Click);
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
          //  AudioController.FadeOutBackgroundSound();
            session.StaveTop.PlayAllNotes();
            session.StaveBottom.PlayAllNotes();
            Play.Visibility = Visibility.Hidden;
            Stop.Visibility = Visibility.Visible;
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
        //    AudioController.FadeInBackgroundSound();
            session.StaveTop.StopMusic();
            session.StaveBottom.StopMusic();
            Stop.Visibility = Visibility.Hidden;
            Play.Visibility = Visibility.Visible;
        }
    }
}
