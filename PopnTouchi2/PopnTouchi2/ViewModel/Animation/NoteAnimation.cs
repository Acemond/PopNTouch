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
            ScatterViewItem Note = new ScatterViewItem();
            Note = e.Source as ScatterViewItem;
            NoteCenter = Note.ActualCenter;

            int width = (int)sessionVM.Grid.ActualWidth;
            int height = (int)sessionVM.Grid.ActualHeight;
            NoteCenter.X = NoteCenter.X * 1920.0 / width;
            NoteCenter.Y = NoteCenter.Y * 1080.0 / height;

            if (NoteCenter.X <= 90.0) NoteCenter.X = 120.0;
            else if (NoteCenter.X >= 1830.0) NoteCenter.X = 1800.0;
            else NoteCenter.X = Math.Floor((NoteCenter.X + 30.0) / 60.0) * 60.0;

            //"Applatissement" de la portée (MAJ : Switch -> Tableau !)
            int offset = GlobalVariables.ManipulationGrid[(int)(NoteCenter.X / 60.0)];
            NoteCenter.Y += offset;

            int positionNote = (int)(NoteCenter.X - 120) / 60;
            Converter converter = new Converter();
           
            //Y dans le cadre portée ?
            //Si oui, animation
            //pas de else
            if (NoteCenter.Y < 576.0 && NoteCenter.Y > 165.0)
            {
                if (NoteCenter.Y < 370.0)
                {
                    if (NoteCenter.Y >= 344) NoteCenter.Y = 344;
                    NoteCenter.Y = Math.Floor((NoteCenter.Y + 6.0) / 20.0) * 20.0 + 4.0;

                    noteVM.Note = new Note(converter.getOctave(NoteCenter.Y), noteVM.Note.Duration, converter.getPitch(NoteCenter.Y), positionNote);
                    sessionVM.Session.StaveTop.AddNote(noteVM.Note, positionNote);
                }
                else
                {
                    if (NoteCenter.Y <= 395) NoteCenter.Y = 395;
                    NoteCenter.Y = Math.Floor((NoteCenter.Y + 15.0) / 20.0) * 20.0 - 5.0;

                    noteVM.Note = new Note(converter.getOctave(NoteCenter.Y), noteVM.Note.Duration, converter.getPitch(NoteCenter.Y), positionNote);
                    sessionVM.Session.StaveBottom.AddNote(noteVM.Note, positionNote);
                }

                NoteCenter.Y -= offset;

                NoteCenter.X = NoteCenter.X * width / 1920.0;
                NoteCenter.Y = NoteCenter.Y * height / 1080.0;

                #region STB
                Storyboard stb = new Storyboard();
                PointAnimation moveCenter = new PointAnimation();

                moveCenter.From = Note.ActualCenter;
                moveCenter.To = NoteCenter;
                moveCenter.Duration = new Duration(TimeSpan.FromSeconds(0.15));

                moveCenter.FillBehavior = FillBehavior.Stop;

                stb.Children.Add(moveCenter);

                Storyboard.SetTarget(moveCenter, Note);
                Storyboard.SetTargetProperty(moveCenter, new PropertyPath(ScatterViewItem.CenterProperty));
                #endregion

                Note.Center = NoteCenter;
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
            sessionVM.Notes.Items.Remove(noteVM.SVItem);
            noteVM = new NoteViewModel(noteVM.SVItem.ActualCenter, noteVM.Note, sessionVM.Notes, sessionVM);
            sessionVM.Notes.Items.Add(noteVM.SVItem);
            sessionVM.NotesOnStave.Add(noteVM);

            double betweenStave = (350 - GlobalVariables.ManipulationGrid[noteVM.Note.Position + 2]) * (sessionVM.SessionSVI.ActualHeight / 1080);
            if (NoteCenter.Y < betweenStave)
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
