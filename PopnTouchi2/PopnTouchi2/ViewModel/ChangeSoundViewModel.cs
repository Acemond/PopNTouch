using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Infrastructure;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Input;

namespace PopnTouchi2.ViewModel
{
    /// <summary>
    /// Object of the view used to change the sound of the notes
    /// </summary>
    public class ChangeSoundViewModel
    {
        /// <summary>
        /// Parameter.
        /// The current Session
        /// </summary>
        private SessionViewModel sessionVM;

        /// <summary>
        /// Property.
        /// The parent Grid.
        /// </summary>
        public Grid Grid { get; set; }

        private double ratio;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChangeSoundViewModel(SessionViewModel s)
        {
            sessionVM = s;
            ratio = s.SessionSVI.Width / 1920.0;

            Grid = new Grid();
            Grid.VerticalAlignment = VerticalAlignment.Top;
            Grid.HorizontalAlignment = HorizontalAlignment.Left;

            Grid.Margin = new System.Windows.Thickness(0.0, 20.0 * ratio, 0, 0);
            
            Grid.Children.Add(createButtonForImage(true, 0.0));
            Grid.Children.Add(createButtonForImage(true, 80.0 * ratio));
            Grid.Children.Add(createButtonForImage(true, 160.0 * ratio));
            Grid.Children.Add(createButtonForImage(false, 240.0 * ratio));
            Grid.Children.Add(createButtonForImage(false, 320.0 * ratio));
            Grid.Children.Add(createButtonForImage(false, 400.0 * ratio));
        }

        /// <summary>
        /// UpdateDimensions when the size of the screen changes
        /// </summary>
        /// <param name="newRatio"></param>
        public void UpdateDimensions(double newRatio)
        {
            double oldRatio = ratio;
            ratio = newRatio;
            foreach (Grid g in Grid.Children)
            {
                g.Height = 28.0 * ratio;
                g.Width = 28.0 * ratio;
                Thickness t = g.Margin;
                t.Left = (t.Left / oldRatio) * newRatio;
                g.Margin = t;
            }

            Grid.Margin = new System.Windows.Thickness(0.0, 20.0 * ratio, 0, 0);
        }

        /// <summary>
        /// createButtonForImage(String path, HorizontalAlignment h)
        /// Create a grid used as a button
        /// With the image from path
        /// </summary>
        /// <param name="enabled">Bool</param>
        /// <param name="margin">The position of the button</param>
        /// <returns>The Grid used as a button</returns>
        public Grid createButtonForImage(bool enabled, double margin)
        {
            Grid g = new Grid();
            double ratio = sessionVM.SessionSVI.Width / 1920.0;

            if (enabled) g.Background = sessionVM.ThemeVM.SoundPointEnableImage;
            else g.Background = sessionVM.ThemeVM.SoundPointDisableImage;

            g.Height = 28.0 * ratio;
            g.Width = 28.0 * ratio;

            g.VerticalAlignment = VerticalAlignment.Center;

            g.PreviewTouchDown += new EventHandler<TouchEventArgs>(TouchDown);
            g.Margin = new Thickness(margin, 0.0, 0.0, 0.0);
            return g;
        }

        /// <summary>
        /// getImageBrush(String path)
        /// Returns the ImageBrush from the path given
        /// </summary>
        /// <param name="path">The path (name) of the image</param>
        /// <returns>The ImageBrush</returns>
        public ImageBrush getImageBrush(String path)
        {
            ImageBrush img = new ImageBrush();
            img.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/Theme" + sessionVM.Session.ThemeID + "/" + path + ".png", UriKind.Relative));
            return img;
        }

        /// <summary>
        /// Event TouchDown 
        /// Used when a Grid is touched
        /// Change the sound of notes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TouchDown(object sender, TouchEventArgs e)
        {
            Grid button = new Grid();
            button = e.Source as Grid;
            int index = Grid.Children.IndexOf(button);
            
            for (int i = index; i >= 0; i--)
            {
                ((Grid)Grid.Children[i]).Background = sessionVM.ThemeVM.SoundPointEnableImage;
            }

            for (int i = index + 1; i < Grid.Children.Count; i++)
            {
                ((Grid)Grid.Children[i]).Background = sessionVM.ThemeVM.SoundPointDisableImage;
            }
            AudioController.UpdateVolume((float)(index + 1));
        }
    }
}
