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
using PopnTouchi2.Infrastructure;
using System.Windows.Controls;

namespace PopnTouchi2.ViewModel.Animation
{
    /// <summary>
    /// Defines all animations linked to a NoteBubble item.
    /// </summary>
    public class NoteBubbleAnimation : Animation
    {
        #region Properties
        /// <summary>
        /// Parameter.
        /// Private
        /// </summary>
        private SessionViewModel sessionVM;

        /// <summary>
        /// Property.
        /// The NoteBubbleViewModel associated
        /// </summary>
        private NoteBubbleViewModel noteBubbleVM { get; set; }

        /// <summary>
        /// Property.
        /// Center of the given bubble to animate
        /// </summary>
        private Point bubbleCenter;

        /// <summary>
        /// Parameter
        /// </summary>
        private Point virtualCenter;

        /// <summary>
        /// Property.
        /// The NoteViewModel at the same place
        /// Used for Flat and Sharp
        /// </summary>
        private NoteViewModel noteVM;

        /// <summary>
        /// Timer used to wait
        /// </summary>
        private DispatcherTimer noteBubbleDT;

        /// <summary>
        /// Boolean
        /// Indicate if there is a Note where the user
        /// release the NoteViewModel
        /// </summary>
        private bool NothingAtThisPlace;

        /// <summary>
        /// Boolean
        /// True if the bubble is dropped on the StaveTop
        /// </summary>
        private bool bubbleDroppedTopStave;

        #endregion

        #region Constructors
        /// <summary>
        /// NoteBubbleAnimation Constructor.
        /// </summary>
        /// <param name="nbVM">Linked with the NoteBubbleVM</param>
        /// <param name="s">The current SessionViewModel</param>
        public NoteBubbleAnimation(NoteBubbleViewModel nbVM, SessionViewModel s)
            : base()
        {
            sessionVM = s;
            noteBubbleVM = nbVM;
            SVItem = nbVM.SVItem;
            ParentSV = nbVM.ParentSV;
            canAnimate = true;
            NothingAtThisPlace = true;

            noteBubbleDT = new DispatcherTimer();
            noteBubbleDT.Tick += new EventHandler(t_Tick);

            SVItem.ContainerManipulationCompleted += touchLeave;

            SVItem.PreviewTouchDown += touchDown;
            Animate();
        }
        #endregion

        #region Methods
        
