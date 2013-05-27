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
    public class NoteAnimation : Animation
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
        private NoteViewModel noteVM { get; set; }

        /// <summary>
        /// Property.
        /// Center of the given Note to animate
        /// </summary>
        private Point NoteCenter;
        #endregion

        #region Constructors
        /// <summary>
        /// NoteBubbleAnimation Constructor.
        /// </summary>
        /// <param name="nVM">Linked with the NoteViewModel</param>
        /// <param name="s">The current SessionViewModel</param>
        public NoteAnimation(NoteViewModel nVM, SessionViewModel s)
            : base()
        {
            sessionVM = s;
            noteVM = nVM;
            SVItem = nVM.SVItem;
            ParentSV = nVM.ParentSV;

            SVItem.ContainerManipulationCompleted += touchLeave;

            SVItem.PreviewTouchDown += touchDown;
        }
        #endregion

        #region Events

        /// <summary>
        /// TODO Description détaillée de ce que fait cette méthode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void touchLeave(object sender, ContainerManipulationCompletedEventArgs e)
        {
            ScatterViewItem bubble = new ScatterViewItem();
            bubble = e.Source as ScatterViewItem;
            NoteCenter = bubble.ActualCenter;

            // int width = int.Parse(GetWidth.Text);
            // int height = int.Parse(GetHeight.Text);
            int width = (int)sessionVM.Grid.ActualWidth;
            int height = (int)sessionVM.Grid.ActualHeight;
            NoteCenter.X = NoteCenter.X * 1920 / width;
            NoteCenter.Y = NoteCenter.Y * 1080 / height;

            if (NoteCenter.X <= 90) NoteCenter.X = 120;
            else if (NoteCenter.X >= 1830) NoteCenter.X = 1800;
            else NoteCenter.X = Math.Floor((NoteCenter.X + 30) / 60) * 60;

            //"Applatissement" de la portée (MAJ : Switch -> Tableau !)
            int offset = GlobalVariables.ManipulationGrid[((long)NoteCenter.X / 60)];
            NoteCenter.Y += offset;

           
            //Y dans le cadre portée ?
            //Si oui, animation
            //pas de else
            if (NoteCenter.Y < 630 && NoteCenter.Y > 105)
            {
                if (NoteCenter.Y < 370)
                {
                    if (NoteCenter.Y >= 335) NoteCenter.Y = 335;
                    NoteCenter.Y = Math.Floor((NoteCenter.Y - 20) / 25) * 25 + 35; //-20 et 35 pour 50
                }
                else
                {
                    if (NoteCenter.Y <= 405) NoteCenter.Y = 405;
                    NoteCenter.Y = Math.Floor((NoteCenter.Y + 10) / 25) * 25 + 5; //20 et 5 pour 50
                }

                NoteCenter.Y -= offset;

                NoteCenter.X = NoteCenter.X * width / 1920;
                NoteCenter.Y = NoteCenter.Y * height / 1080;

                #region STB
                Storyboard stb = new Storyboard();
                PointAnimation moveCenter = new PointAnimation();

                moveCenter.From = bubble.ActualCenter;
                moveCenter.To = NoteCenter;
                moveCenter.Duration = new Duration(TimeSpan.FromSeconds(0.15));
                bubble.Center = NoteCenter;
                moveCenter.FillBehavior = FillBehavior.Stop;

                stb.Children.Add(moveCenter);

                Storyboard.SetTarget(moveCenter, bubble);
                Storyboard.SetTargetProperty(moveCenter, new PropertyPath(ScatterViewItem.CenterProperty));

                #endregion

                moveCenter.Completed += new EventHandler(moveCenter_Completed);
                stb.Begin(SVItem);
            }
            else
            {
                Point center = new Point();
                center = noteVM.SVItem.ActualCenter;
                sessionVM.NotesOnStave.Remove(noteVM);
                sessionVM.Notes.Items.Remove(noteVM.SVItem);

                NoteBubbleViewModel nbVM = new NoteBubbleViewModel(center, new NoteBubble(noteVM.Note), sessionVM.Bubbles, sessionVM);
                sessionVM.Bubbles.Items.Add(nbVM.SVItem);
            }
        }

        void moveCenter_Completed(object sender, EventArgs e)
        {
            int positionNote = (int)(NoteCenter.X - 120) / 60;
            Converter c = new Converter();

            NoteCenter.Y = NoteCenter.Y * 1080 / sessionVM.SessionSVI.ActualHeight - GlobalVariables.ManipulationGrid[positionNote];

            if (NoteCenter.Y <= 160)
            {
                noteVM.Note.Octave = 2;
                String Pitch = c.PositionToPitch.ElementAtOrDefault(((GlobalVariables.StaveTopFirstDo - GlobalVariables.HeightOfOctave) - (int)NoteCenter.Y) / 25);
                noteVM.Note.Pitch = Pitch;
                sessionVM.Session.StaveTop.AddNote(noteVM.Note, positionNote);
            }
            else if (NoteCenter.Y <= GlobalVariables.StaveTopFirstDo)
            {
                noteVM.Note.Octave = 1;
                String Pitch = c.PositionToPitch.ElementAtOrDefault((GlobalVariables.StaveTopFirstDo - (int)NoteCenter.Y) / 25);
                noteVM.Note.Pitch = Pitch;
                sessionVM.Session.StaveTop.AddNote(noteVM.Note, positionNote);
            }

            else if (NoteCenter.Y <= (GlobalVariables.StaveBottomFirstDo - GlobalVariables.HeightOfOctave))
            {
                noteVM.Note.Octave = 2;
                String Pitch = c.PositionToPitch.ElementAtOrDefault(((GlobalVariables.StaveBottomFirstDo - GlobalVariables.HeightOfOctave) - (int)NoteCenter.Y) / 25);
                noteVM.Note.Pitch = Pitch;
                sessionVM.Session.StaveBottom.AddNote(noteVM.Note, positionNote);
            }

            else if (NoteCenter.Y <= GlobalVariables.StaveBottomFirstDo)
            {
                noteVM.Note.Octave = 1;
                String Pitch = c.PositionToPitch.ElementAtOrDefault((GlobalVariables.StaveBottomFirstDo - (int)NoteCenter.Y) / 25);
                noteVM.Note.Pitch = Pitch;
                sessionVM.Session.StaveBottom.AddNote(noteVM.Note, positionNote);
            }

            sessionVM.NotesOnStave.Add(noteVM);
            if (NoteCenter.Y < (370 * (int)sessionVM.Grid.ActualHeight / 1080))
            {
                sessionVM.Session.StaveTop.CurrentInstrument.PlayNote(noteVM.Note);
            }
            else
            {
                sessionVM.Session.StaveBottom.CurrentInstrument.PlayNote(noteVM.Note);
            }
        }

        /// <summary>
        /// When the user touchDown on a bubble, it makes a little sound effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void touchDown(object sender, TouchEventArgs e)
        {
            ////Si la bulle est sur la portée et qu'on la touche, elle s'enleve de la portee
            sessionVM.Session.StaveTop.RemoveNote(noteVM.Note);
            sessionVM.Session.StaveBottom.RemoveNote(noteVM.Note);
            sessionVM.NotesOnStave.Remove(noteVM);
        }
        #endregion
    }
}
