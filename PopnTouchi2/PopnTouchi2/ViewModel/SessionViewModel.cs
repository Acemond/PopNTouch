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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using PopnTouchi2.Model;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework.Audio;

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
        /// TODO
        /// </summary>
        public List<NoteViewModel> NotesOnStave { get; set; }

        /// <summary>
        /// Parameter.
        /// The SessionAnimation instance handling all animations related to the session.
        /// </summary>
        public SessionAnimation Animation { get; set; }

        /// <summary>
        /// Property.
        /// SVI containing the Grid that is containing the session.
        /// </summary>
        public ScatterViewItem SessionSVI { get; set; }

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
        /// Sets Current session's orientation
        /// Top, Bottom, Right, Left.
        /// </summary>
        public string Orientation { get; set; }

        /// <summary>
        /// Parameter.
        /// Session's number.
        /// </summary>
        public int SessionID { get; set; }

        public TreeViewModel TreeUp { get; set; }

        public TreeViewModel TreeDown { get; set; }

        /// <summary>
        /// SessionViewModel Construtor.
        /// Initializes all SessionViewModel components.
        /// </summary>
        /// <param name="s">The Session to link with its ViewModel</param>
        private SessionViewModel(Session s) : base()
        {
            Session = s;
            SessionSVI = new ScatterViewItem();
            Grid = new Grid();
            Bubbles = new ScatterView();
            Notes = new ScatterView();
            NotesOnStave = new List<NoteViewModel>();
            NbgVM = new NoteBubbleGeneratorViewModel(Session.NoteBubbleGenerator, this);
            MbgVM = new MelodyBubbleGeneratorViewModel(Session.MelodyBubbleGenerator, this);

            Orientation = "bottom";
            SessionSVI.Opacity = 0;
            Bubbles.Visibility = Visibility.Visible;
            Notes.Visibility = Visibility.Visible;
            
            Grid.Background = (new ThemeViewModel(Session.Theme, this)).BackgroundImage;
                 
            //TODO changer thickness bottom et top avec les résolutions plus grandes
            displayTrees(new Thickness(10, 50, 200, 90), new Thickness(10,10,200,400));

            Reducer = new SurfaceButton();
            Reduced = false;
            Reducer.Width = 100;
            Reducer.Height = 25;
            Reducer.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            Reducer.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Reducer.Background = Brushes.Red;
            Reducer.Content = "Reduce !";
            
            Play = new SurfaceButton();
            Play.Width = 100;
            Play.Height = 25;
            Play.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Play.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Play.Background = Brushes.Red;
            Play.Content = "Play";
            Play.Visibility = Visibility.Visible;

            Play.Click+=new RoutedEventHandler(Play_Click);

            Stop = new SurfaceButton();
            Stop.Width = 100;
            Stop.Height = 25;
            Stop.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            Stop.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Stop.Background = Brushes.Blue;
            Stop.Content = "Stop";
            Stop.Visibility = Visibility.Hidden;


            Stop.Click += new RoutedEventHandler(Stop_Click);
                        
            SessionSVI.CanMove = false;
            SessionSVI.CanRotate = false;
            SessionSVI.CanScale = false;
            SessionSVI.ShowsActivationEffects = false;

            Grid.Children.Add(Bubbles);
            Grid.Children.Add(Notes);
            Grid.Children.Add(NbgVM.Grid);
            Grid.Children.Add(MbgVM.Grid);
            Grid.Children.Add(Play);
            Grid.Children.Add(Stop);
            
            Grid.SetZIndex(Bubbles, 2);
            Grid.SetZIndex(Notes, 1);
            Grid.SetZIndex(NbgVM.Grid, 0);
            Grid.SetZIndex(MbgVM.Grid, 0);

            SessionSVI.Content = Grid;
        }

        public SessionViewModel(Double width, Double height, Session s, List<int> IDs)
            : this(s)
        {
            int i = 1;
            while (IDs.Contains(i)) i++;
            SessionID = i;
            IDs.Add(i);

            SetDimensions(width, height);

            Animation = new SessionAnimation(this);
            Reducer.Click += new RoutedEventHandler(Animation.Reducer_Click);
        }

        public SessionViewModel(Boolean left, Double width, Double height, Session s, List<int> IDs)
            : this(width * 0.5625, height * 0.5625, s, IDs)
        {
            if (left)
            {
                Orientation = "left";
                SessionSVI.Orientation = 90;
                SessionSVI.Center = new Point( width / 6.0 - 16.875, height / 2.0);
            }
            else
            {
                Orientation = "right";
                SessionSVI.Orientation = -90;
                SessionSVI.Center = new Point(width - (width / 6.0 - 16.875), height / 2.0);
            }
        }
        
        public void SetDimensions(Double width, Double height)
        {
            NbgVM.Grid.Width = width / 8.0;
            NbgVM.Grid.Height = width * 0.07948;
            MbgVM.Grid.Width = width / 8.0;
            MbgVM.Grid.Height = width * 0.07948;

            SessionSVI.Width = width;
            SessionSVI.Height = height;
            SessionSVI.Center = new Point(width / 2.0, height / 2.0);
        }

        private void displayTrees(Thickness up, Thickness down)
        {
            TreeUp = new TreeViewModel(true, up, session, session.Theme);
            Grid.Children.Add(TreeUp.Grid);

            TreeDown = new TreeViewModel(false, down, session, session.Theme);
            Grid.Children.Add(TreeDown.Grid);
        }


        private void Play_Click(object sender, RoutedEventArgs e)
        {
            session.StopBackgroundSound();

            session.StaveTop.PlayAllNotes();
            session.StaveBottom.PlayAllNotes();
            Play.Visibility = Visibility.Hidden;
            Stop.Visibility = Visibility.Visible;
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            session.PlayBackgroundSound();

            session.StaveTop.StopMusic();
            session.StaveBottom.StopMusic();
            Stop.Visibility = Visibility.Hidden;
            Play.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Saves current session into binary file
        /// </summary>
        /// <param name="newBpm">Path of the savefile</param>
        public void SaveSession(string path)
        {
            Stream stream = File.Open(path, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();

            SessionData sd = new SessionData(this);
            formatter.Serialize(stream, sd);
            stream.Close();
        }

        /// <summary>
        /// Loads a session from a binary file
        /// </summary>
        /// <param name="newBpm">Path of the savefile</param>
        public void LoadSession(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = File.Open(path, FileMode.Open);
            SessionData sd = (SessionData)formatter.Deserialize(stream);
            stream.Close();

            switch (Session.ThemeID)
            {
                case 2: Session.Theme = new Theme2(); break;
                case 3: Session.Theme = new Theme3(); break;
                case 4: Session.Theme = new Theme4(); break;
                default: Session.Theme = new Theme1(); break;
            }

            Session.StaveTop = new Stave(true, Session.Theme.InstrumentsTop[0], Session.Theme);
            Session.StaveBottom = new Stave(false, Session.Theme.InstrumentsBottom[0], Session.Theme);
            Session.StaveTop.Notes = sd.StaveTopNotes;
            Session.StaveBottom.Notes = sd.StaveBottomNotes;
            //Notes = sd.NotesSV;
            Session.ThemeID = sd.ThemeID;
        }
    }
}
