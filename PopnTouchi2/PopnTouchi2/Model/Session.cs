using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Model.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Media.Animation;
//using Microsoft.Xna.Framework.Audio; //TODO

namespace PopnTouchi2
{
    /// <summary>
    /// Manages a game session environment contained in a Grid.
    /// </summary>
    public class Session : Grid
    {
        #region Properties
        /// <summary>
        /// Property.
        /// Session's MelodyBubbleGenerator instance.
        /// </summary>
        public MelodyBubbleGenerator MelodyBubbleGenerator { get; set; }
        /// <summary>
        /// Property.
        /// Session's NoteBubbleGenerator instance.
        /// </summary>
        public NoteBubbleGenerator NoteBubbleGenerator { get; set; }
        /// <summary>
        /// Property.
        /// Session's upper Stave instance.
        /// </summary>
        public Stave StaveTop { get; set; }
        /// <summary>
        /// Property.
        /// Session's lower Stave instance.
        /// </summary>
        public Stave StaveBottom { get; set; }
        /// <summary>
        /// Property.
        /// Session's Theme instance.
        /// </summary>
        public Theme Theme { get; set; }
        /// <summary>
        /// Property.
        /// Session's Bubbles' ScatterView instance.
        /// </summary>
        public ScatterView Bubbles { get; set; }

        //TODO delete (Adrien only)
        public Storyboard stbTest { get; set; }
        public SurfaceButton Reducer { get; set; }
        private bool reduced;

        /// <summary>
        /// Property
        /// Session's Background sound
        /// </summary>
        //public Cue BackgroundSound { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Session Constructor.
        /// Initializes interface elements accordingly to a default Theme.
        /// </summary>
        public Session()
        {
            //////////////////////////////////////////////////
            Opacity = 0;

            Storyboard stb = new Storyboard();
            DoubleAnimation openingAnimation = new DoubleAnimation();
            DoubleAnimation heightAnimation = new DoubleAnimation();
            DoubleAnimation widthAnimation = new DoubleAnimation();
            ExponentialEase ease = new ExponentialEase();
            ease.EasingMode = EasingMode.EaseOut;
            ease.Exponent = 2;

            openingAnimation.From = 0;
            openingAnimation.To = 1;
            openingAnimation.Duration = new Duration(TimeSpan.FromSeconds(.8));
            openingAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(openingAnimation);
            Storyboard.SetTarget(openingAnimation, this);
            Storyboard.SetTargetProperty(openingAnimation, new PropertyPath(Grid.OpacityProperty));

            heightAnimation.From = 1080 * 0.8;
            heightAnimation.To = 1080;
            heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(.8));
            heightAnimation.EasingFunction = ease;
            heightAnimation.FillBehavior = FillBehavior.HoldEnd;
            stb.Children.Add(heightAnimation);
            Storyboard.SetTarget(heightAnimation, this);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(Grid.HeightProperty));

