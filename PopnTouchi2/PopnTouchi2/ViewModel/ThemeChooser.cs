using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using Microsoft.Surface.Presentation.Controls;

namespace PopnTouchi2.ViewModel
{
    public class ThemeChooser
    {
        private SessionViewModel sessionVM;

        public Grid Grid { get; set; }

        public StackPanel Themes { get; set; }
        
        public ThemeChooser(SessionViewModel s)
        {
            sessionVM = s;
            Grid = new Grid();
            Grid.Background = new SolidColorBrush(Colors.Black);
            Grid.Opacity = 0.8;

            Themes = new StackPanel();
            Themes.Orientation = Orientation.Horizontal;
            Themes.VerticalAlignment = VerticalAlignment.Center;
            Themes.HorizontalAlignment = HorizontalAlignment.Center;
            Themes.Opacity = 1;

            Border border1 = new Border();
            border1.BorderBrush = new SolidColorBrush(Colors.White);
            border1.BorderThickness = new Thickness(4);
            border1.Margin = new Thickness(20, 0, 20, 0);
            Grid GridTheme1 = new Grid();
            GridTheme1.Width = 470;
            GridTheme1.Height = 265;
            ImageBrush img1 = new ImageBrush();
            img1.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/Theme1/background.jpg", UriKind.Relative));
            GridTheme1.Background = img1;
            border1.Child = GridTheme1;

            GridTheme1.PreviewTouchDown += new EventHandler<TouchEventArgs>(GridTheme1_TouchDown);

            Border border2 = new Border();
            border2.BorderBrush = new SolidColorBrush(Colors.White);
            border2.BorderThickness = new Thickness(4);
            border2.Margin = new Thickness(20, 0, 20, 0);
            Grid GridTheme2 = new Grid();
            GridTheme2.Width = 470;
            GridTheme2.Height = 265;
            ImageBrush img2 = new ImageBrush();
            img2.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/Theme2/background.jpg", UriKind.Relative));
            GridTheme2.Background = img2;
            border2.Child = GridTheme2;

            GridTheme2.PreviewTouchDown += new EventHandler<TouchEventArgs>(GridTheme2_TouchDown);

            Border border3 = new Border();
            border3.BorderBrush = new SolidColorBrush(Colors.White);
            border3.BorderThickness = new Thickness(4);
            border3.Margin = new Thickness(20, 0, 20, 0);
            Grid GridTheme3 = new Grid();
            GridTheme3.Width = 470;
            GridTheme3.Height = 265;
            ImageBrush img3 = new ImageBrush();
            img3.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/Theme3/background.jpg", UriKind.Relative));
            GridTheme3.Background = img3;
            border3.Child = GridTheme3;

            GridTheme3.PreviewTouchDown += new EventHandler<TouchEventArgs>(GridTheme3_TouchDown);

            Themes.Children.Add(border1);
            Themes.Children.Add(border2);
            Themes.Children.Add(border3);
            
        }

        public void Theme_Button_TouchDown(object sender, RoutedEventArgs e)
        {
            sessionVM.Grid.Children.Add(Grid);
            sessionVM.Grid.Children.Add(Themes);
            Grid.SetZIndex(Grid, 100);
            Grid.SetZIndex(Themes, 101);
        }

        private void GridTheme1_TouchDown(object sender, RoutedEventArgs e)
        {
            if (sessionVM.Session.ThemeID == 1)
            {
                sessionVM.Grid.Children.Remove(Grid);
                sessionVM.Grid.Children.Remove(Themes);
                return;
            }
        }
        
        private void GridTheme2_TouchDown(object sender, RoutedEventArgs e)
        {
            if (sessionVM.Session.ThemeID == 2)
            {
                sessionVM.Grid.Children.Remove(Grid);
                sessionVM.Grid.Children.Remove(Themes);
                return;
            }

            sessionVM.Session.Theme = new Theme2();
            sessionVM.Session.ThemeID = 2;
            sessionVM.ThemeVM = new ThemeViewModel(sessionVM.Session.Theme, sessionVM);
            sessionVM.Grid.Background = sessionVM.ThemeVM.BackgroundImage;
            
            sessionVM.Grid.Children.Remove(sessionVM.NbgVM.Grid);
            sessionVM.Grid.Children.Remove(sessionVM.MbgVM.Grid);
            sessionVM.Grid.Children.Remove(sessionVM.Bubbles);
            sessionVM.Grid.Children.Remove(sessionVM.UpdateSound.Grid1);
            sessionVM.Grid.Children.Remove(sessionVM.UpdateSound.Grid2);
            sessionVM.Grid.Children.Remove(sessionVM.Play_Button);
            sessionVM.Grid.Children.Remove(sessionVM.Theme_Button);
            sessionVM.Grid.Children.Remove(sessionVM.TreeUp.Grid);
            sessionVM.Grid.Children.Remove(sessionVM.TreeDown.Grid);


            sessionVM.NbgVM = new NoteBubbleGeneratorViewModel(sessionVM.Session.NoteBubbleGenerator, sessionVM);
            sessionVM.MbgVM = new MelodyBubbleGeneratorViewModel(sessionVM.Session.MelodyBubbleGenerator, sessionVM);
            sessionVM.Bubbles = new ScatterView();
            sessionVM.UpdateSound = new ChangeSoundViewModel(sessionVM);
            sessionVM.Play_Button.Background = sessionVM.ThemeVM.PlayImage;
            sessionVM.Theme_Button.Background = sessionVM.ThemeVM.ThemesImage;
            sessionVM.displayTrees(new Thickness(0, 100, 0, 0), new Thickness(10, 10, 200, 400));

            sessionVM.Grid.Children.Add(sessionVM.NbgVM.Grid);
            sessionVM.Grid.Children.Add(sessionVM.MbgVM.Grid);
            sessionVM.Grid.Children.Add(sessionVM.Bubbles);
            sessionVM.Grid.Children.Add(sessionVM.UpdateSound.Grid1);
            sessionVM.Grid.Children.Add(sessionVM.UpdateSound.Grid2);
            sessionVM.Grid.Children.Add(sessionVM.Play_Button);
            sessionVM.Grid.Children.Add(sessionVM.Theme_Button);

            sessionVM.Grid.Children.Remove(Grid);
            sessionVM.Grid.Children.Remove(Themes);
        }

        private void GridTheme3_TouchDown(object sender, RoutedEventArgs e)
        {
            if (sessionVM.Session.ThemeID == 3)
            {
                sessionVM.Grid.Children.Remove(Grid);
                sessionVM.Grid.Children.Remove(Themes);
                return;
            }
        }
    }
}
