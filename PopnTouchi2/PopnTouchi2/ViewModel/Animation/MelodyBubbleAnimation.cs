using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Input;
using PopnTouchi2.Model.Enums;

namespace PopnTouchi2.ViewModel.Animation
{
    /// <summary>
    /// Defines all animations linked to a Melody Bubble Item.
    /// </summary>
    public class MelodyBubbleAnimation : Animation
    {
        #region Properties
        /// <summary>
        /// Parameter.
        /// Private attribute of current sessionVM.
        /// </summary>
        private SessionViewModel sessionVM;

        /// <summary>
        /// Parameter.
        /// Private melody in the MelodyBubbleViewModel
        /// </summary>
        private MelodyBubbleViewModel melodyBubbleVM;

        /// <summary>
        /// Property
        /// True if the melody is played with the upper instrument
        /// </summary>
        public Boolean playUp;

        /// <summary>
        /// Property.
        /// Center of the given bubble to animate
        /// </summary>
        private Point bubbleCenter;

        /// <summary>
        /// Property.
        /// The position of the melody
        /// </summary>
        public Point PositionMelody { get; set; }

        #endregion

         #region Constructors
        /// <summary>
        /// MelodyBubbleAnimation's Constructor.
        /// </summary>
        /// <param name="mbVM"></param>
        /// <param name="s"></param>
        public MelodyBubbleAnimation(MelodyBubbleViewModel mbVM, SessionViewModel s) 
            : base()
        {
            melodyBubbleVM = mbVM;
            sessionVM = s;
            SVItem = mbVM.SVItem;
            ParentSV = mbVM.ParentSV;
            canAnimate = true;
            playUp = true;

            DispatcherTimer.Tick += new EventHandler(t_Tick);

            SVItem.ContainerManipulationCompleted += touchLeave;

            SVItem.PreviewTouchDown += touchDown;
            Animate();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Note Bubble Animation 
        /// Moves the NoteBubble randomly on the screen.
        /// </summary>
        private void Animate()
        {
            if (canAnimate)
            {
                PointAnimation centerAnimation = new PointAnimation();
                SineEase ease = new SineEase();
                ease.EasingMode = EasingMode.EaseInOut;
                Random r = new Random();
                Double xOffset = (-2) * (r.Next() % 2 - .5) * r.Next(50, 100);
                Double yOffset = (-2) * (r.Next() % 2 - .5) * r.Next(50, 100);

                if (SVItem.Center.X + xOffset > ParentSV.ActualWidth)
                    xOffset = ParentSV.ActualWidth - SVItem.Center.X;
                if (SVItem.Center.X + xOffset < 0)
                    xOffset = 0 - SVItem.Center.X;
                if (SVItem.Center.Y + yOffset > ParentSV.ActualHeight)
                    yOffset = ParentSV.ActualHeight - SVItem.Center.Y;
                if (SVItem.Center.Y + yOffset < 630 * ParentSV.ActualHeight / 1080)
                    yOffset = (630 * ParentSV.ActualHeight / 1080) - SVItem.Center.Y;

                centerAnimation.From = SVItem.Center;
                centerAnimation.To = new Point(SVItem.Center.X + xOffset, SVItem.Center.Y + yOffset);
                centerAnimation.Duration = new Duration(TimeSpan.FromSeconds(r.Next(9, 21)));
                centerAnimation.AccelerationRatio = .3;
                centerAnimation.DecelerationRatio = .3;
                centerAnimation.FillBehavior = FillBehavior.HoldEnd;
                Storyboard.Children.Add(centerAnimation);
                Storyboard.SetTarget(centerAnimation, SVItem);
                Storyboard.SetTargetProperty(centerAnimation, new PropertyPath(ScatterViewItem.CenterProperty));

                centerAnimation.Completed += new EventHandler(centerAnimation_Completed);

                Storyboard.Begin();
            }
        }

        /// <summary>
        /// Stops a current animation performing.
        /// </summary>
        public void StopAnimation()
        {
            canAnimate = false;
            DispatcherTimer.Stop();
            Storyboard.Pause();
            SVItem.Center = SVItem.ActualCenter;
            SVItem.Orientation = SVItem.Orientation;
            Storyboard.Remove();
        }
        #endregion

        #region Events
        /// <summary>
        /// Event used to animate bubbles
        /// Used as a clock
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void t_Tick(object sender, EventArgs e)
        {
            DispatcherTimer.Stop();
            Animate();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void centerAnimation_Completed(object sender, EventArgs e)
        {
            SVItem.Center = SVItem.ActualCenter;
            Storyboard.Remove();
            Storyboard.Children = new TimelineCollection();
            Random r = new Random();
            DispatcherTimer.Interval = TimeSpan.FromMilliseconds(r.Next(3000, 9000));
            DispatcherTimer.Start();
        }
        /// <summary>
        /// Event occured when a MelodyBubble is released
        /// Magnetise the current bubble
        /// Add the melody in the right stave
        /// Move the bubble to its final place
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void touchLeave(object sender, ContainerManipulationCompletedEventArgs e)
        {
            ScatterViewItem bubble = new ScatterViewItem();
            bubble = e.Source as ScatterViewItem;
            bubbleCenter = bubble.ActualCenter;

            // int width = int.Parse(GetWidth.Text);
            // int height = int.Parse(GetHeight.Text);
            int width = (int)sessionVM.Grid.ActualWidth;
            int height = (int)sessionVM.Grid.ActualHeight;
            bubbleCenter.X = bubbleCenter.X * 1920 / width;
            bubbleCenter.Y = bubbleCenter.Y * 1080 / height;



            if (bubbleCenter.X <= 90) bubbleCenter.X = 120;
            else if (bubbleCenter.X >= 1830) bubbleCenter.X = 1800;
            else bubbleCenter.X = Math.Floor((bubbleCenter.X + 30) / 60) * 60;

            //"Applatissement" de la portée (MAJ : Switch -> Tableau !)

            int offset = GlobalVariables.ManipulationGrid.ElementAtOrDefault((int)((long)bubbleCenter.X / 60));
            bubbleCenter.Y += offset;

            PositionMelody = new Point(bubbleCenter.X, bubbleCenter.Y);
            

            //Y dans le cadre portée ?
            //Si oui, animation
            //pas de else
            if (bubbleCenter.Y < 576 && bubbleCenter.Y > 165)
            {
                if (bubbleCenter.Y < 370)
                {
                    if (bubbleCenter.Y >= 344) bubbleCenter.Y = 344;
                    bubbleCenter.Y = Math.Floor((bubbleCenter.Y + 6.0) / 20.0) * 20.0 + 4.0;

                    sessionVM.Session.StaveTop.AddMelody(melodyBubbleVM.MelodyBubble, ((int)PositionMelody.X - 120) / 60);

                }
                else
                {
                    if (bubbleCenter.Y <= 395) bubbleCenter.Y = 395;
                    bubbleCenter.Y = Math.Floor((bubbleCenter.Y + 15.0) / 20.0) * 20.0 - 5.0;

                    sessionVM.Session.StaveBottom.AddMelody(melodyBubbleVM.MelodyBubble, ((int)PositionMelody.X - 120) / 60);
                }

                bubbleCenter.Y -= offset;

                bubbleCenter.X = bubbleCenter.X * width / 1920;
                bubbleCenter.Y = bubbleCenter.Y * height / 1080;

                #region STB
                Storyboard stb = new Storyboard();
                PointAnimation moveCenter = new PointAnimation();

                moveCenter.From = bubble.ActualCenter;
                moveCenter.To = bubbleCenter;
                moveCenter.Duration = new Duration(TimeSpan.FromSeconds(0.15));
                bubble.Center = bubbleCenter;
                moveCenter.FillBehavior = FillBehavior.Stop;

                stb.Children.Add(moveCenter);

                Storyboard.SetTarget(moveCenter, bubble);
                Storyboard.SetTargetProperty(moveCenter, new PropertyPath(ScatterViewItem.CenterProperty));
                #endregion
          
                bubble.Center = bubbleCenter;
                moveCenter.Completed += new EventHandler(moveCenter_Completed);

                stb.Begin(SVItem);

            }
            else
            {
                canAnimate = true;
                Animate();
            }
        }

        void moveCenter_Completed(object sender, EventArgs e)
        {
            List<NoteViewModel> ListOfNotes = melodyBubbleVM.melodyToListOfNote(PositionMelody);
            for (int i = 0; i < ListOfNotes.Count; i++)
            {
                sessionVM.Notes.Items.Add(ListOfNotes[i].SVItem);
            }
            sessionVM.Bubbles.Items.Remove(melodyBubbleVM.SVItem);

        }

        /// <summary>
        /// When the user touchDown on a bubble, it makes a little sound effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void touchDown(object sender, TouchEventArgs e)
        {
            StopAnimation();
            if (playUp)
            {
                sessionVM.Session.StaveTop.melody = melodyBubbleVM.MelodyBubble.Melody;
                playUp = false;
                sessionVM.Session.StaveTop.StopMelody();
                sessionVM.Session.StaveTop.PlayMelody(); 
            }
            else
            {
                sessionVM.Session.StaveBottom.melody = melodyBubbleVM.MelodyBubble.Melody;
                playUp = true;
                sessionVM.Session.StaveBottom.StopMelody();
                sessionVM.Session.StaveBottom.PlayMelody();
            }

        }
        #endregion
    }
}
