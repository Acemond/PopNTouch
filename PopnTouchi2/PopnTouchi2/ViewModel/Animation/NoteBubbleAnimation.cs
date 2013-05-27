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
        /// TODO Description détaillée de ce que fait cette méthode
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
            bubbleCenter.X = bubbleCenter.X * 1920 / width;
            bubbleCenter.Y = bubbleCenter.Y * 1080 / height;

            if (bubbleCenter.X <= 90) bubbleCenter.X = 120;
            else if (bubbleCenter.X >= 1830) bubbleCenter.X = 1800;
            else bubbleCenter.X = Math.Floor((bubbleCenter.X + 30) / 60) * 60;

            //"Applatissement" de la portée (MAJ : Switch -> Tableau !)
            int offset = GlobalVariables.ManipulationGrid[((long)bubbleCenter.X / 60)];
            bubbleCenter.Y += offset;

          

            //Y dans le cadre portée ?
            //Si oui, animation
            //pas de else
            if (bubbleCenter.Y < 630 && bubbleCenter.Y > 105)
            {
                if (bubbleCenter.Y < 370)
                {
                    if (bubbleCenter.Y >= 335) bubbleCenter.Y = 335;
                    bubbleCenter.Y = Math.Floor((bubbleCenter.Y - 20) / 25) * 25 + 35; //-20 et 35 pour 50
                }
                else
                {
                    if (bubbleCenter.Y <= 405) bubbleCenter.Y = 405;
                    bubbleCenter.Y = Math.Floor((bubbleCenter.Y + 10) / 25) * 25 + 5; //20 et 5 pour 50
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

                moveCenter.FillBehavior = FillBehavior.Stop;

                stb.Children.Add(moveCenter);

                Storyboard.SetTarget(moveCenter, bubble);
                Storyboard.SetTargetProperty(moveCenter, new PropertyPath(ScatterViewItem.CenterProperty));
                #endregion

                bubble.Center = bubbleCenter;
                moveCenter.Completed += new EventHandler(moveCenter_Completed);

                stb.Begin(SVItem);

         /*       Point p = bubbleCenter;
                p.X = p.X * 1920 / width;
                p.Y = p.Y * 1080 / height;
                MessageBox.Show(p.ToString());*/
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
            int positionNote = (int)(bubbleCenter.X - 120) / 60;
            Converter c = new Converter();
           
            if (noteBubbleVM.NoteBubble.Note.Sharp || noteBubbleVM.NoteBubble.Note.Flat)
            {
                bool goOn = true;
                foreach (NoteViewModel n in sessionVM.NotesOnStave)
                {
                    if (goOn)
                    {
                        if (n.SVItem.ActualCenter == noteBubbleVM.SVItem.ActualCenter)
                        {
                            noteVM = n;
                            noteVM.Note.Flat = noteBubbleVM.NoteBubble.Note.Flat;
                            noteVM.Note.Sharp = noteBubbleVM.NoteBubble.Note.Sharp;
                            if (bubbleCenter.Y < (370 * (int)sessionVM.Grid.ActualHeight / 1080))
                            {
                                sessionVM.Session.StaveTop.CurrentInstrument.PlayNote(noteVM.Note);
                            }
                            else
                            {
                                sessionVM.Session.StaveBottom.CurrentInstrument.PlayNote(noteVM.Note);
                            }
                            sessionVM.Bubbles.Items.Remove(noteBubbleVM.SVItem);
                            goOn = false;
                        }
                    }
                    else break;
                }
            }

            else
            {

                if (bubbleCenter.Y <= 160)
                {
                    noteBubbleVM.NoteBubble.Note.Octave = 2;
                    String Pitch = c.PositionToPitch.ElementAt(((GlobalVariables.StaveTopFirstDo - GlobalVariables.HeightOfOctave) - (int)bubbleCenter.Y) / 25);
                    noteBubbleVM.NoteBubble.Note.Pitch = Pitch;
                    sessionVM.Session.StaveTop.AddNote(noteBubbleVM.NoteBubble.Note, positionNote);
                }
                else if (bubbleCenter.Y <= GlobalVariables.StaveTopFirstDo)
                {
                    noteBubbleVM.NoteBubble.Note.Octave = 1;
                    String Pitch = c.PositionToPitch.ElementAt((GlobalVariables.StaveTopFirstDo - (int)bubbleCenter.Y) / 25);
                    noteBubbleVM.NoteBubble.Note.Pitch = Pitch;
                    sessionVM.Session.StaveTop.AddNote(noteBubbleVM.NoteBubble.Note, positionNote);
                }

                else if (bubbleCenter.Y <= (GlobalVariables.StaveBottomFirstDo - GlobalVariables.HeightOfOctave))
                {
                    noteBubbleVM.NoteBubble.Note.Octave = 2;
                    String Pitch = c.PositionToPitch.ElementAt(((GlobalVariables.StaveBottomFirstDo - GlobalVariables.HeightOfOctave) - (int)bubbleCenter.Y) / 25);
                    noteBubbleVM.NoteBubble.Note.Pitch = Pitch;
                    sessionVM.Session.StaveBottom.AddNote(noteBubbleVM.NoteBubble.Note, positionNote);
                }

                else if (bubbleCenter.Y <= GlobalVariables.StaveBottomFirstDo)
                {
                    noteBubbleVM.NoteBubble.Note.Octave = 1;
                    String Pitch = c.PositionToPitch.ElementAt((GlobalVariables.StaveBottomFirstDo - (int)bubbleCenter.Y) / 25);
                    noteBubbleVM.NoteBubble.Note.Pitch = Pitch;
                    sessionVM.Session.StaveBottom.AddNote(noteBubbleVM.NoteBubble.Note, positionNote);
                }


                noteVM = new NoteViewModel(bubbleCenter, noteBubbleVM.NoteBubble.Note, sessionVM.Notes, sessionVM);
                sessionVM.Notes.Items.Add(noteVM.SVItem);
                sessionVM.NotesOnStave.Add(noteVM);
                if (bubbleCenter.Y < (370 * (int)sessionVM.Grid.ActualHeight / 1080))
                {
                    sessionVM.Session.StaveTop.CurrentInstrument.PlayNote(noteBubbleVM.NoteBubble.Note);
                }
                else
                {
                    sessionVM.Session.StaveBottom.CurrentInstrument.PlayNote(noteBubbleVM.NoteBubble.Note);
                }
                sessionVM.Bubbles.Items.Remove(noteBubbleVM.SVItem);

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
            String effect = "discovery";
            Random r = new Random();
            int nb = r.Next(1, 5);
            effect += nb.ToString();
            AudioController.PlaySoundWithString(effect);
        }
        #endregion
    }
}
