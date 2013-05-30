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
using PopnTouchi2.Infrastructure;

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
        private NoteViewModel noteVM { get; set; }

        /// <summary>
        /// Boolean
        /// Indicate if there is a Note where the user
        /// release the NoteViewModel
        /// </summary>
        private bool NothingAtThisPlace;

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
        public void Animate()
        {
            if (canAnimate)
            {
                PointAnimation centerAnimation = new PointAnimation();
                SineEase ease = new SineEase();
                ease.EasingMode = EasingMode.EaseInOut;
                Random r = GlobalVariables.GlobalRandom;
                Double xOffset = (-2) * (r.Next() % 2 - .5) * r.Next(50, 100);
                Double yOffset = (-2) * (r.Next() % 2 - .5) * r.Next(50, 100);

                centerAnimation.Duration = new Duration(TimeSpan.FromSeconds(r.Next(9, 21)));
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
                    centerAnimation.Duration = new Duration(TimeSpan.FromSeconds(r.Next(2, 4)));
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
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void t_Tick(object sender, EventArgs e)
        {
            DispatcherTimer.Stop();
            Animate();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void centerAnimation_Completed(object sender, EventArgs e)
        {
            SVItem.Center = SVItem.ActualCenter;
            Storyboard.Remove();
            Storyboard.Children = new TimelineCollection();
            Random r = new Random();
            DispatcherTimer.Interval = TimeSpan.FromMilliseconds(r.Next(3000, 9000));
            DispatcherTimer.Start();
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

            int width = (int)sessionVM.Grid.ActualWidth;
            int height = (int)sessionVM.Grid.ActualHeight;
            bubbleCenter.X = bubbleCenter.X * 1920.0 / width;
            bubbleCenter.Y = bubbleCenter.Y * 1080.0 / height;

            if (bubbleCenter.X <= 90.0) bubbleCenter.X = 120.0;
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
                }
                else
                {
                    if (bubbleCenter.Y <= 395) bubbleCenter.Y = 395;
                    bubbleCenter.Y = Math.Floor((bubbleCenter.Y + 15.0) / 20.0) * 20.0 - 5.0;             
                }

                noteBubbleVM.NoteBubble.Note = new Note(converter.getOctave(bubbleCenter.Y), noteBubbleVM.NoteBubble.Note.Duration, converter.getPitch(bubbleCenter.Y), positionNote, noteBubbleVM.NoteBubble.Note.Sharp, noteBubbleVM.NoteBubble.Note.Flat);

                virtualCenter = bubbleCenter;

                bubbleCenter.Y -= offset;

                bubbleCenter.X = bubbleCenter.X * width / 1920.0;
                bubbleCenter.Y = bubbleCenter.Y * height / 1080.0;


                noteBubbleVM.SVItem.Center = bubbleCenter;
                MyPoint noteBubbleCenter = new MyPoint(bubbleCenter);
                for (int i = 0; i < sessionVM.NotesOnStave.Count && NothingAtThisPlace; i++)
                {
                    if (noteBubbleCenter.QuasiEquals(sessionVM.NotesOnStave[i].SVItem.Center))
                    {
                        NothingAtThisPlace = false;
                        noteVM = sessionVM.NotesOnStave[i];
                    }
                    else NothingAtThisPlace = true;
                }

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
            }
        }

        void moveCenter_Completed(object sender, EventArgs e)
        {
            int positionNote = (int)(virtualCenter.X - 120) / 60;
            double betweenStave = (350 - GlobalVariables.ManipulationGrid.ElementAtOrDefault(noteBubbleVM.NoteBubble.Note.Position + 2)) * (sessionVM.SessionSVI.ActualHeight / 1080);
            bool isUp = (bubbleCenter.Y < betweenStave);
            Converter converter = new Converter();

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
                    {
                        changeline = noteVM.Note.UpSemiTone();
                    }
                    if (noteBubbleVM.NoteBubble.Note.Flat)
                    {
                        changeline = noteVM.Note.DownSemiTone();
                    }
                    sessionVM.Bubbles.Items.Remove(noteBubbleVM.SVItem);
                    
                    double y = converter.getCenterY(isUp, noteVM.Note);
                    if (y < 80)
                    {
                        sessionVM.NotesOnStave.Remove(noteVM);
                        if (isUp)
                            sessionVM.Session.StaveTop.RemoveNote(noteVM.Note);
                        else
                            sessionVM.Session.StaveBottom.RemoveNote(noteVM.Note);

                        noteVM.Note.Sharp = false;
                        noteVM.Note.Flat = false;
                        noteVM.Note.Position = -1;
                        noteVM.Note.Pitch = "la";
                        NoteBubbleViewModel nbVM = new NoteBubbleViewModel(noteVM.SVItem.Center, new NoteBubble(noteVM.Note), sessionVM.Bubbles, sessionVM);
                        sessionVM.Bubbles.Items.Add(nbVM.SVItem);

                        String effect = "pop" + (new Random()).Next(1, 5).ToString();
                        AudioController.PlaySoundWithString(effect);
                    }
                    else
                    {
                        y *= sessionVM.SessionSVI.ActualHeight / 1080.0;
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
            StopAnimation();
            String effect = "discovery" + (new Random()).Next(1, 5).ToString();
            AudioController.PlaySoundWithString(effect);
        }
        #endregion
    }
}
