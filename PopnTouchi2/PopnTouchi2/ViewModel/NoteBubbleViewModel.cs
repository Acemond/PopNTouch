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

namespace PopnTouchi2
{
    /// <summary>
    /// Binds NoteBubble's properties to the View.
    /// </summary>
    public class NoteBubbleViewModel : ViewModelBase
    {
        /// <summary>
        /// Parameter.
        /// NoteBubble element from the Model.
        /// </summary>
        private NoteBubble noteBubble;

        public ScatterView ParentSV { get; set; }

        public ScatterViewItem SVItem { get; set; }

        private NoteBubbleAnimation animation;

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
        public NoteBubbleViewModel(NoteBubble nb, ScatterView sv)
        {
            noteBubble = nb;
            SVItem = new ScatterViewItem();
            ParentSV = sv;
            
            Random r = new Random();
            SVItem.Center = new Point(r.Next((int)sv.ActualWidth), r.Next((int)(635 * sv.ActualHeight / 1080), (int)sv.ActualHeight));

            SVItem.CanScale = false;
            SVItem.HorizontalAlignment = HorizontalAlignment.Center;
            SVItem.CanRotate = false;
            SVItem.HorizontalAlignment = HorizontalAlignment.Center;

            animation = new NoteBubbleAnimation(this);
        }

        /// <summary>
        /// NoteBubbleViewModel theme specific constructor.
        /// </summary>
        /// <param name="?"></param>
        /// <param name="sv"></param>
        /// <param name="theme"></param>
        public NoteBubbleViewModel(NoteBubble nb, ScatterView sv, Theme theme) : this(nb, sv)
        {
            FrameworkElementFactory bubbleImage = new FrameworkElementFactory(typeof(Image));
            bubbleImage.SetValue(Image.SourceProperty, theme.GetNoteBubbleImageSource(nb.Note.Duration));
            bubbleImage.SetValue(Image.IsHitTestVisibleProperty, false);
            bubbleImage.SetValue(Image.WidthProperty, 85.0);
            bubbleImage.SetValue(Image.HeightProperty, 85.0);

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
        }


        /// <summary>
        /// Property.
        /// TODO
        /// </summary>
        public NoteBubble Note
        {
            get
            {
                return noteBubble;
            }
            set
            {
                noteBubble = value;
                NotifyPropertyChanged("Note");
            }
        }

        /// <summary>
        /// Property.
        /// TODO
        /// </summary>
        public bool IsOnStave
        {
            get
            {
                return isOnStave;
            }
            set
            {
                isOnStave = value;
                NotifyPropertyChanged("IsOnStave");
            }
        }
    }
}
