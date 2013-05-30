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
    /// <summary>
    /// TODO
    /// </summary>
    public class ThemeChooser
    {
        /// <summary>
        /// TODO
        /// </summary>
        private SessionViewModel sessionVM;

        /// <summary>
        /// TODO
        /// </summary>
        public Grid Grid { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public Grid Themes { get; set; }

        private Border border1;
        private Grid GridTheme1;
        private Border border2;
        private Grid GridTheme2;
        private Border border3;
        private Grid GridTheme3;
        private Border border4;
        private Grid GridTheme4;
        public Grid Bird { get; set; }
        public Grid Dragon { get; set; }
        public Grid Cat { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="s"></param>
        public ThemeChooser(SessionViewModel s)
        {
            sessionVM = s;
            double ratio = sessionVM.SessionSVI.Width / 1920.0;
            Grid = new Grid();
            Grid.Background = new SolidColorBrush(Colors.Black);
            Grid.Opacity = 0.8;

            Themes = new Grid();
            Themes.Width = 1036 * ratio;
            Themes.Height = 626 * ratio;
            Themes.VerticalAlignment = VerticalAlignment.Center;
            Themes.HorizontalAlignment = HorizontalAlignment.Center;
            Themes.Opacity = 1;

            border1 = new Border();
            border1.BorderBrush = new SolidColorBrush(Colors.White);
            border1.BorderThickness = new Thickness(4.0 * ratio);
            border1.Margin = new Thickness(20.0 * ratio, 20.0 * ratio, 20.0 * ratio, 20.0 * ratio);
            border1.HorizontalAlignment = HorizontalAlignment.Left;
            border1.VerticalAlignment = VerticalAlignment.Top;
            GridTheme1 = new Grid();
            GridTheme1.Width = 470.0 * ratio;
            GridTheme1.Height = 265.0 * ratio;
            ImageBrush img1 = new ImageBrush();
            img1.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/Theme1/background.jpg", UriKind.Relative));
            GridTheme1.Background = img1;
            border1.Child = GridTheme1;

            GridTheme1.PreviewTouchDown += new EventHandler<TouchEventArgs>(GridTheme1_TouchDown);

            border2 = new Border();
            border2.BorderBrush = new SolidColorBrush(Colors.White);
            border2.BorderThickness = new Thickness(4.0 * ratio);
            border2.Margin = new Thickness(20.0 * ratio, 20.0 * ratio, 20.0 * ratio, 20.0 * ratio);
            border2.HorizontalAlignment = HorizontalAlignment.Right;
            border2.VerticalAlignment = VerticalAlignment.Top;
            GridTheme2 = new Grid();
            GridTheme2.Width = 470.0 * ratio;
            GridTheme2.Height = 265.0 * ratio;
            ImageBrush img2 = new ImageBrush();
            img2.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/Theme2/background.jpg", UriKind.Relative));
            GridTheme2.Background = img2;
            border2.Child = GridTheme2;

            GridTheme2.PreviewTouchDown += new EventHandler<TouchEventArgs>(GridTheme2_TouchDown);

            border3 = new Border();
            border3.BorderBrush = new SolidColorBrush(Colors.White);
            border3.BorderThickness = new Thickness(4.0 * ratio);
            border3.Margin = new Thickness(20.0 * ratio, 20.0 * ratio, 20.0 * ratio, 20.0 * ratio);
            border3.HorizontalAlignment = HorizontalAlignment.Left;
            border3.VerticalAlignment = VerticalAlignment.Bottom;
            GridTheme3 = new Grid();
            GridTheme3.Width = 470.0 * ratio;
            GridTheme3.Height = 265.0 * ratio;
            ImageBrush img3 = new ImageBrush();
            img3.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/Theme3/background.jpg", UriKind.Relative));
            GridTheme3.Background = img3;
            border3.Child = GridTheme3;

            GridTheme3.PreviewTouchDown += new EventHandler<TouchEventArgs>(GridTheme3_TouchDown);

            border4 = new Border();
            border4.BorderBrush = new SolidColorBrush(Colors.White);
            border4.BorderThickness = new Thickness(4.0 * ratio);
            border4.Margin = new Thickness(20.0 * ratio, 20.0 * ratio, 20.0 * ratio, 20.0 * ratio);
            border4.HorizontalAlignment = HorizontalAlignment.Right;
            border4.VerticalAlignment = VerticalAlignment.Bottom;
            GridTheme4 = new Grid();
            GridTheme4.Width = 470.0 * ratio;
            GridTheme4.Height = 265.0 * ratio;
            ImageBrush img4 = new ImageBrush();
            img4.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/Theme4/background.jpg", UriKind.Relative));
            GridTheme4.Background = img4;
            border4.Child = GridTheme4;

            GridTheme4.PreviewTouchDown += new EventHandler<TouchEventArgs>(GridTheme4_TouchDown);

            Bird = new Grid();
            Bird.Width = 140.0 * ratio;
            Bird.Height = 165.0 * ratio;
            Bird.Margin = new Thickness(0, 585.0 * ratio, 80.0 * ratio, 0);
            Bird.Background = new SolidColorBrush(Colors.Transparent);

            Bird.PreviewTouchDown += new EventHandler<TouchEventArgs>(Bird_TouchDown);

            Dragon = new Grid();
            Dragon.Width = 175.0 * ratio;
            Dragon.Height = 275.0 * ratio;
            Dragon.Margin = new Thickness(0, 550.0 * ratio, 40.0 * ratio, 0);
            Dragon.Background = new SolidColorBrush(Colors.Transparent);

            Dragon.PreviewTouchDown += new EventHandler<TouchEventArgs>(Dragon_TouchDown);

            Cat = new Grid();
            Cat.Width = 200.0 * ratio;
            Cat.Height = 165.0 * ratio;
            Cat.Margin = new Thickness(0, 500.0 * ratio, 450.0 * ratio, 0);
            Cat.Background = new SolidColorBrush(Colors.Transparent);

            Cat.PreviewTouchDown += new EventHandler<TouchEventArgs>(Cat_TouchDown);

            Themes.Children.Add(border1);
            Themes.Children.Add(border2);
            Themes.Children.Add(border3);
            Themes.Children.Add(border4);
            
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="width"></param>
        public void SetDimensions(double width)
        {
            double ratio = width / 1920.0;

            Themes.Width = 1036 * ratio;
            Themes.Height = 626 * ratio;

            border1.BorderThickness = new Thickness(4.0 * ratio);
            border1.Margin = new Thickness(20.0 * ratio, 20.0 * ratio, 20.0 * ratio, 20.0 * ratio);
            GridTheme1.Width = 470.0 * ratio;
            GridTheme1.Height = 265.0 * ratio;

            border2.BorderThickness = new Thickness(4.0 * ratio);
            border2.Margin = new Thickness(20.0 * ratio, 20.0 * ratio, 20.0 * ratio, 20.0 * ratio);
            GridTheme2.Width = 470.0 * ratio;
            GridTheme2.Height = 265.0 * ratio;

            border3.BorderThickness = new Thickness(4.0 * ratio);
            border3.Margin = new Thickness(20.0 * ratio, 20.0 * ratio, 20.0 * ratio, 20.0 * ratio);
            GridTheme3.Width = 470.0 * ratio;
            GridTheme3.Height = 265.0 * ratio;

            border4.BorderThickness = new Thickness(4.0 * ratio);
            border4.Margin = new Thickness(20.0 * ratio, 20.0 * ratio, 20.0 * ratio, 20.0 * ratio);
            GridTheme4.Width = 470.0 * ratio;
            GridTheme4.Height = 265.0 * ratio;

            Bird.Width = 140.0 * ratio;
            Bird.Height = 165.0 * ratio;
            Bird.Margin = new Thickness(0, 585.0 * ratio, 80.0 * ratio, 0);

            Dragon.Width = 175.0 * ratio;
            Dragon.Height = 275.0 * ratio;
            Dragon.Margin = new Thickness(0, 550.0 * ratio, 40.0 * ratio, 0);

            Cat.Width = 200.0 * ratio;
            Cat.Height = 165.0 * ratio;
            Cat.Margin = new Thickness(0, 500.0 * ratio, 450.0 * ratio, 0);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Theme_Button_TouchDown(object sender, RoutedEventArgs e)
        {
            if (!sessionVM.FullyEnlarged) return;
            sessionVM.Grid.Children.Add(Grid);
            sessionVM.Grid.Children.Add(Themes);
            sessionVM.SessionSVI.CanScale = false;
            Grid.SetZIndex(Grid, 100);
            Grid.SetZIndex(Themes, 101);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridTheme1_TouchDown(object sender, RoutedEventArgs e)
        {
            if (sessionVM.Session.ThemeID == 1)
            {
                sessionVM.Grid.Children.Remove(Grid);
                sessionVM.Grid.Children.Remove(Themes);
                return;
            }

            if (sessionVM.Session.ThemeID == 2)
            {
                sessionVM.Grid.Children.Remove(Bird);
            }

            if (sessionVM.Session.ThemeID == 3)
            {
                sessionVM.Grid.Children.Remove(Dragon);
            }

            if (sessionVM.Session.ThemeID == 4)
            {
                sessionVM.Grid.Children.Remove(Cat);
            }

            double ratio = sessionVM.SessionSVI.Width / 1920.0;

            sessionVM.Session.StopBackgroundSound();
            sessionVM.Session.Theme = new Theme1();
            sessionVM.Session.ThemeID = 1;
            sessionVM.ThemeVM = new ThemeViewModel(sessionVM.Session.Theme, sessionVM);
            sessionVM.Grid.Background = sessionVM.ThemeVM.BackgroundImage;

            sessionVM.Grid.Children.Remove(sessionVM.NbgVM.Grid);
            sessionVM.Grid.Children.Remove(sessionVM.MbgVM.Grid);
            sessionVM.Grid.Children.Remove(sessionVM.Bubbles);
            sessionVM.Grid.Children.Remove(sessionVM.UpdateSound.Grid);
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
            sessionVM.Session.StaveTop.CurrentInstrument = sessionVM.Session.Theme.InstrumentsTop[0];
            sessionVM.Session.StaveBottom.CurrentInstrument = sessionVM.Session.Theme.InstrumentsBottom[0];
            sessionVM.displayTrees(new Thickness(20.0 * ratio, 0, 0, 130.0 * ratio), new Thickness(20.0 * ratio, 0, 0, 580.0 * ratio));

            sessionVM.SetDimensions(sessionVM.SessionSVI.Width, sessionVM.SessionSVI.Height);

            sessionVM.Grid.Children.Add(sessionVM.NbgVM.Grid);
            sessionVM.Grid.Children.Add(sessionVM.MbgVM.Grid);
            sessionVM.Grid.Children.Add(sessionVM.Bubbles);
            sessionVM.Grid.Children.Add(sessionVM.UpdateSound.Grid);
            sessionVM.Grid.Children.Add(sessionVM.Play_Button);
            sessionVM.Grid.Children.Add(sessionVM.Theme_Button);

            sessionVM.Grid.Children.Remove(Grid);
            sessionVM.Grid.Children.Remove(Themes);

            sessionVM.Session.PlayBackgroundSound();
            sessionVM.SessionSVI.CanScale = true;
        }
        
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridTheme2_TouchDown(object sender, RoutedEventArgs e)
        {
            if (sessionVM.Session.ThemeID == 2)
            {
                sessionVM.Grid.Children.Remove(Grid);
                sessionVM.Grid.Children.Remove(Themes);
                return;
            }

            if (sessionVM.Session.ThemeID == 3)
            {
                sessionVM.Grid.Children.Remove(Dragon);
            }

            if (sessionVM.Session.ThemeID == 4)
            {
                sessionVM.Grid.Children.Remove(Cat);
            }
    
            double ratio = sessionVM.SessionSVI.Width / 1920.0;

            sessionVM.Session.StopBackgroundSound();
            sessionVM.Session.Theme = new Theme2();
            sessionVM.Session.ThemeID = 2;
            sessionVM.ThemeVM = new ThemeViewModel(sessionVM.Session.Theme, sessionVM);
            sessionVM.Grid.Background = sessionVM.ThemeVM.BackgroundImage;
            
            sessionVM.Grid.Children.Remove(sessionVM.NbgVM.Grid);
            sessionVM.Grid.Children.Remove(sessionVM.MbgVM.Grid);
            sessionVM.Grid.Children.Remove(sessionVM.Bubbles);
            sessionVM.Grid.Children.Remove(sessionVM.UpdateSound.Grid);
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
            sessionVM.Session.StaveTop.CurrentInstrument = sessionVM.Session.Theme.InstrumentsTop[0];
            sessionVM.Session.StaveBottom.CurrentInstrument = sessionVM.Session.Theme.InstrumentsBottom[0];
            sessionVM.displayTrees(new Thickness(20.0 * ratio, 0, 0, 130 * ratio), new Thickness(20 * ratio, 0, 0, 580 * ratio));

            sessionVM.SetDimensions(sessionVM.SessionSVI.Width, sessionVM.SessionSVI.Height);

            sessionVM.Grid.Children.Add(Bird);
            sessionVM.Grid.Children.Add(sessionVM.NbgVM.Grid);
            sessionVM.Grid.Children.Add(sessionVM.MbgVM.Grid);
            sessionVM.Grid.Children.Add(sessionVM.Bubbles);
            sessionVM.Grid.Children.Add(sessionVM.UpdateSound.Grid);
            sessionVM.Grid.Children.Add(sessionVM.Play_Button);
            sessionVM.Grid.Children.Add(sessionVM.Theme_Button);

            Grid.SetZIndex(Bird, 0);

            sessionVM.Grid.Children.Remove(Grid);
            sessionVM.Grid.Children.Remove(Themes);

            sessionVM.Session.PlayBackgroundSound();
            sessionVM.SessionSVI.CanScale = true;
        }

        private void Bird_TouchDown(object sender, RoutedEventArgs e)
        {
            String effect = "whistle" + (new Random()).Next(1, 5).ToString();
            AudioController.PlaySoundWithString(effect);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridTheme3_TouchDown(object sender, RoutedEventArgs e)
        {
            if (sessionVM.Session.ThemeID == 3)
            {
                sessionVM.Grid.Children.Remove(Grid);
                sessionVM.Grid.Children.Remove(Themes);
                return;
            }

            if (sessionVM.Session.ThemeID == 2)
            {
                sessionVM.Grid.Children.Remove(Bird);
            }

            if (sessionVM.Session.ThemeID == 4)
            {
                sessionVM.Grid.Children.Remove(Cat);
            }
    
            double ratio = sessionVM.SessionSVI.Width / 1920.0;

            sessionVM.Session.StopBackgroundSound();
            sessionVM.Session.Theme = new Theme3();
            sessionVM.Session.ThemeID = 3;
            sessionVM.ThemeVM = new ThemeViewModel(sessionVM.Session.Theme, sessionVM);
            sessionVM.Grid.Background = sessionVM.ThemeVM.BackgroundImage;

            sessionVM.Grid.Children.Remove(sessionVM.NbgVM.Grid);
            sessionVM.Grid.Children.Remove(sessionVM.MbgVM.Grid);
            sessionVM.Grid.Children.Remove(sessionVM.Bubbles);
            sessionVM.Grid.Children.Remove(sessionVM.UpdateSound.Grid);
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
            sessionVM.Session.StaveTop.CurrentInstrument = sessionVM.Session.Theme.InstrumentsTop[0];
            sessionVM.Session.StaveBottom.CurrentInstrument = sessionVM.Session.Theme.InstrumentsBottom[0];
            sessionVM.displayTrees(new Thickness(20.0 * ratio, 0, 0, 130.0 * ratio), new Thickness(20.0 * ratio, 0, 0, 580.0 * ratio));

            sessionVM.SetDimensions(sessionVM.SessionSVI.Width, sessionVM.SessionSVI.Height);

            sessionVM.Grid.Children.Add(Dragon);
            sessionVM.Grid.Children.Add(sessionVM.NbgVM.Grid);
            sessionVM.Grid.Children.Add(sessionVM.MbgVM.Grid);
            sessionVM.Grid.Children.Add(sessionVM.Bubbles);
            sessionVM.Grid.Children.Add(sessionVM.UpdateSound.Grid);
            sessionVM.Grid.Children.Add(sessionVM.Play_Button);
            sessionVM.Grid.Children.Add(sessionVM.Theme_Button);

            Grid.SetZIndex(Dragon, 0);

            sessionVM.Grid.Children.Remove(Grid);
            sessionVM.Grid.Children.Remove(Themes);

            sessionVM.Session.PlayBackgroundSound();
            sessionVM.SessionSVI.CanScale = true;
        }

        private void Dragon_TouchDown(object sender, RoutedEventArgs e)
        {
            String effect = "dragon" + (new Random()).Next(1, 5).ToString();
            AudioController.PlaySoundWithString(effect);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridTheme4_TouchDown(object sender, RoutedEventArgs e)
        {
            if (sessionVM.Session.ThemeID == 4)
            {
                sessionVM.Grid.Children.Remove(Grid);
                sessionVM.Grid.Children.Remove(Themes);
                return;
            }

            if (sessionVM.Session.ThemeID == 2)
            {
                sessionVM.Grid.Children.Remove(Bird);
            }

            if (sessionVM.Session.ThemeID == 3)
            {
                sessionVM.Grid.Children.Remove(Dragon);
            }

            double ratio = sessionVM.SessionSVI.Width / 1920.0;

            sessionVM.Session.StopBackgroundSound();
            sessionVM.Session.Theme = new Theme4();
            sessionVM.Session.ThemeID = 4;
            sessionVM.ThemeVM = new ThemeViewModel(sessionVM.Session.Theme, sessionVM);
            sessionVM.Grid.Background = sessionVM.ThemeVM.BackgroundImage;

            sessionVM.Grid.Children.Remove(sessionVM.NbgVM.Grid);
            sessionVM.Grid.Children.Remove(sessionVM.MbgVM.Grid);
            sessionVM.Grid.Children.Remove(sessionVM.Bubbles);
            sessionVM.Grid.Children.Remove(sessionVM.UpdateSound.Grid);
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
            sessionVM.Session.StaveTop.CurrentInstrument = sessionVM.Session.Theme.InstrumentsTop[0];
            sessionVM.Session.StaveBottom.CurrentInstrument = sessionVM.Session.Theme.InstrumentsBottom[0];
            sessionVM.displayTrees(new Thickness(20.0 * ratio, 0, 0, 130.0 * ratio), new Thickness(20.0 * ratio, 0, 0, 580.0 * ratio));

            sessionVM.SetDimensions(sessionVM.SessionSVI.Width, sessionVM.SessionSVI.Height);
            sessionVM.Session.PlayBackgroundSound();

            sessionVM.Grid.Children.Add(Cat);
            sessionVM.Grid.Children.Add(sessionVM.NbgVM.Grid);
            sessionVM.Grid.Children.Add(sessionVM.MbgVM.Grid);
            sessionVM.Grid.Children.Add(sessionVM.Bubbles);
            sessionVM.Grid.Children.Add(sessionVM.UpdateSound.Grid);
            sessionVM.Grid.Children.Add(sessionVM.Play_Button);
            sessionVM.Grid.Children.Add(sessionVM.Theme_Button);

            Grid.SetZIndex(Cat, 0);

            sessionVM.Grid.Children.Remove(Grid);
            sessionVM.Grid.Children.Remove(Themes);
            sessionVM.SessionSVI.CanScale = true;
        }

        private void Cat_TouchDown(object sender, RoutedEventArgs e)
        {
            String effect = "chat" + (new Random()).Next(1, 7).ToString();
            AudioController.PlaySoundWithString(effect);
        }
    }
}
