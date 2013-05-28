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

            int positionNote = (int)(bubbleCenter.X - 120) / 60;
            Converter converter = new Converter();

            //Y dans le cadre portée ?
            //Si oui, animation
            //pas de else
            if (bubbleCenter.Y < 576.0 && bubbleCenter.Y > 165.0)
            {
                if (bubbleCenter.Y < 370.0)
                {
                    if (bubbleCenter.Y >= 344.0) bubbleCenter.Y = 344.0;
                    bubbleCenter.Y = Math.Floor((bubbleCenter.Y + 6.0) / 20.0) * 20.0 + 4.0;

                    noteBubbleVM.NoteBubble.Note = new Note(converter.getOctave(bubbleCenter.Y), noteBubbleVM.NoteBubble.Note.Duration, converter.getPitch(bubbleCenter.Y), positionNote, noteBubbleVM.NoteBubble.Note.Sharp, noteBubbleVM.NoteBubble.Note.Flat);
                }
                else
                {
                    if (bubbleCenter.Y <= 395) bubbleCenter.Y = 395;
                    bubbleCenter.Y = Math.Floor((bubbleCenter.Y + 15.0) / 20.0) * 20.0 - 5.0;

                    noteBubbleVM.NoteBubble.Note = new Note(converter.getOctave(bubbleCenter.Y), noteBubbleVM.NoteBubble.Note.Duration, converter.getPitch(bubbleCenter.Y), positionNote, noteBubbleVM.NoteBubble.Note.Sharp, noteBubbleVM.NoteBubble.Note.Flat);
                }

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
            }
        }

        void moveCenter_Completed(object sender, EventArgs e)
        {
            NoteViewModel noteVM = null;
            double centerX = bubbleCenter.X * 1920.0 / sessionVM.Grid.ActualWidth;
            int positionNote = (int)(centerX - 120) / 60;
            Converter c = new Converter();
            double betweenStave = (350 - GlobalVariables.ManipulationGrid[noteBubbleVM.NoteBubble.Note.Position + 2]) * (sessionVM.SessionSVI.ActualHeight / 1080);
            bool isUp = (bubbleCenter.Y < betweenStave);

            if (noteBubbleVM.NoteBubble.Note.Sharp || noteBubbleVM.NoteBubble.Note.Flat)
            {
                bool goOn = true;

                for(int i = 0; i< sessionVM.NotesOnStave.Count && goOn; i++){


                    bool changeLine = false;
                    if (sessionVM.NotesOnStave[i].SVItem.ActualCenter == bubbleCenter)
                    {
                        noteVM = sessionVM.NotesOnStave[i];
                        if(noteBubbleVM.NoteBubble.Note.Flat)
                            changeLine = noteVM.Note.DownSemiTone();
                        else
                            changeLine = noteVM.Note.UpSemiTone();

                        sessionVM.NotesOnStave.Remove(sessionVM.NotesOnStave[i]);
                        sessionVM.NotesOnStave.Add(noteVM);
                        sessionVM.Bubbles.Items.Remove(noteBubbleVM.SVItem);
                        if (changeLine)
                        {
                            sessionVM.Notes.Items.Remove(noteVM.SVItem);
                            double y = c.getCenterY(isUp, noteVM.Note);
                            if (y == 0)
                            {
                                sessionVM.NotesOnStave.Remove(noteVM);
                                if (isUp)
                                    sessionVM.Session.StaveTop.RemoveNote(noteVM.Note);
                                else
                                    sessionVM.Session.StaveBottom.RemoveNote(noteVM.Note);

                                Point center = new Point();
                                center = noteVM.SVItem.ActualCenter;

                                NoteBubbleViewModel nbVM = new NoteBubbleViewModel(center, new NoteBubble(noteVM.Note), sessionVM.Bubbles, sessionVM);
                                nbVM.NoteBubble.Note.Sharp = false;
                                nbVM.NoteBubble.Note.Flat = false;
                                sessionVM.Bubbles.Items.Add(nbVM.SVItem);

                                String effect = "pop" + (new Random()).Next(1, 5).ToString();
                                AudioController.PlaySoundWithString(effect);
                            }
                            else
                            {
                                y = (y - GlobalVariables.ManipulationGrid[positionNote + 2]) * (sessionVM.SessionSVI.ActualHeight / 1080);
                                noteVM = new NoteViewModel(new Point(bubbleCenter.X, y), noteVM.Note, sessionVM.Notes, sessionVM);
                                sessionVM.Notes.Items.Add(noteVM.SVItem);
                                if (isUp)
                                {
                                    sessionVM.Session.StaveTop.CurrentInstrument.PlayNote(noteVM.Note);
                                    sessionVM.Session.StaveTop.AddNote(noteVM.Note, positionNote);
                                }
                                else
                                {
                                    sessionVM.Session.StaveBottom.CurrentInstrument.PlayNote(noteVM.Note);
                                    sessionVM.Session.StaveBottom.AddNote(noteVM.Note, positionNote);

                                }
                            }
                        }
                       
                        goOn = false;
                    }
                }
            }

            else
            {
                noteVM = new NoteViewModel(bubbleCenter, noteBubbleVM.NoteBubble.Note, sessionVM.Notes, sessionVM);
                sessionVM.Notes.Items.Add(noteVM.SVItem);
                sessionVM.NotesOnStave.Add(noteVM);
                
                if (isUp)
                {
                    sessionVM.Session.StaveTop.CurrentInstrument.PlayNote(noteBubbleVM.NoteBubble.Note);
                    sessionVM.Session.StaveTop.AddNote(noteBubbleVM.NoteBubble.Note, positionNote);
                }
                else
                {
                    sessionVM.Session.StaveBottom.CurrentInstrument.PlayNote(noteBubbleVM.NoteBubble.Note);
                    sessionVM.Session.StaveBottom.AddNote(noteBubbleVM.NoteBubble.Note, positionNote);
                }
                sessionVM.Bubbles.Items.Remove(noteBubbleVM.SVItem);
                sessionVM.NbgVM.NoteBubbleVMs.Remove(noteBubbleVM);
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
