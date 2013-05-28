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
using System.Windows.Input;

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
        public Grid Play { get; set; }

        /// <summary>
        /// Property.
        /// Theme Button.
        /// </summary>
        public Grid Theme_Button { get; set; }

        /// <summary>
        /// Property
        /// Boolean true if the stave is playing
        /// </summary>
        public Boolean IsPlaying { get; set; }

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

        /// <summary>
        /// Property
        /// The Tree on the StaveTop
        /// </summary>
        public TreeViewModel TreeUp { get; set; }

        /// <summary>
        /// Property
        /// The Tree on the StaveBottom
        /// </summary>
        public TreeViewModel TreeDown { get; set; }

        /// <summary>
        /// The Grid containing the circles for sound changing
        /// </summary>
        public ChangeSoundViewModel UpdateSound { get; set; }

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
            UpdateSound = new ChangeSoundViewModel(s);
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

            //TODO mettre dans SetDimensions
            displayTrees(new Thickness(0, 0, 0, 0), new Thickness(10,10,200,400));
            //else displayTrees(new Thickness(10, 75, 200, 90), new Thickness(10, 30, 200, 200));

            Reducer = new SurfaceButton();
            Reduced = false;
            Reducer.Width = 100;
            Reducer.Height = 25;
            Reducer.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            Reducer.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Reducer.Background = Brushes.Red;
            Reducer.Content = "Reduce !";
            
            Play = new Grid();
            Play.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Play.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            ImageBrush img = new ImageBrush();
            img.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/Theme" + session.ThemeID + "/playdrop.png", UriKind.Relative));
            Play.Background = img;
            Play.Visibility = Visibility.Visible;
            IsPlaying = false;

            Play.PreviewTouchDown += new EventHandler<TouchEventArgs>(Play_TouchDown);

            Theme_Button = new Grid();
            Theme_Button.Width = 351;
            Theme_Button.Height = 110;
            Theme_Button.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Theme_Button.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            ImageBrush theme_img = new ImageBrush();
            theme_img.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/UI_items/themes.png", UriKind.Relative));
            Theme_Button.Background = theme_img;
            Theme_Button.Visibility = Visibility.Visible;

            Theme_Button.PreviewTouchDown += new EventHandler<TouchEventArgs>(Theme_Button_TouchDown);

            SessionSVI.CanMove = false;
            SessionSVI.CanRotate = false;
            SessionSVI.CanScale = false;
            SessionSVI.ShowsActivationEffects = false;

            Grid.Children.Add(Bubbles);
            Grid.Children.Add(Notes);
            Grid.Children.Add(NbgVM.Grid);
            Grid.Children.Add(MbgVM.Grid);
            Grid.Children.Add(Play);
            Grid.Children.Add(Theme_Button);
            Grid.Children.Add(UpdateSound.Grid1);
            Grid.Children.Add(UpdateSound.Grid2);

            Grid.SetZIndex(Theme_Button, 5);
            Grid.SetZIndex(UpdateSound.Grid1, 4);
            Grid.SetZIndex(UpdateSound.Grid2, 4);
            Grid.SetZIndex(TreeUp.Grid, 3);
            Grid.SetZIndex(TreeDown.Grid, 3);
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

            //Size of SurfaceButton Play
            Play.Width = width / 11.0;
            Play.Height = height / 7.0;
   
            UpdateSound.Grid1.Width = width / 10;
            UpdateSound.Grid2.Width = width / 10;
            UpdateSound.Grid1.Height = height / 11;
            UpdateSound.Grid2.Height = height / 11;

            TreeUp.Grid.Width = width / 7;
            TreeUp.Grid.Height = width / 5;
            TreeDown.Grid.Width = width / 7;
            TreeDown.Grid.Height = width / 5;

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

            Grid.SetZIndex(TreeUp.Grid, 4);
            Grid.SetZIndex(TreeDown.Grid, 3);
        }

        private void Theme_Button_TouchDown(object sender, RoutedEventArgs e)
        {
            Grid g = new Grid();
            g.Background = new SolidColorBrush(Colors.Black);
            g.Opacity = 0.3;
            g.Visibility = Visibility.Visible;
        }

        private void Play_TouchDown(object sender, RoutedEventArgs e)
        {
            if (!IsPlaying)
            {
                session.StopBackgroundSound();

                session.StaveTop.PlayAllNotes();
                session.StaveBottom.PlayAllNotes();
                Play.Opacity = 0.5;
                IsPlaying = true;
            }
            else
            {
                session.PlayBackgroundSound();

                session.StaveTop.StopMusic();
                session.StaveBottom.StopMusic();
                Play.Opacity = 1;
                IsPlaying = false;
            }
        }

        /// <summary>
        /// Saves current session into binary file
        /// </summary>
        /// <param name="newBpm">Path of the savefile</param>
        public void SaveSession()
        {
            string path = "Sessions/sess"+SessionID+".bin";
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
        public void LoadSession()
        {
            string path = "Sessions/sess" + SessionID + ".bin";
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = File.Open(path, FileMode.Open);
            SessionData sd = (SessionData)formatter.Deserialize(stream);
            stream.Close();

            Session = new Session();
            Session.ThemeID = sd.ThemeID;

            switch (Session.ThemeID)
            {
                case 2: Session.Theme = new Theme2(); break;
                case 3: Session.Theme = new Theme3(); break;
                case 4: Session.Theme = new Theme4(); break;
                default: Session.Theme = new Theme1(); break;
            }
            Converter conv = new Converter();
            Session.StaveTop = new Stave(Session.Theme.InstrumentsTop[0], Session.Theme);
            Session.StaveBottom = new Stave(Session.Theme.InstrumentsBottom[0], Session.Theme);
            Session.ThemeID = sd.ThemeID;

            Grid.Background = (new ThemeViewModel(Session.Theme, this)).BackgroundImage;

            Bubbles = new ScatterView();
            Notes = new ScatterView();
            UpdateSound = new ChangeSoundViewModel(Session);
            NotesOnStave = new List<NoteViewModel>();

            NbgVM = new NoteBubbleGeneratorViewModel(Session.NoteBubbleGenerator, this);
            MbgVM = new MelodyBubbleGeneratorViewModel(Session.MelodyBubbleGenerator, this);

            Grid.Children.Add(Bubbles);
            Grid.Children.Add(Notes);
            Grid.Children.Add(Reducer);
            Grid.Children.Add(Play);
            Grid.Children.Add(UpdateSound.Grid1);
            Grid.Children.Add(UpdateSound.Grid2);
            Grid.Children.Add(TreeUp.Grid);
            Grid.Children.Add(TreeDown.Grid);
            Grid.Children.Add(NbgVM.Grid);
            Grid.Children.Add(MbgVM.Grid);
            Grid.Children.Add(Theme_Button);

            Grid.SetZIndex(Theme_Button, 5);
            Grid.SetZIndex(UpdateSound.Grid1, 4);
            Grid.SetZIndex(UpdateSound.Grid2, 4);
            Grid.SetZIndex(TreeUp.Grid, 3);
            Grid.SetZIndex(TreeDown.Grid, 3);
            Grid.SetZIndex(Bubbles, 2);
            Grid.SetZIndex(Notes, 1);
            Grid.SetZIndex(NbgVM.Grid, 0);
            Grid.SetZIndex(MbgVM.Grid, 0);
            
            double XCenter;
            foreach (Note note in sd.StaveTopNotes)
            {
                XCenter = note.Position * 60.0 + 120.0;
                NoteViewModel noteVM = new NoteViewModel(new Point(XCenter, conv.getCenterY(true, note)), note, Notes, this);
                Session.StaveTop.AddNote(note, note.Position);
                Notes.Items.Add(noteVM.SVItem);
                NotesOnStave.Add(noteVM);
            }

            foreach (Note note in sd.StaveBottomNotes)
            {
                XCenter = note.Position * 60.0 + 120.0;
                NoteViewModel noteVM = new NoteViewModel(new Point(XCenter, conv.getCenterY(false, note)), note, Notes, this);
                Session.StaveBottom.AddNote(note, note.Position);
                Notes.Items.Add(noteVM.SVItem);
                NotesOnStave.Add(noteVM);
            }

            SetDimensions(Grid.ActualWidth, Grid.ActualHeight);
        }

        public void EraseSession()
        {
            Bubbles = null;
            MbgVM = null;
            NbgVM = null;
            Notes = null;
            NotesOnStave = null;
            Session = null;
        }
    }
}
