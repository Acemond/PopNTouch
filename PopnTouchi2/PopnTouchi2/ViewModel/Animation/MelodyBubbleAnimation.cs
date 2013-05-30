﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Input;
using PopnTouchi2.Model.Enums;
using System.Windows.Controls;

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
        public int playUp;

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

        private DispatcherTimer topHighlightDt;
        private DispatcherTimer bottomHighlightDt;

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
            playUp = 0;

            DispatcherTimer.Tick += new EventHandler(t_Tick);

            SVItem.ContainerManipulationCompleted += touchLeave;

            SVItem.PreviewTouchDown += touchDown;
            Animate();
        }
        #endregion

        #region Methods
        public void BeginBubbleAnimation()
        {
            Random r = GlobalVariables.GlobalRandom;
            DispatcherTimer.Interval = TimeSpan.FromMilliseconds(r.Next(100, 10000));
            DispatcherTimer.Start();
        }

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
                Random r = GlobalVariables.GlobalRandom;
                Double xOffset = (-2) * (r.Next() % 2 - .5) * r.Next(50, 100);
                Double yOffset = (-2) * (r.Next() % 2 - .5) * r.Next(50, 100);

                centerAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(r.Next(9000, 21000)));
                centerAnimation.AccelerationRatio = .3;
                centerAnimation.DecelerationRatio = .3;

                if (SVItem.Center.X + xOffset > ParentSV.ActualWidth)
                    xOffset = (ParentSV.ActualWidth - SVItem.Center.X) - (double)r.Next(50);
                if (SVItem.Center.X + xOffset < 0)
                    xOffset = 0 - SVItem.Center.X + (double)r.Next(50);
                if (SVItem.Center.Y + yOffset > ParentSV.ActualHeight)
                    yOffset = ParentSV.ActualHeight - SVItem.Center.Y - (double)r.Next(50);
                if (SVItem.Center.Y + yOffset < 630.0 * ParentSV.ActualHeight / 1080.0)
                    yOffset = ((630.0 * ParentSV.ActualHeight / 1080.0) - SVItem.Center.Y) + (double)r.Next(50);
                if (SVItem.Center.Y < 630.0 * ParentSV.ActualHeight / 1080.0)
                {
                    centerAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(r.Next(2000, 4000)));
                    centerAnimation.DecelerationRatio = .7;
                }

                centerAnimation.From = SVItem.Center;
                centerAnimation.To = new Point(SVItem.Center.X + xOffset, SVItem.Center.Y + yOffset);
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



            if (bubbleCenter.X < 150.0) bubbleCenter.X = 150.0;
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

                    sessionVM.Session.StaveTop.StopMelody();
                    sessionVM.Session.StaveTop.AddMelody(melodyBubbleVM.MelodyBubble, ((int)PositionMelody.X - 120) / 60);

                }
                else
                {
                    if (bubbleCenter.Y <= 395) bubbleCenter.Y = 395;
                    bubbleCenter.Y = Math.Floor((bubbleCenter.Y + 15.0) / 20.0) * 20.0 - 5.0;

                    sessionVM.Session.StaveBottom.StopMelody();
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
                sessionVM.NotesOnStave.Add(ListOfNotes[i]);
                sessionVM.Notes.Items.Add(ListOfNotes[i].SVItem);
                
            }
            sessionVM.Bubbles.Items.Remove(melodyBubbleVM.SVItem);
            sessionVM.MbgVM.MelodyBubbleVMs.Remove(melodyBubbleVM);

        }

        /// <summary>
        /// When the user touchDown on a bubble, it makes a little sound effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void touchDown(object sender, TouchEventArgs e)
        {
            StopAnimation();
            int time = (melodyBubbleVM.MelodyBubble.Melody.Notes.Last().Position + 1) * (30000 / sessionVM.Session.Bpm);
            if ((playUp % 4) == 0)
            {
                sessionVM.Session.StaveTop.melody = melodyBubbleVM.MelodyBubble.Melody;
                
                try
                {
                    DisplayHighlightGrid(true, true);
                    topHighlightDt = new DispatcherTimer();
                    topHighlightDt.Interval = TimeSpan.FromMilliseconds(time);
                    topHighlightDt.Tick += new EventHandler(topHighlightDt_Tick);
                    topHighlightDt.Start();
                }
                catch (Exception exc) { }
                sessionVM.Session.StaveTop.StopMelody();
                sessionVM.Session.StaveTop.PlayMelody();
                playUp++;
            }
            else if ((playUp % 4) == 2)
            {
                sessionVM.Session.StaveBottom.melody = melodyBubbleVM.MelodyBubble.Melody;
                try
                {
                    DisplayHighlightGrid(true, false);
                    bottomHighlightDt = new DispatcherTimer();
                    bottomHighlightDt.Interval = TimeSpan.FromMilliseconds(time);
                    bottomHighlightDt.Tick += new EventHandler(bottomHighlightDt_Tick);
                    bottomHighlightDt.Start();
                }
                catch (Exception exc) { }
                sessionVM.Session.StaveBottom.StopMelody();
                sessionVM.Session.StaveBottom.PlayMelody();
                playUp++;
            }
            else
            {
                try
                {
                    topHighlightDt.Stop();
                    bottomHighlightDt.Stop();
                }
                catch (Exception exc) { }
                sessionVM.Session.StaveTop.StopMelody();
                sessionVM.Session.StaveBottom.StopMelody();
                playUp++;
            }
     
        }

        void topHighlightDt_Tick(object sender, EventArgs e)
        {
            topHighlightDt.Stop();
            DisplayHighlightGrid(false, true);
        }

        void bottomHighlightDt_Tick(object sender, EventArgs e)
        {
            bottomHighlightDt.Stop();
            DisplayHighlightGrid(false, false);
        }



        private void DisplayHighlightGrid(bool appear, bool top)
        {
            Storyboard pGSTB = new Storyboard();
            DoubleAnimation previewGridAnimation = new DoubleAnimation();

            if (appear)
            {
                if (top) previewGridAnimation.From = sessionVM.topStaveHighlight.Opacity;
                else previewGridAnimation.From = sessionVM.bottomStaveHighlight.Opacity;
                previewGridAnimation.To = 1;
            }
            else
            {
                if (top) previewGridAnimation.From = sessionVM.topStaveHighlight.Opacity;
                else previewGridAnimation.From = sessionVM.bottomStaveHighlight.Opacity;
                previewGridAnimation.To = 0;
            }
            previewGridAnimation.Duration = new Duration(TimeSpan.FromSeconds(.2));
            previewGridAnimation.FillBehavior = FillBehavior.HoldEnd;
            pGSTB.Children.Add(previewGridAnimation);
            if(top) Storyboard.SetTarget(previewGridAnimation, sessionVM.topStaveHighlight);
            else Storyboard.SetTarget(previewGridAnimation, sessionVM.bottomStaveHighlight);
            Storyboard.SetTargetProperty(previewGridAnimation, new PropertyPath(Grid.OpacityProperty));

            pGSTB.Begin();
        }
        #endregion
    }
}
