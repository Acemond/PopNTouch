using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PopnTouchi2.Infrastructure;
using PopnTouchi2.ViewModel.Animation;
using PopnTouchi2.Model.Enums;

namespace PopnTouchi2.ViewModel
{
    /// <summary>
    /// Defines all graphic items linked to a MelodyBubble.
    /// </summary>
    public class MelodyBubbleViewModel : ViewModelBase
    {
        /// <summary>
        /// Parameter.
        /// </summary>
        private MelodyBubble melodyBubble;
        /// <summary>
        /// Property.
        /// MelodyBubble element from the Model.
        /// </summary>
        public MelodyBubble MelodyBubble
        {
            get
            {
                return melodyBubble;
            }
            set
            {
                melodyBubble = value;
                NotifyPropertyChanged("MelodyBubble");
            }
        }
        /// <summary>
        /// Property.
        /// The parent ScatterView.
        /// </summary>
        public ScatterView ParentSV { get; set; }
        /// <summary>
        /// Property.
        /// The ScatterViewItem containing the notebubble.
        /// </summary>
        public ScatterViewItem SVItem { get; set; }
        /// <summary>
        /// Parameter.
        /// The MelodyBubbleAnimation item handling all animations for the melodyBubble.
        /// </summary>
        public MelodyBubbleAnimation Animation { get; set; }


         /// <summary>
        /// NoteBubbleViewModel Constructor.
        /// TODO
        /// </summary>
        public MelodyBubbleViewModel(MelodyBubble mb, ScatterView sv, SessionViewModel s) : base(s)
        {
            MelodyBubble = mb;
            SVItem = new ScatterViewItem();
            ParentSV = sv;
            
            Random r = new Random();
            SVItem.Center = new Point(r.Next((int)sv.ActualWidth), r.Next((int)(635 * sv.ActualHeight / 1080), (int)sv.ActualHeight));

            SVItem.CanScale = false;
            SVItem.HorizontalAlignment = HorizontalAlignment.Center;
            SVItem.CanRotate = false;
            SVItem.HorizontalAlignment = HorizontalAlignment.Center;

            FrameworkElementFactory bubbleImage = new FrameworkElementFactory(typeof(Image));
 
            bubbleImage.SetValue(Image.SourceProperty, new ThemeViewModel(SessionVM.Session.Theme, SessionVM).GetMelodyBubbleImageSource(mb.Melody.gesture));

            bubbleImage.SetValue(Image.IsHitTestVisibleProperty, false);

            bubbleImage.SetValue(Image.WidthProperty, (135.0 / 1920.0) * SessionVM.SessionSVI.ActualWidth);
            bubbleImage.SetValue(Image.HeightProperty, (135.0 / 1080.0) * SessionVM.SessionSVI.ActualHeight);

            FrameworkElementFactory touchZone = new FrameworkElementFactory(typeof(Ellipse));
            touchZone.SetValue(Ellipse.FillProperty, Brushes.Transparent);
            touchZone.SetValue(Ellipse.MarginProperty, new Thickness(15));

            FrameworkElementFactory grid = new FrameworkElementFactory(typeof(Grid));
            grid.AppendChild(bubbleImage);
            grid.AppendChild(touchZone);

            ControlTemplate ct = new ControlTemplate(typeof(ScatterViewItem));
            ct.VisualTree = grid;

            Style bubbleStyle = new Style(typeof(ScatterViewItem));
            bubbleStyle.Setters.Add(new Setter(ScatterViewItem.TemplateProperty, ct));
            SVItem.Style = bubbleStyle;

            Animation = new MelodyBubbleAnimation(this, SessionVM);
        }

        /// <summary>
        /// Transforms the MelodyBubble into a list of 
        /// NoteViewModel objects
        /// Used to put them on the staves
        /// </summary>
        /// <param name="positionMelody">The point where the user released his finger</param>
        /// <returns>The list of NoteViewModel</returns>
        public List<NoteViewModel> melodyToListOfNote(Point positionMelody)
        {
            int initPos = melodyBubble.Melody.Notes[0].Position;
            bool up = (positionMelody.Y < 350);
            Converter c = new Converter();
            double height = SessionVM.SessionSVI.ActualHeight;

            List<NoteViewModel> notes = new List<NoteViewModel>();
            for(int i = 0; i< melodyBubble.Melody.Notes.Count; i++)
            {
                double x = (positionMelody.X + (melodyBubble.Melody.Notes[i].Position - initPos) * 60) * SessionVM.Grid.ActualWidth / 1920 ;
                double y = c.getCenterY(up, melodyBubble.Melody.Notes[i]);
                
                double offset = GlobalVariables.ManipulationGrid[((long)positionMelody.X / 60)+melodyBubble.Melody.Notes[i].Position - initPos];
                y -= offset;
                y *= (height / 1080);

                Point p = new Point(x, y);
                notes.Add(new NoteViewModel(p,melodyBubble.Melody.Notes[i], SessionVM.Notes, SessionVM));
            }

            return notes;
        }
    }
}
