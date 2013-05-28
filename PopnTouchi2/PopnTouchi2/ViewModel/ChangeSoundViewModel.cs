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
        private Session session;

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
        public List<SurfaceButton> Images { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ChangeSoundViewModel(Session s)
        {
            session = s;
            
            Grid1 = new Grid();
            Grid2 = new Grid();
            Grid1.VerticalAlignment = VerticalAlignment.Top;
            Grid2.VerticalAlignment = VerticalAlignment.Top;

            if (session.OnePlayer)
            {
                Grid1.Margin = new System.Windows.Thickness(10, 0, 850, 150);
                Grid2.Margin = new System.Windows.Thickness(10, 0, 520, 150);
            }
            else
            {
                Grid1.Margin = new System.Windows.Thickness(20, 0, 500, 100);
                Grid2.Margin = new System.Windows.Thickness(20, 0, 250, 100);
            }


            Images = new List<SurfaceButton>();

            Images.Add(createButtonForImage("soundpointenable", HorizontalAlignment.Left));
            Images.Add(createButtonForImage("soundpointenable", HorizontalAlignment.Center));
            Images.Add(createButtonForImage("soundpointenable", HorizontalAlignment.Right));
            Images.Add(createButtonForImage("soundpointdisable", HorizontalAlignment.Left));
            Images.Add(createButtonForImage("soundpointdisable", HorizontalAlignment.Center));
            Images.Add(createButtonForImage("soundpointdisable", HorizontalAlignment.Right));

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

        public SurfaceButton createButtonForImage(String path, HorizontalAlignment h)
        {
            SurfaceButton b = new SurfaceButton();
            b.Background = getImageBrush(path);
            if (session.OnePlayer)
            {
                b.Height = 24;
                b.Width = 24;
            }
            else
            {
                b.MaxHeight = 5;
                b.MaxWidth = 5;
            }
            b.VerticalAlignment = VerticalAlignment.Center;
            b.HorizontalAlignment = h;

            b.Visibility = Visibility.Visible;
            return b;

        }

        public ImageBrush getImageBrush(String path)
        {
            ImageBrush img = new ImageBrush();
            img.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/Theme" + session.ThemeID + "/" + path + ".png", UriKind.Relative));
            return img;
        }

        private void TouchDown(object sender, TouchEventArgs e)
        {
            SurfaceButton button = new SurfaceButton();
            button = e.Source as SurfaceButton;
            int index = Images.IndexOf(button);

            for (int i = index; i >= 0; i--)
            {
                Images[i].Background = getImageBrush("soundpointenable");
            }

            for (int j = index + 1; j < Images.Count; j++)
            {
                Images[j].Background = getImageBrush("soundpointdisable");
            }
            AudioController.UpdateVolume((float)(index+1));
        }
    }
}
