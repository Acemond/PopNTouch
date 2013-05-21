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
    public class MelodyBubbleViewModel : ViewModelBase
    {
        /// <summary>
        /// Parameter.
        /// MelodyBubble element from the Model.
        /// </summary>
        private MelodyBubble melodyBubble;
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
        private MelodyBubbleAnimation animation;
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
            melodyBubble = mb;
            SVItem = new ScatterViewItem();
            ParentSV = sv;
            
            Random r = new Random();
            SVItem.Center = new Point(r.Next((int)sv.ActualWidth), r.Next((int)(635 * sv.ActualHeight / 1080), (int)sv.ActualHeight));

            SVItem.CanScale = false;
            SVItem.HorizontalAlignment = HorizontalAlignment.Center;
            SVItem.CanRotate = false;
            SVItem.HorizontalAlignment = HorizontalAlignment.Center;

            FrameworkElementFactory bubbleImage = new FrameworkElementFactory(typeof(Image));
            switch (s.Session.ThemeID)
            {
                case 1:
                    bubbleImage.SetValue(Image.SourceProperty, new Theme1ViewModel(s.Session.Theme, s).GetMelodyBubbleImageSource(mb.Melody));
                    break;
                case 2:
                    bubbleImage.SetValue(Image.SourceProperty, new Theme2ViewModel(s.Session.Theme, s).GetMelodyBubbleImageSource(mb.Melody));
                    break;
                case 3:
                    bubbleImage.SetValue(Image.SourceProperty, new Theme3ViewModel(s.Session.Theme, s).GetMelodyBubbleImageSource(mb.Melody));
                    break;
                case 4:
                    bubbleImage.SetValue(Image.SourceProperty, new Theme4ViewModel(s.Session.Theme, s).GetMelodyBubbleImageSource(mb.Melody));
                    break;
            }

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

            animation = new MelodyBubbleAnimation(this, s);
        }
    }
}
