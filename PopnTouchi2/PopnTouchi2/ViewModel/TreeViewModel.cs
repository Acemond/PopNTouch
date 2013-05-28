﻿using System;
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
        public SessionViewModel SessionVM;

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
        private Boolean Up;

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
        /// <param name="up"></param>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <param name="theme"></param>
        public TreeViewModel(Boolean up, Thickness t, SessionViewModel s)
        {
            this.Up = !up;
            SessionVM = s;
            Grid = new Grid();
            Grid.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Grid.Margin = t;

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
            Images.Add(createGridForImage(Instrument1.Name.ToString(), 100, 100, HorizontalAlignment.Left, VerticalAlignment.Center));
            Images.Add(createGridForImage(Instrument1.Name.ToString(), 100, 100, HorizontalAlignment.Right, VerticalAlignment.Top));
            Images.Add(createGridForImage(Instrument2.Name.ToString(), 100, 100, HorizontalAlignment.Right, VerticalAlignment.Bottom));

            Images.Add(createGridForLinks("root", 50, 50, new Thickness(0, 0, 100, 0)));
            Images.Add(createGridForLinks("lower_branch", 80, 120, new Thickness(50, 80, 50, 0)));
            Images.Add(createGridForLinks("upper_branch", 80, 120, new Thickness(50, 0, 50, 80)));

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

        /// <summary>
        /// Create a grid for the root, and the branches
        /// </summary>
        /// <param name="path">The name of the image</param>
        /// <param name="height">Height of grid</param>
        /// <param name="width">Width of grid</param>
        /// <param name="t">The Thickness</param>
        /// <returns>The grid</returns>
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

        /// <summary>
        /// Event occured when 
        /// the current instrument is touched
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void touchDown0(object sender, TouchEventArgs e)
        {
            for (int i = 1; i < Images.Count; i++) { Images[i].Visibility = Visibility.Visible; }
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
            Images[0].Visibility = Visibility.Hidden;
            Images[0].Background = getImageBrush(Instrument1.Name.ToString());
            Images[0].Visibility = Visibility.Visible;
            for (int i = 1; i < Images.Count; i++) { Images[i].Visibility = Visibility.Hidden; }
            if (Up)
            {
                SessionVM.Session.StaveTop.CurrentInstrument = Instrument1;
            }
            else
            {
                SessionVM.Session.StaveBottom.CurrentInstrument = Instrument1;
            }
            
        }

        /// <summary>
        /// Event occurend when
        /// the instrument on the bottom-right is touched
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void touchDown2(object sender, TouchEventArgs e)
        {
            Images[0].Visibility = Visibility.Hidden;
            Images[0].Background = getImageBrush(Instrument2.Name.ToString());
            Images[0].Visibility = Visibility.Visible;
            for (int i = 1; i < Images.Count; i++) { Images[i].Visibility = Visibility.Hidden; }
            if (Up)
            {
                SessionVM.Session.StaveTop.CurrentInstrument = Instrument2;
            }
            else
            {
                SessionVM.Session.StaveBottom.CurrentInstrument = Instrument2;
            }
        }
    }
}
