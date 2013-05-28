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
    /// Binds NoteBubble's properties to the View.
    /// </summary>
    public class NoteBubbleViewModel : ViewModelBase
    {
        /// <summary>
        /// Parameter.
        /// </summary>
        private NoteBubble noteBubble;
        /// <summary>
        /// Property.
        /// NoteBubble element from the Model.
        /// </summary>
        public NoteBubble NoteBubble
        {
            get
            {
                return noteBubble;
            }
            set
            {
                noteBubble = value;
                NotifyPropertyChanged("NoteBubble");
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
        /// The NoteBubbleAnimation item handling all animations for the noteBubble.
        /// </summary>
        public NoteBubbleAnimation Animation { get; set; }

        /// <summary>
        /// NoteBubbleViewModel theme specific constructor.
        /// </summary>
        /// <param name="nb">The NoteBubble to link with its ViewModel</param>
        /// <param name="sv">The Parent ScatterView</param>
        /// <param name="s">The current SessionVM</param>
        public NoteBubbleViewModel(NoteBubble nb, ScatterView sv, SessionViewModel s) : base(s)
        {
            NoteBubble = nb;
            SVItem = new ScatterViewItem();
            ParentSV = sv;

            Random r = new Random();
            SVItem.Center = new Point(r.Next((int)sv.ActualWidth), r.Next((int)(635 * sv.ActualHeight / 1080), (int)sv.ActualHeight));

            SVItem.CanScale = false;
            SVItem.HorizontalAlignment = HorizontalAlignment.Center;
            SVItem.CanRotate = false;
            SVItem.HorizontalAlignment = HorizontalAlignment.Center;

            FrameworkElementFactory bubbleImage = new FrameworkElementFactory(typeof(Image));
            if (!noteBubble.Note.Sharp && !noteBubble.Note.Flat)
            {    
                bubbleImage.SetValue(Image.SourceProperty, new ThemeViewModel(SessionVM.Session.Theme, SessionVM).GetNoteBubbleImageSource(nb.Note.Duration));                  
            }
            else
            {
                bubbleImage.SetValue(Image.SourceProperty, new ThemeViewModel(SessionVM.Session.Theme, SessionVM).GetNoteBubbleImageSource(nb.Note.Sharp));
            }
            
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
            touchZone.SetValue(Ellipse.MarginProperty, new Thickness(15));

            FrameworkElementFactory grid = new FrameworkElementFactory(typeof(Grid));
            grid.AppendChild(bubbleImage);
            grid.AppendChild(touchZone);

            ControlTemplate ct = new ControlTemplate(typeof(ScatterViewItem));
            ct.VisualTree = grid;

            Style bubbleStyle = new Style(typeof(ScatterViewItem));
            bubbleStyle.Setters.Add(new Setter(ScatterViewItem.TemplateProperty, ct));
            SVItem.Style = bubbleStyle;

            Animation = new NoteBubbleAnimation(this, SessionVM);
        }

        /// <summary>
        /// NoteBubbleViewModel theme specific constructor.
        /// </summary>
        /// <param name="center">Location where the NoteBubble should be</param>
        /// <param name="nb">The NoteBubble to link with its ViewModel</param>
        /// <param name="sv">The Parent ScatterView</param>
        /// <param name="s">The current SessionVM</param>
        public NoteBubbleViewModel(Point center, NoteBubble nb, ScatterView sv, SessionViewModel s) : base(s)
        {
            NoteBubble = nb;
            SVItem = new ScatterViewItem();
            ParentSV = sv;

            Random r = new Random();
            SVItem.Center = center;

            SVItem.CanScale = false;
            SVItem.HorizontalAlignment = HorizontalAlignment.Center;
            SVItem.CanRotate = false;
            SVItem.HorizontalAlignment = HorizontalAlignment.Center;

            FrameworkElementFactory bubbleImage = new FrameworkElementFactory(typeof(Image));

            bubbleImage.SetValue(Image.SourceProperty, new ThemeViewModel(SessionVM.Session.Theme, SessionVM).GetNoteBubbleImageSource(nb.Note.Duration));
                 
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

            Animation = new NoteBubbleAnimation(this, SessionVM);
        }
    }
}
