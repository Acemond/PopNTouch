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
        /// Parameter.
        /// True if the center of the Bubble is located on the stave.
        /// </summary>
        private bool isOnStave;
        /// <summary>
        /// Parameter.
        /// Event triggered when a NoteBubble is dropped on the stave.
        /// </summary>
        public event EventHandler dropBubble;

         /// <summary>
        /// NoteBubbleViewModel Constructor.
        /// TODO
        /// </summary>
        public MelodyBubbleViewModel(MelodyBubble mb, ScatterView sv, SessionViewModel s) : base(s)
        {
            MelodyBubble = mb;
            SVItem = new ScatterViewItem();
            ParentSV = sv;
            isOnStave = false;
            
            Random r = new Random();
            SVItem.Center = new Point(r.Next((int)sv.ActualWidth), r.Next((int)(635 * sv.ActualHeight / 1080), (int)sv.ActualHeight));

            SVItem.CanScale = false;
            SVItem.HorizontalAlignment = HorizontalAlignment.Center;
            SVItem.CanRotate = false;
            SVItem.HorizontalAlignment = HorizontalAlignment.Center;

            FrameworkElementFactory bubbleImage = new FrameworkElementFactory(typeof(Image));
 
            bubbleImage.SetValue(Image.SourceProperty, new ThemeViewModel(SessionVM.Session.Theme, SessionVM).GetMelodyBubbleImageSource(mb.Melody.gesture));

            bubbleImage.SetValue(Image.IsHitTestVisibleProperty, false);
            bubbleImage.SetValue(Image.WidthProperty, 135.0);
            bubbleImage.SetValue(Image.HeightProperty, 135.0);

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

        public List<NoteViewModel> melodyToListOfNote(Point center)
        {
         
            int width = (int)SessionVM.Grid.ActualWidth;
            int height = (int)SessionVM.Grid.ActualHeight;
            int initialPosition = melodyBubble.Melody.Notes[0].Position;
            String initialPitch = melodyBubble.Melody.Notes[0].Pitch;
            int initialOctave = melodyBubble.Melody.Notes[0].Octave;
            Converter c = new Converter();

            List<NoteViewModel> notes = new List<NoteViewModel>();
            for(int i = 0; i< melodyBubble.Melody.Notes.Count; i++)
            {
                double x = center.X + (melodyBubble.Melody.Notes[i].Position - initialPosition) * (60*width/1920);
                double y = center.Y + ((initialOctave - melodyBubble.Melody.Notes[i].Octave) * 7 + (c.PositionToPitch.IndexOf(initialPitch) - c.PositionToPitch.IndexOf(melodyBubble.Melody.Notes[i].Pitch))) * 25;
                Point p = new Point(x, y);
                notes.Add(new NoteViewModel(p,melodyBubble.Melody.Notes[i], SessionVM.Notes, SessionVM));
            }

            return notes;
        }
    }
}
