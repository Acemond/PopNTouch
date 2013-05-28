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
        /// Property.
        /// TODO
        /// </summary>
        public Session Session { get; set; }

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
        /// List of NoteViewModel
        /// Contains all the NoteViewModel's on the screen
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
        /// ThemeViewModel of the SessionViewModel
        /// </summary>
        public ThemeViewModel ThemeVM { get; set; }
        /// <summary>
        /// Property.
        /// Temporary Reduce Button.
        /// </summary>
        public SurfaceButton Reducer { get; set; }

        /// <summary>
        /// Property.
        /// Play Button.
        /// </summary>
        public Grid Play_Button { get; set; }

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
        /// Property.
        /// Manager of Themes choice.
        /// </summary>
        public ThemeChooser ThemeChooser { get; set; }

        public SessionViewModel(Double width, Double height, Session s, List<int> IDs)
        {
            Session = s;
            SessionSVI = new ScatterViewItem();
            SessionSVI.Width = width;
            SessionSVI.Height = height;
            double ratio = width / 1920.0;
            ThemeVM = new ThemeViewModel(Session.Theme, this);
            Grid = new Grid();
            UpdateSound = new ChangeSoundViewModel(this);
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
            
            Reducer = new SurfaceButton();
            Reduced = false;
            Reducer.Width = 100;
            Reducer.Height = 25;
            Reducer.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            Reducer.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Reducer.Background = Brushes.Red;
            Reducer.Content = "Reduce !";

            Play_Button = new Grid();
            Play_Button.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Play_Button.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Play_Button.Background = ThemeVM.PlayImage;
            Play_Button.Visibility = Visibility.Visible;
            IsPlaying = false;

            Play_Button.PreviewTouchDown += new EventHandler<TouchEventArgs>(Play_Button_TouchDown);

            Theme_Button = new Grid();
            Theme_Button.Width = 351;
            Theme_Button.Height = 110;
            Theme_Button.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            Theme_Button.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Theme_Button.Margin = new Thickness(0, 0, 100.0, 0);
            Theme_Button.Background = ThemeVM.ThemesImage;
            Theme_Button.Visibility = Visibility.Visible;

            SessionSVI.CanMove = false;
            SessionSVI.CanRotate = false;
            SessionSVI.CanScale = false;
            SessionSVI.ShowsActivationEffects = false;

            displayTrees(new Thickness(20.0 * ratio, 0, 0, 130.0 * ratio), new Thickness(20.0 * ratio, 0, 0, 580.0 * ratio));

            Grid.Children.Add(Bubbles);
            Grid.Children.Add(Notes);
            Grid.Children.Add(NbgVM.Grid);
            Grid.Children.Add(MbgVM.Grid);
            Grid.Children.Add(Play_Button);
            Grid.Children.Add(Theme_Button);
            Grid.Children.Add(UpdateSound.Grid);

            Grid.SetZIndex(Theme_Button, 5);
            Grid.SetZIndex(UpdateSound.Grid, 4);
            Grid.SetZIndex(TreeUp.Grid, 3);
            Grid.SetZIndex(TreeDown.Grid, 3);
            Grid.SetZIndex(Bubbles, 2);
            Grid.SetZIndex(Notes, 1);
            Grid.SetZIndex(NbgVM.Grid, 0);
            Grid.SetZIndex(MbgVM.Grid, 0);

            SessionSVI.Content = Grid;

            ThemeChooser = new ThemeChooser(this);
            Theme_Button.PreviewTouchDown += new EventHandler<TouchEventArgs>(ThemeChooser.Theme_Button_TouchDown);

            int i = 1;
            while (IDs.Contains(i)) i++;
            SessionID = i;
            IDs.Add(i);

            SetDimensions(width, height);

            Animation = new SessionAnimation(this);
            Reducer.Click += new RoutedEventHandler(Animation.Reducer_Click);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="left"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="s"></param>
        /// <param name="IDs"></param>
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
        
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SetDimensions(Double width, Double height)
        {
            SessionSVI.Width = width;
            SessionSVI.Height = height;

            double ratio = width / 1920.0;

            NbgVM.Grid.Width = width / 8.0;
            NbgVM.Grid.Height = width * 0.07948;
            MbgVM.Grid.Width = width / 8.0;
            MbgVM.Grid.Height = width * 0.07948;

            ThemeChooser.SetDimensions(width);

            //Size of SurfaceButton Play
            Play_Button.Width = width / 11.0;
            Play_Button.Height = height / 7.0;

            Theme_Button.Width = (351.0 / 1920.0) * width;
            Theme_Button.Height = (110 / 1080.0) * height;

            TreeUp.Grid.Margin = new Thickness(20.0 * ratio, 0, 0, 130.0 * ratio);
            TreeDown.Grid.Margin = new Thickness(20.0 * ratio, 0, 0, 580.0 * ratio);
            
            SessionSVI.Width = width;
            SessionSVI.Height = height;
            SessionSVI.Center = new Point(width / 2.0, height / 2.0);
        }

        public void displayTrees(Thickness up, Thickness down)
        {
            TreeUp = new TreeViewModel(true, up, this);
            Grid.Children.Add(TreeUp.Grid);

            TreeDown = new TreeViewModel(false, down, this);
            Grid.Children.Add(TreeDown.Grid);

            Grid.SetZIndex(TreeUp.Grid, 4);
            Grid.SetZIndex(TreeDown.Grid, 3);
        }

        private void Play_Button_TouchDown(object sender, RoutedEventArgs e)
        {
            if (!IsPlaying)
            {
                Session.StopBackgroundSound();

                Session.StaveTop.PlayAllNotes();
                Session.StaveBottom.PlayAllNotes();
                Play_Button.Opacity = 0.5;
                IsPlaying = true;
            }
            else
            {
                Session.PlayBackgroundSound();

                Session.StaveTop.StopMusic();
                Session.StaveBottom.StopMusic();
                Play_Button.Opacity = 1;
                IsPlaying = false;
            }
        }

        /// <summary>
        /// Saves current session into binary file
        /// </summary>
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
        public void LoadSession()
        {
            //TODO : check : no problem with that ?
            SessionSVI.Width = Grid.ActualWidth;
            SessionSVI.Height = Grid.ActualHeight;
            //ODOT

            double ratio = SessionSVI.Width / 1920.0;

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
            UpdateSound = new ChangeSoundViewModel(this);
            NotesOnStave = new List<NoteViewModel>();

            NbgVM = new NoteBubbleGeneratorViewModel(Session.NoteBubbleGenerator, this);
            MbgVM = new MelodyBubbleGeneratorViewModel(Session.MelodyBubbleGenerator, this);

            displayTrees(new Thickness(20.0 * ratio, 0, 0, 130.0 * ratio), new Thickness(20.0 * ratio, 0, 0, 580.0 * ratio));

            Grid.Children.Add(Bubbles);
            Grid.Children.Add(Notes);
            Grid.Children.Add(Reducer);
            Grid.Children.Add(Play_Button);
            Grid.Children.Add(UpdateSound.Grid);
            Grid.Children.Add(NbgVM.Grid);
            Grid.Children.Add(MbgVM.Grid);
            Grid.Children.Add(Theme_Button);

            Grid.SetZIndex(Theme_Button, 5);
            Grid.SetZIndex(UpdateSound.Grid, 4);
            Grid.SetZIndex(TreeUp.Grid, 3);
            Grid.SetZIndex(TreeDown.Grid, 3);
            Grid.SetZIndex(Bubbles, 2);
            Grid.SetZIndex(Notes, 1);
            Grid.SetZIndex(NbgVM.Grid, 0);
            Grid.SetZIndex(MbgVM.Grid, 0);
            
            double XCenter;
            foreach (Note note in sd.StaveTopNotes)
            {
                XCenter = ((note.Position * 60.0 + 120.0) / 1920.0) * Grid.ActualWidth;
                NoteViewModel noteVM = new NoteViewModel(new Point(XCenter, (conv.getCenterY(true, note) / 1080.0) * Grid.ActualHeight), note, Notes, this);
                Session.StaveTop.AddNote(note, note.Position);
                Notes.Items.Add(noteVM.SVItem);
                NotesOnStave.Add(noteVM);
            }

            foreach (Note note in sd.StaveBottomNotes)
            {
                XCenter = ((note.Position * 60.0 + 120.0) / 1920.0) * Grid.ActualWidth;
                NoteViewModel noteVM = new NoteViewModel(new Point(XCenter, (conv.getCenterY(false, note) / 1080.0) * Grid.ActualHeight), note, Notes, this);
                Session.StaveBottom.AddNote(note, note.Position);
                Notes.Items.Add(noteVM.SVItem);
                NotesOnStave.Add(noteVM);
            }

            TreeDown.SetInstrument(sd.TopInstrument);
            TreeUp.SetInstrument(sd.BottomInstrument);

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
            TreeDown = null;
            TreeUp = null;
        }
    }
}
