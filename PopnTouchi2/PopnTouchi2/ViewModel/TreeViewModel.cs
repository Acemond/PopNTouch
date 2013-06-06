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
        /// Parameter.
        /// Used to the relative dimensions
        /// </summary>
        private double ratio;

        /// <summary>
        /// Constructor of a TreeViewModel
        /// </summary>
        /// <param name="up"></param>
        /// <param name="t"></param>
        /// <param name="s"></param>
        public TreeViewModel(bool up, Thickness t, SessionViewModel s)
        {
            this.Up = !up;
            SessionVM = s;
            ratio = s.SessionSVI.Width / 1920.0;

            Grid = new Grid();
            Grid.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Grid.Margin = t;

            Grid.Height = 210.0 * ratio;
            Grid.Width = 210.0 * ratio;

            if (Up)
            {
                Instrument1 = new Instrument(SessionVM.Session.Theme.InstrumentsTop[0].Name);
                Instrument2 = new Instrument(SessionVM.Session.Theme.InstrumentsTop[1].Name);
            }
            else
            {
                Instrument1 = new Instrument(SessionVM.Session.Theme.InstrumentsBottom[0].Name);
                Instrument2 = new Instrument(SessionVM.Session.Theme.InstrumentsBottom[1].Name);
            }


            Images = new List<Grid>();
            Images.Add(createGridForImage(Instrument1.Name.ToString(), 100.0 * ratio, 100.0 * ratio, HorizontalAlignment.Left, VerticalAlignment.Center));
            Images.Add(createGridForImage(Instrument1.Name.ToString(), 100.0 * ratio, 100.0 * ratio, HorizontalAlignment.Right, VerticalAlignment.Top));
            Images.Add(createGridForImage(Instrument2.Name.ToString(), 100.0 * ratio, 100.0 * ratio, HorizontalAlignment.Right, VerticalAlignment.Bottom));

            Grid root = createGridForLinks("root", 50.0 * ratio, 50.0 * ratio, new Thickness(0, 0, 100.0 * ratio, 0));

            Images.Add(root);
            Images.Add(createGridForLinks("lower_branch", 80.0 * ratio, 120.0 * ratio, new Thickness(50.0 * ratio, 60.0 * ratio, 50.0 * ratio, 0.0)));
            Images.Add(createGridForLinks("upper_branch", 80.0 * ratio, 120.0 * ratio, new Thickness(50.0 * ratio, 0.0, 50.0 * ratio, 60.0 * ratio)));

            Images[0].Visibility = Visibility.Visible;

            foreach (Grid g in Images) Grid.Children.Add(g);

            Grid.SetZIndex(root, 300);

            Images[0].TouchDown += new EventHandler<TouchEventArgs>(touchDown0);
            Images[1].TouchDown += new EventHandler<TouchEventArgs>(touchDown1);
            Images[2].TouchDown += new EventHandler<TouchEventArgs>(touchDown2);
            Images[3].TouchDown += new EventHandler<TouchEventArgs>(touchDown1);
        }

        /// <summary>
        /// Update the dimensions of the TreeViewModel
        /// To have relative dimensions
        /// </summary>
        /// <param name="newRatio">The newRation</param>
        public void UpdateDimensions(double newRatio)
        {
            double oldRatio = ratio;
            ratio = newRatio;
            foreach (Grid g in Grid.Children)
            {
                g.Height = (g.Height / oldRatio) * ratio;
                g.Width = (g.Width / oldRatio) * ratio;
                Thickness t = g.Margin;
                t.Left = (t.Left / oldRatio) * newRatio;
                g.Margin = t;
            }
            Thickness t2 = Grid.Margin;
            t2.Left = (t2.Left / oldRatio) * newRatio;
            t2.Right = (t2.Right / oldRatio) * newRatio;
            t2.Top = (t2.Top / oldRatio) * newRatio;
            t2.Bottom = (t2.Bottom / oldRatio) * newRatio;
            Grid.Margin = t2;
        }

        /// <summary>
        /// Setter of instrument
        /// </summary>
        /// <param name="instru"></param>
        public void SetInstrument(Instrument instru)
        {
            if (instru.Name == Instrument1.Name) SwitchToInstru1();
            else if (instru.Name == Instrument2.Name) SwitchToInstru2();
        }

        /// <summary>
        /// Returns the ImageBrush from
        /// the path (name) of the image
        /// </summary>
        /// <param name="path">The name of the image</param>
        /// <returns>The ImageBrush</returns>
        public ImageBrush getImageBrush(String path)
        {
            ImageBrush img = new ImageBrush();
            img.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/UI_items/Instruments/"+path+".png", UriKind.Relative));
            return img;
        }

        /// <summary>
        /// Create a grid with path as Image
        /// for the background
        /// </summary>
        /// <param name="path">The name of the Image</param>
        /// <param name="height">Height of the grid</param>
        /// <param name="width">Width of the grid</param>
        /// <param name="h">Horizontal Alignment</param>
        /// <param name="v">Vertical Alignment</param>
        /// <returns>The grid</returns>
        public Grid createGridForImage(String path, double height, double width, HorizontalAlignment h, VerticalAlignment v)
        {
            Grid g = new Grid();
            ImageBrush ib = getImageBrush(path);
            ib.Stretch = Stretch.Uniform;
            g.Background = ib;
            g.Height = height;
            g.Width = width;
            g.HorizontalAlignment = h;
            g.VerticalAlignment = v;
            g.Visibility = Visibility.Hidden;
            Grid.SetZIndex(g, 201);
            return g;
        }

        /// <summary>
        /// Create a grid for the root, and the branches
        /// </summary>
        /// <param name="path">The name of the image</param>
        /// <param name="height">Height of grid</param>
        /// <param name="width">Width of grid</param>
        /// <param name="t">The Thickness</param>
        /// <returns>The grid</returns>
        public Grid createGridForLinks(String path, double height, double width, Thickness t)
        {
            Grid g = new Grid();
            g.Background = getImageBrush(path);
            g.Height = height;
            g.Width = width;
            g.Margin = t;
            g.Visibility = Visibility.Hidden;
            Grid.SetZIndex(g, 200);
            return g;
        }

        /// <summary>
        /// Event occured when 
        /// the current instrument is touched
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void touchDown0(object sender, TouchEventArgs e)
        {
            for (int i = 1; i < Images.Count; i++) Images[i].Visibility = Visibility.Visible;
            Images[0].Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Event occured when
        /// the instrument on the top-right is touched
        /// or the root
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void touchDown1(object sender, TouchEventArgs e)
        {
            SwitchToInstru1();
        }

        /// <summary>
        /// Event occurend when
        /// the instrument on the bottom-right is touched
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void touchDown2(object sender, TouchEventArgs e)
        {
            SwitchToInstru2();
        }

        /// <summary>
        /// Switch the main instrument to the top instrument
        /// </summary>
        private void SwitchToInstru1()
        {
            Images[0].Visibility = Visibility.Hidden;
            Images[0].Background = getImageBrush(Instrument1.Name.ToString());
            Images[0].Visibility = Visibility.Visible;
            for (int i = 1; i < Images.Count; i++) { Images[i].Visibility = Visibility.Hidden; }

            if (Up) SessionVM.Session.StaveTop.CurrentInstrument = Instrument1;
            else SessionVM.Session.StaveBottom.CurrentInstrument = Instrument1;
            SessionVM.Session.ChangeBpm(SessionVM.Session.Bpm);
        }

        /// <summary>
        /// Switch the main instrument to the bottom instrument
        /// </summary>
        private void SwitchToInstru2()
        {
            Images[0].Visibility = Visibility.Hidden;
            Images[0].Background = getImageBrush(Instrument2.Name.ToString());
            Images[0].Visibility = Visibility.Visible;
            for (int i = 1; i < Images.Count; i++) { Images[i].Visibility = Visibility.Hidden; }

            if (Up) SessionVM.Session.StaveTop.CurrentInstrument = Instrument2;
            else SessionVM.Session.StaveBottom.CurrentInstrument = Instrument2;
            SessionVM.Session.ChangeBpm(SessionVM.Session.Bpm);
        }
    }
}
