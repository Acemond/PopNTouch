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
        public Grid Grid1 { get; set; }

        /// <summary>
        /// Property.
        /// The parent Grid.
        /// </summary>
        public Grid Grid2 { get; set; }

        /// <summary>
        /// Property.
        /// The List of circles' image
        /// </summary>
        public List<Grid> Images { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ChangeSoundViewModel(SessionViewModel s)
        {
            sessionVM = s;
            
            Grid1 = new Grid();
            Grid2 = new Grid();
            Grid1.VerticalAlignment = VerticalAlignment.Top;
            Grid2.VerticalAlignment = VerticalAlignment.Top;

            if (sessionVM.Session.OnePlayer)
            {
                Grid1.Margin = new System.Windows.Thickness(10, 0, 1200, 0);
                Grid2.Margin = new System.Windows.Thickness(10, 0, 690, 0);
            }
            else
            {
                Grid1.Margin = new System.Windows.Thickness(20, 0, 750, 0);
                Grid2.Margin = new System.Windows.Thickness(20, 0, 470, 0);
            }


            Images = new List<Grid>();

            Images.Add(createButtonForImage(true, HorizontalAlignment.Left));
            Images.Add(createButtonForImage(true, HorizontalAlignment.Center));
            Images.Add(createButtonForImage(true, HorizontalAlignment.Right));
            Images.Add(createButtonForImage(false, HorizontalAlignment.Left));
            Images.Add(createButtonForImage(false, HorizontalAlignment.Center));
            Images.Add(createButtonForImage(false, HorizontalAlignment.Right));

            for(int i = 0; i< Images.Count/2 ; i++)
            {
                Grid1.Children.Add(Images[i]);
                Images[i].PreviewTouchDown += new EventHandler<TouchEventArgs>(TouchDown);
            }

            for (int i = Images.Count/2; i < Images.Count; i++)
            {
                Grid2.Children.Add(Images[i]);
                Images[i].PreviewTouchDown += new EventHandler<TouchEventArgs>(TouchDown);
            }
        }

        public Grid createButtonForImage(Boolean enabled, HorizontalAlignment h)
        {
            Grid g = new Grid();
            if (enabled)
            {
                g.Background = sessionVM.ThemeVM.SoundPointEnableImage;
            }
            else
            {
                g.Background = sessionVM.ThemeVM.SoundPointDisableImage;
            }

            if (sessionVM.Session.OnePlayer)
            {
                g.Height = 24;
                g.Width = 24;
            }
            else
            {
                g.Height = 16;
                g.Width = 16;
            }
            g.VerticalAlignment = VerticalAlignment.Center;
            g.HorizontalAlignment = h;

            g.Visibility = Visibility.Visible;
            return g;

        }

        private void TouchDown(object sender, TouchEventArgs e)
        {
            Grid button = new Grid();
            button = e.Source as Grid;
            int index = Images.IndexOf(button);

            for (int i = index; i >= 0; i--)
            {
                Images[i].Background = sessionVM.ThemeVM.SoundPointEnableImage;
            }

            for (int j = index + 1; j < Images.Count; j++)
            {
                Images[j].Background = sessionVM.ThemeVM.SoundPointDisableImage;
            }
            AudioController.UpdateVolume((float)(index+1));
        }
    }
}
