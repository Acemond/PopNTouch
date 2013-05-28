using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Shapes;
using PopnTouchi2.Infrastructure;
using PopnTouchi2.ViewModel.Animation;

namespace PopnTouchi2.ViewModel
{
    /// <summary>
    /// Binds Note's properties to the View.
    /// </summary>
    public class NoteViewModel : ViewModelBase
    {
        /// <summary>
        /// Parameter.
        /// Note element from the Model.
        /// </summary>
        private Note note;

        /// <summary>
        /// Property.
        /// The parent ScatterView.
        /// </summary>
        public ScatterView ParentSV { get; set; }

        /// <summary>
        /// Property.
        /// The ScatterViewItem containing the note.
        /// </summary>
        public ScatterViewItem SVItem { get; set; }

        /// <summary>
        /// Parameter.
        /// The NoteAnimation item handling all animations for the note.
        /// </summary>
        public NoteAnimation Animation { get; set; }

        /// <summary>
        /// NoteViewModel Constructor.
        /// TODO
        /// </summary>
        public NoteViewModel(Point center, Note n, ScatterView sv, SessionViewModel s)
            : base(s)
        {
            note = n;
            SVItem = new ScatterViewItem();
            ParentSV = sv;

            SVItem.Center = center;

            SVItem.CanScale = false;
            SVItem.HorizontalAlignment = HorizontalAlignment.Center;
            SVItem.CanRotate = false;
            SVItem.HorizontalAlignment = HorizontalAlignment.Center;
            

            FrameworkElementFactory bubbleImage = new FrameworkElementFactory(typeof(Image));

            String noteValue = note.Duration.ToString();

            if (center.Y < (370 * SessionVM.SessionSVI.ActualHeight /1080))
                bubbleImage.SetValue(Image.SourceProperty, new BitmapImage(new Uri(@"../../Resources/Images/UI_items/Notes/black/" + noteValue + ".png", UriKind.Relative)));
            else
                bubbleImage.SetValue(Image.SourceProperty, new BitmapImage(new Uri(@"../../Resources/Images/UI_items/Notes/white/" + noteValue + ".png", UriKind.Relative)));

            bubbleImage.SetValue(Image.IsHitTestVisibleProperty, false);
            if (SessionVM.Session.OnePlayer)
            {
                bubbleImage.SetValue(Image.WidthProperty, 85.0);
                bubbleImage.SetValue(Image.HeightProperty, 85.0);
            }
            else
            {
                bubbleImage.SetValue(Image.WidthProperty, 50.0);
                bubbleImage.SetValue(Image.HeightProperty, 50.0);
            }

            FrameworkElementFactory touchZone = new FrameworkElementFactory(typeof(Ellipse));
            touchZone.SetValue(Ellipse.FillProperty, Brushes.Transparent);
            touchZone.SetValue(Ellipse.MarginProperty, new Thickness(20, 1, 12, 1));
            

            FrameworkElementFactory grid = new FrameworkElementFactory(typeof(Grid));
            grid.AppendChild(bubbleImage);
            grid.AppendChild(touchZone);

            ControlTemplate ct = new ControlTemplate(typeof(ScatterViewItem));
            ct.VisualTree = grid;

            Style bubbleStyle = new Style(typeof(ScatterViewItem));
            bubbleStyle.Setters.Add(new Setter(ScatterViewItem.TemplateProperty, ct));
            SVItem.Style = bubbleStyle;

            Animation = new NoteAnimation(this, SessionVM);
        }

        /// <summary>
        /// Property.
        /// TODO
        /// </summary>
        public Note Note
        {
            get
            {
                return note;
            }
            set
            {
                note = value;
                NotifyPropertyChanged("Notes");
            }
        }
    }
}
