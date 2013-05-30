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
using PopnTouchi2.Model.Enums;
using System.Windows.Shapes;

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
        /// Play Button.
        /// </summary>
        public Grid Play_Button { get; set; }

        /// <summary>
        /// Property.
        /// Theme Button.
        /// </summary>
        public Grid Theme_Button { get; set; }

        /// <summary>
        /// Property.
        /// Tempo Button.
        /// </summary>
        public Grid Tempo_Button { get; set; }

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
        /// Preview Grid to ease note placement
        /// </summary>
        public Grid previewGrid { get; set; }

        /// <summary>
        /// Preview Grid to ease note placement
        /// </summary>
        public Grid topStaveHighlight { get; set; }

        /// <summary>
        /// Preview Grid to ease note placement
        /// </summary>
        public Grid bottomStaveHighlight { get; set; }

        /// <summary>
        /// The Grid containing the circles for sound changing
        /// </summary>
        public ChangeSoundViewModel UpdateSound { get; set; }

        /// <summary>
        /// Delete Button
        /// </summary>
        public SurfaceButton DeleteButton { get; set; }

        private Thread play;

        public bool BeingDeleted { get; set; }
        public bool removeDeleteButtonsOnTouchUp { get; set; }
        public bool FullyEnlarged { get; set; }
        public bool InitialScale { get; set; }
        private double ratio;
        public double originalRatio { get; set; }

        /// <summary>
        /// Property.
        /// Manager of Themes choice.
        /// </summary>
        public ThemeChooser ThemeChooser { get; set; }

        public SessionViewModel(Double width, Double height, Session s, List<int> IDs, bool animated)
        {
            Session = s;
            BeingDeleted = false;
            InitialScale = true;
            removeDeleteButtonsOnTouchUp = false;
            SessionSVI = new ScatterViewItem();
            SessionSVI.Width = width;
            SessionSVI.Height = height;

            ratio = width / 1920.0;
            originalRatio = width / 1920.0;

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
            
            Play_Button = new Grid();
            Play_Button.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Play_Button.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Play_Button.Background = ThemeVM.PlayImage;
            Play_Button.Visibility = Visibility.Visible;
            IsPlaying = false;

            previewGrid = new Grid();
            ImageBrush previewGridImage = new ImageBrush();
            previewGridImage.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/ui_items/previewGrid.png", UriKind.Relative));
            previewGrid.Background = previewGridImage;
            previewGrid.Opacity = 0;
            previewGrid.Margin = new Thickness(150.0 * ratio, 90.0 * ratio, 90.0 * ratio, 480.0 * ratio);

            topStaveHighlight = new Grid();
            ImageBrush tSHImage = new ImageBrush();
            tSHImage.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/ui_items/blackStaveHighlight.png", UriKind.Relative));
            topStaveHighlight.Background = tSHImage;
            topStaveHighlight.Opacity = 0;
            topStaveHighlight.Margin = new Thickness(0.0, 60.0 * ratio, 0.0, 480.0 * ratio);

            bottomStaveHighlight = new Grid();
            ImageBrush bSHImage = new ImageBrush();
            bSHImage.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/ui_items/whiteStaveHighlight.png", UriKind.Relative));
            bottomStaveHighlight.Background = bSHImage;
            bottomStaveHighlight.Opacity = 0;
            bottomStaveHighlight.Margin = new Thickness(0.0, 60.0 * ratio, 0.0, 480.0 * ratio);

            Play_Button.PreviewTouchDown += new EventHandler<TouchEventArgs>(Play_Button_TouchDown);

            Theme_Button = new Grid();
            Theme_Button.Width = 351;
            Theme_Button.Height = 110;
            Theme_Button.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            Theme_Button.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Theme_Button.Margin = new Thickness(0, 0, 100.0 * ratio, 0);
            Theme_Button.Background = ThemeVM.ThemesImage;
            Theme_Button.Visibility = Visibility.Visible;

            Tempo_Button = new Grid();
            Tempo_Button.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            Tempo_Button.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Tempo_Button.Margin = new Thickness(0, 0, 470.0 * ratio, 0);
            Tempo_Button.Background = ThemeVM.TempoImage[1];
            Tempo_Button.Visibility = Visibility.Visible;
            Tempo_Button.PreviewTouchDown += new EventHandler<TouchEventArgs>(Tempo_Button_TouchDown);

            SessionSVI.CanMove = false;
            SessionSVI.CanRotate = false;
            //SessionSVI.CanScale = false;
            SessionSVI.ShowsActivationEffects = false;

            displayTrees(new Thickness(20.0 * ratio, 0, 0, 130.0 * ratio), new Thickness(20.0 * ratio, 0, 0, 580.0 * ratio));

            Grid.Children.Add(Bubbles);
            Grid.Children.Add(Notes);
            Grid.Children.Add(NbgVM.Grid);
            Grid.Children.Add(MbgVM.Grid);
            Grid.Children.Add(Play_Button);
            Grid.Children.Add(Tempo_Button);
            Grid.Children.Add(UpdateSound.Grid);
            Grid.Children.Add(previewGrid);
            Grid.Children.Add(topStaveHighlight);
            Grid.Children.Add(bottomStaveHighlight);

            Grid.SetZIndex(UpdateSound.Grid, 4);
            Grid.SetZIndex(TreeUp.Grid, 3);
            Grid.SetZIndex(TreeDown.Grid, 3);
            Grid.SetZIndex(Bubbles, 2);
            Grid.SetZIndex(Notes, 1);
            Grid.SetZIndex(NbgVM.Grid, 0);
            Grid.SetZIndex(MbgVM.Grid, 0);
            Grid.SetZIndex(previewGrid, 0);
            
            SessionSVI.Content = Grid;

            ThemeChooser = new ThemeChooser(this);
            Theme_Button.PreviewTouchDown += new EventHandler<TouchEventArgs>(ThemeChooser.Theme_Button_TouchDown);

            int i = 1;
            while (IDs.Contains(i)) i++;
            SessionID = i;
            IDs.Add(i);

            SetDimensions(width, height);
            SessionSVI.Center = new Point(width / 2.0, height / 2.0);

            if (animated) Animation = new SessionAnimation(this);

            DeleteButton = new SurfaceButton();
            DeleteButton.Visibility = Visibility.Hidden;
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/ui_items/delete-icon.png", UriKind.Relative));
            DeleteButton.Background = ib;
            DeleteButton.Width = 25.0;
            DeleteButton.Height = 25.0;
            DeleteButton.HorizontalAlignment = HorizontalAlignment.Left;
            DeleteButton.VerticalAlignment = VerticalAlignment.Top;
            Grid.Children.Add(DeleteButton);

            Grid.SetZIndex(DeleteButton, 1000);

            DeleteButton.PreviewTouchDown += new EventHandler<TouchEventArgs>(DeleteButton_PreviewTouchDown);

            SessionSVI.SizeChanged += new SizeChangedEventHandler(SessionSVI_SizeChanged);
            SessionSVI.PreviewTouchUp += new EventHandler<TouchEventArgs>(SessionSVI_TouchLeave);
        }

        void SessionSVI_TouchLeave(object sender, TouchEventArgs e)
        {
            if (!FullyEnlarged) return;
            if (ratio != originalRatio)
                UpdateEveryDimensions(originalRatio * 1920.0, originalRatio * 1080.0);
        }

        void SessionSVI_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (InitialScale) UpdateEveryDimensions(Grid.ActualWidth, Grid.ActualHeight);
            if (!FullyEnlarged) return;
            UpdateEveryDimensions(Grid.ActualWidth, Grid.ActualHeight);
            if (ratio < originalRatio * 0.9)
            {
                UpdateEveryDimensions(originalRatio * 0.9 * 1920.0, originalRatio * 0.9 * 1080.0);
                SessionSVI.CanScale = false;
                Animation.Reduce();
            }
            if (ratio == originalRatio) Animation.resumeAllBubblesAnimations();
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
            : this(width * 0.5625, height * 0.5625, s, IDs, true)
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
        /// Loads a session from HDD
        /// </summary>
        /// <param name="width">MainDesktop width</param>
        /// <param name="height">MainDesktop height</param>
        /// <param name="s">Its session</param>
        /// <param name="ID">Its original ID</param>
        public SessionViewModel(Double width, Double height, Session s, List<int> IDs, int ID)
            : this(width, height, s, IDs, false)
        {
            InitialScale = false;
            SessionID = ID;
            Grid.Children.Remove(Bubbles);
            Grid.Children.Remove(UpdateSound.Grid);
            Grid.Children.Remove(Theme_Button);
            Grid.Children.Remove(Notes);
            Grid.Children.Remove(NbgVM.Grid);
            Grid.Children.Remove(MbgVM.Grid);
            Grid.Children.Remove(TreeUp.Grid);
            Grid.Children.Remove(TreeDown.Grid);
            Grid.Children.Remove(Play_Button);
            Grid.Children.Remove(Tempo_Button);
            EraseSession();

            SessionSVI.CanMove = true;
            SessionSVI.CanRotate = true;

            SessionSVI.Opacity = 1;
        }

        void DeleteButton_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            DeleteButton.Visibility = Visibility.Hidden;
            Animation.DeleteSession();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void UpdateEveryDimensions(Double width, Double height)
        {
            Animation.stopAllBubblesAnimations();
            double oldRatio = ratio;
            SetDimensions(width, height);
            double newRatio = width / 1920.0;
            UpdateSound.UpdateDimensions(newRatio);
            TreeUp.UpdateDimensions(newRatio);
            TreeDown.UpdateDimensions(newRatio);
            Notes.Width = Grid.ActualWidth;
            Notes.Height = Grid.ActualHeight;
            Bubbles.Width = Grid.ActualWidth;
            Bubbles.Height = Grid.ActualHeight;
            Tempo_Button.Margin = new Thickness(0, 0, 470.0 * ratio, 0);
            Theme_Button.Margin = new Thickness(0, 0, 100.0 * ratio, 0);
            previewGrid.Margin = new Thickness(150.0 * ratio, 90.0 * ratio, 90.0 * ratio, 480.0 * ratio);
            topStaveHighlight.Margin = new Thickness(0.0, 60.0 * ratio, 0.0, 480.0 * ratio);
            bottomStaveHighlight.Margin = new Thickness(0.0, 60.0 * ratio, 0.0, 480.0 * ratio);
            foreach (ScatterViewItem svi in Notes.Items)
            {
                ScaleTransform st = new ScaleTransform(ratio / originalRatio, ratio / originalRatio, svi.ActualCenter.X, svi.ActualCenter.Y);
                svi.LayoutTransform = st;

                svi.Width = (svi.ActualWidth / oldRatio) * ratio;
                svi.Height = (svi.ActualHeight / oldRatio) * ratio;

                Point oldCenter = svi.Center;
                Point newCenter = new Point((oldCenter.X / oldRatio) * ratio, (oldCenter.Y / oldRatio) * ratio);
                svi.Center = newCenter;
            }
            foreach (ScatterViewItem svi in Bubbles.Items)
            {
                ScaleTransform st = new ScaleTransform(ratio / originalRatio, ratio / originalRatio, svi.ActualCenter.X, svi.ActualCenter.Y);
                svi.LayoutTransform = st;

                svi.Width = (svi.ActualWidth / oldRatio) * ratio;
                svi.Height = (svi.ActualHeight / oldRatio) * ratio;

                Point oldCenter = svi.Center;
                Point newCenter = new Point((oldCenter.X / oldRatio) * ratio, (oldCenter.Y / oldRatio) * ratio);
                svi.Center = newCenter;
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
            
            ratio = width / 1920.0;

            NbgVM.Grid.Width = width / 8.0;
            NbgVM.Grid.Height = width * 0.07948;
            MbgVM.Grid.Width = width / 8.0;
            MbgVM.Grid.Height = width * 0.07948;

            ThemeChooser.SetDimensions(width);

            Tempo_Button.Margin = new Thickness(0, 0, 470.0 * ratio, 0);
            Theme_Button.Margin = new Thickness(0, 0, 100.0 * ratio, 0);
            previewGrid.Margin = new Thickness(150.0 * ratio, 90.0 * ratio, 90.0 * ratio, 480.0 * ratio);
            topStaveHighlight.Margin = new Thickness(0.0, 60.0 * ratio, 0.0, 480.0 * ratio);
            bottomStaveHighlight.Margin = new Thickness(0.0, 60.0 * ratio, 0.0, 480.0 * ratio);

            //Size of SurfaceButton Play
            Play_Button.Width = (140.0 / 1920.0) * width;
            Play_Button.Height = (150.0 / 1080) * height;

            Theme_Button.Width = (351.0 / 1920.0) * width;
            Theme_Button.Height = (110 / 1080.0) * height;

            Tempo_Button.Width = (150.0 / 1920.0) * width;
            Tempo_Button.Height = (70.0 / 1080.0) * height;

            TreeUp.UpdateDimensions(ratio);
            TreeDown.UpdateDimensions(ratio);

            UpdateSound.UpdateDimensions(ratio);
            
            SessionSVI.Width = width;
            SessionSVI.Height = height;
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
            play = new Thread(PlayStaves);
            if (!IsPlaying)
            {
                Session.StopBackgroundSound();              
                play.Start();

                Play_Button.Opacity = 0.5;
                IsPlaying = true;
            }
            else
            {
                StopSound();
            }
        }

        public void StopSound()
        {
            try { play.Abort(); } catch (Exception exc) { }
            Session.StaveTop.StopMusic();
            Session.StaveBottom.StopMusic();
            Play_Button.Opacity = 1;
            IsPlaying = false;
            Session.PlayBackgroundSound();
        }

        private void PlayStaves()
        {
            int timeTop = Session.StaveTop.PlayAllNotes();
            int timeDown = Session.StaveBottom.PlayAllNotes();
            Thread.Sleep(Math.Max(timeTop,timeDown));

            UpdatePlay();   
        }

        void UpdateControl()
        {
            Session.StaveTop.StopMusic();
            Session.StaveBottom.StopMusic();
            Play_Button.Opacity = 1;
            IsPlaying = false;
            Session.PlayBackgroundSound();
            
        }

        private void UpdatePlay(){
            try
            {
                Play_Button.Dispatcher.BeginInvoke((Action)UpdateControl, null);
            }
            catch (Exception exc) { }
        }


        private void Tempo_Button_TouchDown(object sender, RoutedEventArgs e)
        {
            if (Session.Bpm == 60)
            {
                Session.ChangeBpm(90);
                Tempo_Button.Background = ThemeVM.TempoImage[1];
            }
            else if (Session.Bpm == 90)
            {
                Session.ChangeBpm(120);
                Tempo_Button.Background = ThemeVM.TempoImage[2];
            }
            else
            {
                Session.ChangeBpm(60);
                Tempo_Button.Background = ThemeVM.TempoImage[0];
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
        /// Loads a session from a binary file reduced
        /// </summary>
        public void LoadReducedSession(FileStream ScStream, DesktopView desktop)
        {
            SessionSVI.BorderBrush = Brushes.White;
            SessionSVI.BorderThickness = new Thickness(15.0);

            SessionSVI.Width = SessionSVI.Width / 4.0 + 30.0;
            SessionSVI.Height = SessionSVI.Height / 4.0 + 30.0;

            Animation = new SessionAnimation(this, true, desktop);
            Animation.Fs = ScStream;
            Reduced = true;

            SessionSVI.CanScale = false;
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

            ratio = Grid.ActualWidth / 1920.0;

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

            ThemeVM = new ThemeViewModel(Session.Theme, this);
            Converter conv = new Converter();
            Session.StaveTop = new Stave(Session.Theme.InstrumentsTop[0]);
            Session.StaveBottom = new Stave(Session.Theme.InstrumentsBottom[0]);

            Grid.Background = (new ThemeViewModel(Session.Theme, this)).BackgroundImage;

            Bubbles = new ScatterView();
            Notes = new ScatterView();
            UpdateSound = new ChangeSoundViewModel(this);
            NotesOnStave = new List<NoteViewModel>();

            NbgVM = new NoteBubbleGeneratorViewModel(Session.NoteBubbleGenerator, this);
            MbgVM = new MelodyBubbleGeneratorViewModel(Session.MelodyBubbleGenerator, this);

            displayTrees(new Thickness(20.0 * ratio, 0, 0, 130.0 * ratio), new Thickness(20.0 * ratio, 0, 0, 580.0 * ratio));

            Play_Button = new Grid();
            Play_Button.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Play_Button.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Play_Button.Background = ThemeVM.PlayImage;
            Play_Button.Visibility = Visibility.Visible;
            IsPlaying = false;

            Play_Button.PreviewTouchDown += new EventHandler<TouchEventArgs>(Play_Button_TouchDown);

            switch (Session.ThemeID)
            {
                case 2: Grid.Children.Add(ThemeChooser.Bird); break;
                case 3: Grid.Children.Add(ThemeChooser.Dragon); break;
                case 4: Grid.Children.Add(ThemeChooser.Cat); break;
                default: break;
            }
            Grid.Children.Add(Bubbles);
            Grid.Children.Add(Notes);
            Grid.Children.Add(Play_Button);
            Grid.Children.Add(UpdateSound.Grid);
            Grid.Children.Add(NbgVM.Grid);
            Grid.Children.Add(MbgVM.Grid);
            Grid.Children.Add(Theme_Button);
            Grid.Children.Add(Tempo_Button);

            Grid.SetZIndex(Theme_Button, 5);
            Grid.SetZIndex(UpdateSound.Grid, 4);
            Grid.SetZIndex(TreeUp.Grid, 3);
            Grid.SetZIndex(TreeDown.Grid, 3);
            Grid.SetZIndex(Bubbles, 2);
            Grid.SetZIndex(Notes, 1);
            Grid.SetZIndex(NbgVM.Grid, 0);
            Grid.SetZIndex(MbgVM.Grid, 0);
            Grid.SetZIndex(ThemeChooser.Bird, 0);
            Grid.SetZIndex(ThemeChooser.Dragon, 0);
            Grid.SetZIndex(ThemeChooser.Cat, 0);

            Theme_Button.Background = ThemeVM.ThemesImage;

            double XCenter;
            foreach (Note note in sd.StaveTopNotes)
            {
                XCenter = ((note.Position * 60.0 + 120.0) / 1920.0) * Grid.ActualWidth;
                Note newNote = new Note(note);
                NoteViewModel noteVM = new NoteViewModel(new Point(XCenter, (conv.getCenterY(true, note) / 1080.0) * Grid.ActualHeight), newNote, Notes, this);
                Session.StaveTop.AddNote(newNote, newNote.Position);
                Notes.Items.Add(noteVM.SVItem);
                NotesOnStave.Add(noteVM);
            }

            foreach (Note note in sd.StaveBottomNotes)
            {
                XCenter = ((note.Position * 60.0 + 120.0) / 1920.0) * Grid.ActualWidth;
                Note newNote = new Note(note);
                NoteViewModel noteVM = new NoteViewModel(new Point(XCenter, (conv.getCenterY(false, note) / 1080.0) * Grid.ActualHeight), newNote, Notes, this);
                Session.StaveBottom.AddNote(newNote, newNote.Position);
                Notes.Items.Add(noteVM.SVItem);
                NotesOnStave.Add(noteVM);
                Session.StaveBottom.AddNote(noteVM.Note, noteVM.Note.Position);
            }

            TreeDown.SetInstrument(sd.TopInstrument);
            TreeUp.SetInstrument(sd.BottomInstrument);
            Session.PlayBackgroundSound();

            SetDimensions(Grid.ActualWidth, Grid.ActualHeight);
            //UpdateEveryDimensions(Grid.ActualWidth, Grid.ActualHeight);

            Session.ChangeBpm(sd.bpm);
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
            ThemeVM = null;
            Play_Button = null;
        }

        public void DeleteSession()
        {
            try
            {
                File.Delete("Sessions/sess" + SessionID + ".bin");
                Animation.Fs.Close();
                File.Delete("SnapShots/sc" + SessionID + ".jpg");
            }
            catch (Exception exc)
            {

            }
        }
    }
}
