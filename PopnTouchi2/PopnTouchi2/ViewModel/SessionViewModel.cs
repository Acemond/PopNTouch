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
        /// Parameter.
        /// True if the session is reduced on the background.
        /// </summary>
        public bool Reduced { get; set; }

        /// <summary>
        /// SessionViewModel Construtor.
        /// TODO
        /// </summary>
        /// <param name="session"></param>
        public SessionViewModel(Session s) : base()
        {
            SessionVM = this;
            Session = s;
            Grid.Opacity = 0;

            Grid = new Grid();
            Bubbles = new ScatterView();
            Bubbles.Visibility = Visibility.Visible;
            Grid.Children.Add(Bubbles);

            NbgVM = new NoteBubbleGeneratorViewModel(Session.NoteBubbleGenerator, this);
            MbgVM = new MelodyBubbleGeneratorViewModel(Session.MelodyBubbleGenerator, this);
            Grid.Children.Add(NbgVM.Grid);
            Grid.Children.Add(MbgVM.Grid);

            Animation = new SessionAnimation(this);

            switch (s.ThemeID)
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

            Reducer.Click += new RoutedEventHandler(Animation.Reducer_Click);
        }

        
    }
}
