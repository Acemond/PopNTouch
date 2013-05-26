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
    public class TreeViewModel : ViewModelBase
    {
        /// <summary>
        /// Parameter.
        /// Private
        /// </summary>
        private Session session;

        /// <summary>
        /// Property.
        /// The parent Grid.
        /// </summary>
        public Grid Grid { get; set; }

        /// <summary>
        /// Property.
        /// The ScatterViewItem containing the tree.
        /// </summary>
        public List<Grid> Images { get; set; }

        /// <summary>
        /// Attribute
        /// up is true if the Tree is on the StaveTop
        /// </summary>
        private bool Up;

        /// <summary>
        /// Property.
        /// </summary>
        public Instrument Instrument1 { get; set; }

        /// <summary>
        /// Property.
        /// </summary>
        public Instrument Instrument2 { get; set; }

        /// <summary>
        /// Constructor of a TreeViewModel
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s"></param>
        public TreeViewModel(bool up, Thickness t, Session s, Theme theme)
        {
            this.Up = up;
            session = s;
            Grid = new Grid();
            Grid.Width = 200;
            Grid.Height = 200;
            Grid.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Grid.Margin = t;

            if (Up)
            {
                Instrument1 = new Instrument(theme.InstrumentsTop[0].Name);
                Instrument2 = new Instrument(theme.InstrumentsTop[1].Name);
            }
            else
            {
                Instrument1 = new Instrument(theme.InstrumentsBottom[0].Name);
                Instrument2 = new Instrument(theme.InstrumentsBottom[1].Name);
            }


            Images = new List<Grid>();
            Images.Add(createGridForImage(Instrument1.Name.ToString(), 80, 80, HorizontalAlignment.Left, VerticalAlignment.Center));
            Images.Add(createGridForImage(Instrument1.Name.ToString(), 80, 80, HorizontalAlignment.Right, VerticalAlignment.Top));
            Images.Add(createGridForImage(Instrument2.Name.ToString(), 80, 80, HorizontalAlignment.Right, VerticalAlignment.Bottom));
            Images.Add(createGridForLinks("root", 50, 50, new Thickness(0, 0, 100, 0)));
            Images.Add(createGridForLinks("lower_branch", 80, 80, new Thickness(50, 80, 50, 0)));
            Images.Add(createGridForLinks("upper_branch", 80, 80, new Thickness(50, 0, 50, 80)));

            Images[0].Visibility = Visibility.Visible;

            foreach (Grid g in Images)
            {
                Grid.Children.Add(g);
            }

            Images[0].TouchDown += new EventHandler<TouchEventArgs>(touchDown0);
            Images[1].TouchDown += new EventHandler<TouchEventArgs>(touchDown1);
            Images[2].TouchDown += new EventHandler<TouchEventArgs>(touchDown2);
            Images[3].TouchDown += new EventHandler<TouchEventArgs>(touchDown1);
        }

        public ImageBrush getImageBrush(String path)
        {
            ImageBrush img = new ImageBrush();
            img.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/UI_items/"+path+".png", UriKind.Relative));
            return img;
        }

        public Grid createGridForImage(String path, int height, int width, HorizontalAlignment h, VerticalAlignment v)
        {
            Grid g = new Grid();
            g.Background = getImageBrush(path);
            g.Height = height;
            g.Width = width;
            g.HorizontalAlignment = h;
            g.VerticalAlignment = v;
            g.Visibility = Visibility.Hidden;
         
            return g;
        }

        public Grid createGridForLinks(String path, int height, int width, Thickness t)
        {
            Grid g = new Grid();
            g.Background = getImageBrush(path);
            g.Height = height;
            g.Width = width;
            g.Margin = t;
            g.Visibility = Visibility.Hidden;

            return g;
        }

        private void touchDown0(object sender, TouchEventArgs e)
        {
            for (int i = 1; i < Images.Count; i++) { Images[i].Visibility = Visibility.Visible; }
            Images[0].Visibility = Visibility.Hidden;
        }

        private void touchDown1(object sender, TouchEventArgs e)
        {
            Images[0].Visibility = Visibility.Hidden;
            Images[0].Background = getImageBrush(Instrument1.Name.ToString());
            Images[0].Visibility = Visibility.Visible;
            for (int i = 1; i < Images.Count; i++) { Images[i].Visibility = Visibility.Hidden; }
            if (Up)
            {
                session.StaveTop.CurrentInstrument = Instrument1;
            }
            else
            {
                session.StaveBottom.CurrentInstrument = Instrument1;
            }
            
        }

        private void touchDown2(object sender, TouchEventArgs e)
        {
            Images[0].Visibility = Visibility.Hidden;
            Images[0].Background = getImageBrush(Instrument2.Name.ToString());
            Images[0].Visibility = Visibility.Visible;
            for (int i = 1; i < Images.Count; i++) { Images[i].Visibility = Visibility.Hidden; }
            if (Up)
            {
                session.StaveTop.CurrentInstrument = Instrument2;
            }
            else
            {
                session.StaveBottom.CurrentInstrument = Instrument2;
            }
        }
    }
}