            widthAnimation.From = 1920 * 0.8;
            widthAnimation.To = 1920 ;
            widthAnimation.Duration = new Duration(TimeSpan.FromSeconds(.8));
            widthAnimation.EasingFunction = ease;
            widthAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(widthAnimation);
            Storyboard.SetTarget(widthAnimation, this);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Grid.WidthProperty));

            stb.Begin();
            ///////////////////////////////////////////////////

            Theme = new Theme1(); //Could be randomized
            Background = Theme.BackgroundImage;

            NoteBubbleGenerator = new NoteBubbleGenerator(Theme);
            Children.Add(NoteBubbleGenerator);
            MelodyBubbleGenerator = new MelodyBubbleGenerator(Theme);
            Children.Add(MelodyBubbleGenerator);

            Bubbles = new ScatterView();
            Bubbles.Visibility = Visibility.Visible;
            Children.Add(Bubbles);

            StaveTop = new Stave(Theme.InstrumentsTop[0]);
            StaveBottom = new Stave(Theme.InstrumentsBottom[0]);
            
            //TODO : DELETE (Adrien only), Test Button //
            Reducer = new SurfaceButton();
            reduced = false;
            Reducer.Width = 100;
            Reducer.Height = 25;
            Reducer.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            Reducer.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Reducer.Background = Brushes.Red;
            Reducer.Content = "Reduce !";
            Children.Add(Reducer);

            Reducer.Click += new RoutedEventHandler(Reducer_Click);
            /////////////////////////////////////////////

            
            //TODO uncomment when ok
            //sound methods
            /*BackgroundSound = Theme.sound;
            BackgroundSound.Play();*/
        }

        //TODO Delete
        void Reducer_Click(object sender, RoutedEventArgs e)
        {
            if (reduced)
            {
                Reducer.Content = "Reduce !";
                Reducer.Background = Brushes.Red;

                ScatterViewItem svi = (ScatterViewItem)Parent;
                Desktop mainDesktop = (Desktop)((ScatterView)svi.Parent).Parent;
                
                Storyboard stb = new Storyboard();
                PointAnimation centerPosAnimation = new PointAnimation();
                DoubleAnimation heightAnimation = new DoubleAnimation();
                DoubleAnimation widthAnimation = new DoubleAnimation();
                DoubleAnimation gridHeightAnimation = new DoubleAnimation();
                DoubleAnimation gridWidthAnimation = new DoubleAnimation();
                DoubleAnimation orientationAnimation = new DoubleAnimation();
                ThicknessAnimation borderAnimation = new ThicknessAnimation();

                centerPosAnimation.From = svi.ActualCenter;
                centerPosAnimation.To = new Point(mainDesktop.ActualWidth / 2, mainDesktop.ActualHeight / 2);
                centerPosAnimation.Duration = new Duration(TimeSpan.FromSeconds(.75));
                centerPosAnimation.FillBehavior = FillBehavior.HoldEnd;
                stb.Children.Add(centerPosAnimation);
                Storyboard.SetTarget(centerPosAnimation, svi);
                Storyboard.SetTargetProperty(centerPosAnimation, new PropertyPath(ScatterViewItem.CenterProperty));

                orientationAnimation.From = svi.ActualOrientation;
                if(svi.ActualOrientation <= 180)
                    orientationAnimation.To = 0;
                else
                    orientationAnimation.To = 360;
                orientationAnimation.Duration = new Duration(TimeSpan.FromSeconds(.75));
                orientationAnimation.FillBehavior = FillBehavior.HoldEnd;
                stb.Children.Add(orientationAnimation);
                Storyboard.SetTarget(orientationAnimation, svi);
                Storyboard.SetTargetProperty(orientationAnimation, new PropertyPath(ScatterViewItem.OrientationProperty));

                heightAnimation.From = svi.ActualHeight ;
                heightAnimation.To = svi.ActualHeight * 4;
                heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                heightAnimation.EasingFunction = new ExponentialEase();
                heightAnimation.AccelerationRatio = 1;
                heightAnimation.FillBehavior = FillBehavior.HoldEnd;
                stb.Children.Add(heightAnimation);
                Storyboard.SetTarget(heightAnimation, svi);
                Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(ScatterViewItem.HeightProperty));

                widthAnimation.From = svi.ActualWidth;
                widthAnimation.To = svi.ActualWidth  * 4;
                widthAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                widthAnimation.EasingFunction = new ExponentialEase();
                widthAnimation.AccelerationRatio = 1;
                widthAnimation.FillBehavior = FillBehavior.HoldEnd;
                stb.Children.Add(widthAnimation);
                Storyboard.SetTarget(widthAnimation, svi);
                Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(ScatterViewItem.WidthProperty));

                borderAnimation.From = new Thickness(15);
                borderAnimation.To = new Thickness(60);
                borderAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                borderAnimation.FillBehavior = FillBehavior.HoldEnd;
                stb.Children.Add(borderAnimation);
                Storyboard.SetTarget(borderAnimation, svi);
                Storyboard.SetTargetProperty(borderAnimation, new PropertyPath(ScatterViewItem.BorderThicknessProperty));

                gridHeightAnimation.From = ActualHeight;
                gridHeightAnimation.To = ActualHeight * 4;
                gridHeightAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                gridHeightAnimation.EasingFunction = new ExponentialEase();
                gridHeightAnimation.AccelerationRatio = 1;
                gridHeightAnimation.FillBehavior = FillBehavior.HoldEnd;
                stb.Children.Add(gridHeightAnimation);
                Storyboard.SetTarget(gridHeightAnimation, this);
                Storyboard.SetTargetProperty(gridHeightAnimation, new PropertyPath(Grid.HeightProperty));

                gridWidthAnimation.From = ActualWidth;
                gridWidthAnimation.To = ActualWidth * 4;
                gridWidthAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                gridWidthAnimation.EasingFunction = new ExponentialEase();
                gridWidthAnimation.AccelerationRatio = 1;
                gridWidthAnimation.FillBehavior = FillBehavior.HoldEnd;
                stb.Children.Add(gridWidthAnimation);
                Storyboard.SetTarget(gridWidthAnimation, this);
                Storyboard.SetTargetProperty(gridWidthAnimation, new PropertyPath(Grid.WidthProperty));

                widthAnimation.Completed += new EventHandler(stb_enlarge_Completed);

                /*Width = ActualWidth * 4;
                Height = ActualHeight * 4;*/

                stb.Begin(this);


                this.NoteBubbleGenerator.Width = NoteBubbleGenerator.ActualWidth * 4;
                this.NoteBubbleGenerator.Height = NoteBubbleGenerator.ActualHeight * 4;
                this.MelodyBubbleGenerator.Width = MelodyBubbleGenerator.ActualWidth * 4;
                this.MelodyBubbleGenerator.Height = MelodyBubbleGenerator.ActualHeight * 4;

                reduced = false;
            }
            else
            {
                Reducer.Content = "Enlarge !";
                Reducer.Background = Brushes.Green;

                Storyboard stb = new Storyboard();
                DoubleAnimation heightAnimation = new DoubleAnimation();
                DoubleAnimation widthAnimation = new DoubleAnimation();

                ExponentialEase ease = new ExponentialEase();
                ease.EasingMode = EasingMode.EaseInOut;
                ease.Exponent = 1.5;

                heightAnimation.From = ActualHeight;
                heightAnimation.To = ActualHeight / 4;
                heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                heightAnimation.EasingFunction = ease;
                heightAnimation.AccelerationRatio = .4;
                heightAnimation.DecelerationRatio = .1;
                heightAnimation.FillBehavior = FillBehavior.Stop;
                stb.Children.Add(heightAnimation);
                Storyboard.SetTarget(heightAnimation, this);
                Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(Grid.HeightProperty));

                widthAnimation.From = ActualWidth;
                widthAnimation.To = ActualWidth / 4;
                widthAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                widthAnimation.EasingFunction = ease;
                widthAnimation.AccelerationRatio = .4;
                widthAnimation.DecelerationRatio = .1;
                widthAnimation.FillBehavior = FillBehavior.Stop;
                stb.Children.Add(widthAnimation);
                Storyboard.SetTarget(widthAnimation, this);
                Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Grid.WidthProperty));

                Height = ActualHeight / 4;
                Width = ActualWidth / 4;

                widthAnimation.Completed += new EventHandler(stb_Completed);

                stb.Begin(this);

                this.NoteBubbleGenerator.Width = NoteBubbleGenerator.ActualWidth / 4;
                this.NoteBubbleGenerator.Height = NoteBubbleGenerator.ActualHeight / 4;
                this.MelodyBubbleGenerator.Width = MelodyBubbleGenerator.ActualWidth / 4;
                this.MelodyBubbleGenerator.Height = MelodyBubbleGenerator.ActualHeight / 4;
            }
        }

        void stb_enlarge_Completed(object sender, EventArgs e)
        {
            ScatterViewItem svi = (ScatterViewItem)Parent;
            Desktop mainDesktop = (Desktop)((ScatterView)svi.Parent).Parent;
            svi.Content = null;

            ((ScatterView)svi.Parent).Items.Remove(svi);
            mainDesktop.Children.Add(this);
        }

        void stb_Completed(object sender, EventArgs e)
        {
            ScatterViewItem svi = new ScatterViewItem();
            Desktop mainDesktop = (Desktop)Parent;
            mainDesktop.Children.Remove(this);
            svi.Width = Width;
            svi.Height = Height;
            svi.Content = this;
            svi.BorderBrush = Brushes.White;
            svi.Orientation = 0;
            svi.Center = new Point(mainDesktop.ActualWidth / 2, mainDesktop.ActualHeight / 2);
            mainDesktop.Photos.Items.Add(svi);


            Storyboard stb = new Storyboard();
            ThicknessAnimation borderAnimation = new ThicknessAnimation();
            DoubleAnimation heightAnimation = new DoubleAnimation();
            DoubleAnimation widthAnimation = new DoubleAnimation();

            borderAnimation.From = new Thickness(0);
            borderAnimation.To = new Thickness(15);
            borderAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            borderAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(borderAnimation);
            Storyboard.SetTarget(borderAnimation, svi);
            Storyboard.SetTargetProperty(borderAnimation, new PropertyPath(ScatterViewItem.BorderThicknessProperty));

            heightAnimation.From = Height;
            heightAnimation.To = Height + 30;
            heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            heightAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(heightAnimation);
            Storyboard.SetTarget(heightAnimation, svi);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(ScatterViewItem.HeightProperty));

            widthAnimation.From = Width;
            widthAnimation.To = Width + 30;
            widthAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            widthAnimation.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(widthAnimation);
            Storyboard.SetTarget(widthAnimation, svi);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(ScatterViewItem.WidthProperty));

            svi.BorderThickness = new Thickness(15);
            svi.Width += 30;
            svi.Height += 30;

            widthAnimation.Completed += new EventHandler(stb_border_Completed);

            stb.Begin(this);


            reduced = true;
        }

        void stb_border_Completed(object sender, EventArgs e)
        {
            ScatterViewItem svi = (ScatterViewItem)Parent;
            Desktop mainDesktop = (Desktop)((ScatterView)svi.Parent).Parent;

            stbTest = new Storyboard();
            PointAnimation centerPosAnimation = new PointAnimation();
            DoubleAnimation orientationAnimation = new DoubleAnimation();


            ExponentialEase ease = new ExponentialEase();
            ease.EasingMode = EasingMode.EaseOut;
            ease.Exponent = 1.5;

            Random r = new Random();
            Point newCenter = new Point(r.Next((int)mainDesktop.ActualWidth), r.Next((int)mainDesktop.ActualHeight));
            Double newOrientation = r.Next(-180,180);

            centerPosAnimation.From = svi.ActualCenter;
            centerPosAnimation.To = newCenter;
            centerPosAnimation.Duration = new Duration(TimeSpan.FromSeconds(5));
            centerPosAnimation.DecelerationRatio = .9;
            centerPosAnimation.EasingFunction = ease;
            centerPosAnimation.FillBehavior = FillBehavior.Stop;
            stbTest.Children.Add(centerPosAnimation);
            Storyboard.SetTarget(centerPosAnimation, svi);
            Storyboard.SetTargetProperty(centerPosAnimation, new PropertyPath(ScatterViewItem.CenterProperty));

            orientationAnimation.From = svi.ActualOrientation;
            orientationAnimation.To = newOrientation;
            orientationAnimation.Duration = new Duration(TimeSpan.FromSeconds(5));
            orientationAnimation.DecelerationRatio = .9;
            orientationAnimation.EasingFunction = ease;
            orientationAnimation.FillBehavior = FillBehavior.Stop;
            stbTest.Children.Add(orientationAnimation);
            Storyboard.SetTarget(orientationAnimation, svi);
            Storyboard.SetTargetProperty(orientationAnimation, new PropertyPath(ScatterViewItem.OrientationProperty));

            svi.Center = newCenter;
            svi.Orientation = newOrientation;

            stbTest.Begin();
            svi.PreviewTouchDown += new EventHandler<System.Windows.Input.TouchEventArgs>(Session_TouchDown);
        }

        //Stops the animation and set coordinates to current location
        void Session_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            stbTest.Pause();
            ScatterViewItem svi = (ScatterViewItem)Parent;
            Desktop mainDesktop = (Desktop)((ScatterView)svi.Parent).Parent;

            svi.Center = svi.ActualCenter;
            svi.Orientation = svi.ActualOrientation;
            stbTest.Remove();
        }
        /////////////

        #endregion

        #region Methods
        /// <summary>
        /// Changes the global Bpm with a new value.
        /// </summary>
        /// <param name="newBpm">The new Bpm value</param>
        public void ChangeBpm(int newBpm)
        {
            GlobalVariables.bpm = newBpm;
        }

        /// <summary>
        /// TODO
        /// </summary>
        public void Reduce()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// TODO
        /// </summary>
        public void Enlarge()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