        /// <summary>
        /// Begin the bubble animation
        /// </summary>
        public void BeginBubbleAnimation()
        {
            Random r = GlobalVariables.GlobalRandom;
            noteBubbleDT.Interval = TimeSpan.FromMilliseconds(r.Next(100, 10000));
            noteBubbleDT.Start();
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
        /// Note Bubble Animation 
        /// Moves the NoteBubble randomly on the screen.
        /// </summary>
        public void MoveFromLocation()
        {
            if (canAnimate)
            {
                PointAnimation centerAnimation = new PointAnimation();
                SineEase ease = new SineEase();
                ease.EasingMode = EasingMode.EaseInOut;
                Random r = GlobalVariables.GlobalRandom;
                double xOffset = (-2) * (r.Next() % 2 - .5) * r.Next(40, 75);
                double yOffset = (-2) * (r.Next() % 2 - .5) * r.Next(40, 75);

                centerAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(r.Next(1000, 2000)));
                centerAnimation.AccelerationRatio = .2;
                centerAnimation.DecelerationRatio = .8;

                if (SVItem.Center.X + xOffset > ParentSV.ActualWidth)
                    xOffset = (ParentSV.ActualWidth - SVItem.Center.X) - (double)r.Next(50);
                if (SVItem.Center.X + xOffset < 0)
                    xOffset = 0 - SVItem.Center.X + (double)r.Next(50);
                if (SVItem.Center.Y + yOffset > ParentSV.ActualHeight)
                    yOffset = ParentSV.ActualHeight - SVItem.Center.Y - (double)r.Next(50);
                if (SVItem.Center.Y + yOffset < 630.0 * ParentSV.ActualHeight / 1080.0)
                    yOffset = ((630.0 * ParentSV.ActualHeight / 1080.0) - SVItem.Center.Y) + (double)r.Next(50);

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
            noteBubbleDT.Stop();
            Storyboard.Pause();
            SVItem.Center = SVItem.ActualCenter;
            SVItem.Orientation = SVItem.Orientation;
            Storyboard.Remove();
        }
        #endregion

        #region Events
        /// <summary>
        /// Used to wait and then animate the bubble
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void t_Tick(object sender, EventArgs e)
        {
            noteBubbleDT.Stop();
            Animate();
        }

        /// <summary>
        /// Event occured when the animation is finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void centerAnimation_Completed(object sender, EventArgs e)
        {
            SVItem.Center = SVItem.ActualCenter;
            Storyboard.Remove();
            Storyboard.Children = new TimelineCollection();
            Random r = new Random();
            noteBubbleDT.Interval = TimeSpan.FromMilliseconds(r.Next(1000, 10000));
            noteBubbleDT.Start();
        }

        /// <summary>
        /// Event occured when a Bubble is released
        /// Magnetise the current bubble
        /// Add the note in the right stave
        /// Move the bubble to its final place
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void touchLeave(object sender, ContainerManipulationCompletedEventArgs e)
        {
            ScatterViewItem bubble = new ScatterViewItem();
            bubble = e.Source as ScatterViewItem;
            bubbleCenter = bubble.ActualCenter;
            NothingAtThisPlace = true;

            int width = (int)sessionVM.Grid.ActualWidth;
            int height = (int)sessionVM.Grid.ActualHeight;
            bubbleCenter.X = bubbleCenter.X * 1920.0 / width;
            bubbleCenter.Y = bubbleCenter.Y * 1080.0 / height;

            if (bubbleCenter.X < 150.0) bubbleCenter.X = 150.0;
            else if (bubbleCenter.X >= 1830.0) bubbleCenter.X = 1800.0;
            else bubbleCenter.X = Math.Floor((bubbleCenter.X + 30.0) / 60.0) * 60.0;

            //"Applatissement" de la portée (MAJ : Switch -> Tableau !)
            int offset = GlobalVariables.ManipulationGrid.ElementAtOrDefault((int)(bubbleCenter.X / 60.0));
            bubbleCenter.Y += offset;

            Converter converter = new Converter();
            int positionNote = (int)(bubbleCenter.X - 120) / 60;

            //Y dans le cadre portée ?
            //Si oui, animation
            //pas de else
            if (bubbleCenter.Y < 576.0 && bubbleCenter.Y > 165.0)
            {
                if (bubbleCenter.Y < 370.0)
                {
                    if (bubbleCenter.Y >= 344.0) bubbleCenter.Y = 344.0;
                    bubbleCenter.Y = Math.Floor((bubbleCenter.Y + 6.0) / 20.0) * 20.0 + 4.0;
                    bubbleDroppedTopStave = true;
                }
                else
                {
                    if (bubbleCenter.Y <= 395) bubbleCenter.Y = 395;
                    bubbleCenter.Y = Math.Floor((bubbleCenter.Y + 15.0) / 20.0) * 20.0 - 5.0;
                    bubbleDroppedTopStave = false;
                }

                noteBubbleVM.NoteBubble.Note = new Note(converter.getOctave(bubbleCenter.Y), noteBubbleVM.NoteBubble.Note.Duration, converter.getPitch(bubbleCenter.Y), positionNote, noteBubbleVM.NoteBubble.Note.Sharp, noteBubbleVM.NoteBubble.Note.Flat);

                virtualCenter = bubbleCenter;

                bubbleCenter.Y -= offset;

                bubbleCenter.X = bubbleCenter.X * width / 1920.0;
                bubbleCenter.Y = bubbleCenter.Y * height / 1080.0;
                
                #region STB
                Storyboard stb = new Storyboard();
                PointAnimation moveCenter = new PointAnimation();

                moveCenter.From = bubble.ActualCenter;
                moveCenter.To = bubbleCenter;
                moveCenter.Duration = new Duration(TimeSpan.FromSeconds(0.15));

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
                DisplayPreviewGrid(false);
            }
        }

        /// <summary>
        /// Event occured when the NoteBubble finished its move (to the right place)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void moveCenter_Completed(object sender, EventArgs e)
        {
            Converter converter = new Converter();
            DisplayPreviewGrid(false);

            noteBubbleVM.SVItem.Center = bubbleCenter;
            for (int i = 0; i < sessionVM.NotesOnStave.Count && NothingAtThisPlace; i++)
            {
                if ((int)((virtualCenter.X - 120.0) / 60.0) == sessionVM.NotesOnStave[i].Note.Position
                    && !sessionVM.NotesOnStave[i].Picked
                    && converter.getOctave(virtualCenter.Y) == sessionVM.NotesOnStave[i].Note.Octave
                    && converter.getPitch(virtualCenter.Y) == sessionVM.NotesOnStave[i].Note.Pitch
                    && ((bubbleDroppedTopStave && sessionVM.Session.StaveTop.Notes.Contains(sessionVM.NotesOnStave[i].Note)) ||
                        (!bubbleDroppedTopStave && sessionVM.Session.StaveBottom.Notes.Contains(sessionVM.NotesOnStave[i].Note))))
                {
                    if (noteBubbleVM.NoteBubble.Note.Duration == NoteValue.alteration)
                    {
                        NothingAtThisPlace = false;
                        noteVM = sessionVM.NotesOnStave[i];
                    }
                    else
                    {
                        sessionVM.NotesOnStave[i].Animation.BackToBubbleFormat(true);
                        NothingAtThisPlace = true;
                    }
                }
                else NothingAtThisPlace = true;
            }

            int positionNote = (int)((virtualCenter.X - 120.0) / 60.0);
            double betweenStave = (350 - GlobalVariables.ManipulationGrid.ElementAtOrDefault(noteBubbleVM.NoteBubble.Note.Position + 2)) * (sessionVM.SessionSVI.ActualHeight / 1080);
            bool isUp = (bubbleCenter.Y < betweenStave);

            if (NothingAtThisPlace)
            {
                NoteViewModel noteViewModel = new NoteViewModel(bubbleCenter, noteBubbleVM.NoteBubble.Note, sessionVM.Notes, sessionVM);
                if (!noteViewModel.Note.Sharp && !noteViewModel.Note.Flat)
                {
                    if (isUp)
                    {
                        sessionVM.Session.StaveTop.CurrentInstrument.PlayNote(noteViewModel.Note);
                        sessionVM.Session.StaveTop.AddNote(noteViewModel.Note, positionNote);
                    }
                    else
                    {
                        sessionVM.Session.StaveBottom.CurrentInstrument.PlayNote(noteViewModel.Note);
                        sessionVM.Session.StaveBottom.AddNote(noteViewModel.Note, positionNote);
                    }
                    sessionVM.Bubbles.Items.Remove(noteBubbleVM.SVItem);
                    sessionVM.NbgVM.NoteBubbleVMs.Remove(noteBubbleVM);
                    sessionVM.Notes.Items.Add(noteViewModel.SVItem);
                    sessionVM.NotesOnStave.Add(noteViewModel);
                }
            }
            else
            {
                //If the NoteBubbleViewModel is a (#) or (b)
                if (noteBubbleVM.NoteBubble.Note.Sharp || noteBubbleVM.NoteBubble.Note.Flat)
                {
                    bool changeline = false;
                    if (noteBubbleVM.NoteBubble.Note.Sharp)
                        changeline = noteVM.Note.UpSemiTone();
                    if (noteBubbleVM.NoteBubble.Note.Flat)
                        changeline = noteVM.Note.DownSemiTone();
                    
                    double y = converter.getCenterY(isUp, noteVM.Note);
                    if (y < 80)
                    {
                        if (noteBubbleVM.NoteBubble.Note.Sharp)
                            noteVM.Note.DownSemiTone();
                        if (noteBubbleVM.NoteBubble.Note.Flat)
                            noteVM.Note.UpSemiTone();

                        canAnimate = true;
                        Animate();
                    }
                    else
                    {
                        y *= sessionVM.SessionSVI.ActualHeight / 1080.0;
                        sessionVM.NbgVM.NoteBubbleVMs.Remove(noteBubbleVM);
                        sessionVM.Bubbles.Items.Remove(noteBubbleVM.SVItem);
                        sessionVM.Notes.Items.Remove(noteVM.SVItem);
                        sessionVM.NotesOnStave.Remove(noteVM);
                        noteVM = new NoteViewModel(new Point(bubbleCenter.X, y), noteVM.Note, sessionVM.Notes, sessionVM);

                        if (isUp)
                        {
                            sessionVM.Session.StaveTop.RemoveNote(noteVM.Note);
                            sessionVM.Session.StaveTop.CurrentInstrument.PlayNote(noteVM.Note);
                            sessionVM.Session.StaveTop.AddNote(noteVM.Note, positionNote);
                        }
                        else
                        {
                            sessionVM.Session.StaveBottom.RemoveNote(noteVM.Note);
                            sessionVM.Session.StaveBottom.CurrentInstrument.PlayNote(noteVM.Note);
                            sessionVM.Session.StaveBottom.AddNote(noteVM.Note, positionNote);
                        }
                        sessionVM.Notes.Items.Add(noteVM.SVItem);
                        sessionVM.NotesOnStave.Add(noteVM);
                    }
                }

                else
                {
                    canAnimate = true;
                    Animate();
                }
            }
        }

        /// <summary>
        /// When the user touchDown on a bubble, it makes a little sound effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void touchDown(object sender, TouchEventArgs e)
        {
            DisplayPreviewGrid(true);
            StopAnimation();
            String effect = "discovery" + (new Random()).Next(1, 5).ToString();
            AudioController.PlaySoundWithString(effect);
        }

        /// <summary>
        /// Display the preview grid
        /// </summary>
        /// <param name="appear"></param>
        private void DisplayPreviewGrid(bool appear)
        {
            Storyboard pGSTB = new Storyboard();
            DoubleAnimation previewGridAnimation = new DoubleAnimation();

            if (appear)
            {
                previewGridAnimation.From = sessionVM.previewGrid.Opacity;
                previewGridAnimation.To = 0.25;
            }
            else
            {
                previewGridAnimation.From = sessionVM.previewGrid.Opacity;
                previewGridAnimation.To = 0;
            }
            previewGridAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
            previewGridAnimation.FillBehavior = FillBehavior.HoldEnd;
            pGSTB.Children.Add(previewGridAnimation);
            Storyboard.SetTarget(previewGridAnimation, sessionVM.previewGrid);
            Storyboard.SetTargetProperty(previewGridAnimation, new PropertyPath(Grid.OpacityProperty));

            pGSTB.Begin();
        }
        #endregion
    }
}
