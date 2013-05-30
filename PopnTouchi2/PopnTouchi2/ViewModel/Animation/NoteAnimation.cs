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
using System.Threading;
using PopnTouchi2.Infrastructure;
using System.Windows.Controls;

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

        /// <summary>
        /// Parameter
        /// </summary>
        private Point virtualCenter;

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
        /// <param name="nVM">Linked with the NoteViewModel</param>
        /// <param name="s">The current SessionViewModel</param>
        public NoteAnimation(NoteViewModel nVM, SessionViewModel s)
            : base()
        {
            sessionVM = s;
            noteVM = nVM;
            SVItem = nVM.SVItem;
            ParentSV = nVM.ParentSV;
            NothingAtThisPlace = true;

            SVItem.ContainerManipulationCompleted += new ContainerManipulationCompletedEventHandler(touchLeave);

            SVItem.PreviewTouchDown += new EventHandler<TouchEventArgs>(SVItem_PreviewTouchDown);
        }
        #endregion

        #region Events

        /// <summary>
        /// Event occured when a Note is released
        /// Magnetise the current note
        /// Add the note in the right stave
        /// Move the note to its final place
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void touchLeave(object sender, ContainerManipulationCompletedEventArgs e)
        {
            if (noteVM.Picked) return;
            noteVM.Picked = true;

            ScatterViewItem Note = new ScatterViewItem();
            Note = e.Source as ScatterViewItem;
            NoteCenter = Note.ActualCenter;

            int width = (int)sessionVM.Grid.ActualWidth;
            int height = (int)sessionVM.Grid.ActualHeight;
            NoteCenter.X = NoteCenter.X * 1920.0 / width;
            NoteCenter.Y = NoteCenter.Y * 1080.0 / height;

            if (NoteCenter.X < 150.0) NoteCenter.X = 150.0;
            else if (NoteCenter.X >= 1830.0) NoteCenter.X = 1800.0;
            else NoteCenter.X = Math.Floor((NoteCenter.X + 30.0) / 60.0) * 60.0;

            //"Applatissement" de la portée (MAJ : Switch -> Tableau !)
            int offset = GlobalVariables.ManipulationGrid[(int)(NoteCenter.X / 60.0)];
            NoteCenter.Y += offset;

            int positionNote = (int)(NoteCenter.X - 120) / 60;
            Converter converter = new Converter();
           
            //Y dans le cadre portée ?
            //Si oui, animation
            //Sinon transform to bubble
            if (NoteCenter.Y < 576.0 && NoteCenter.Y > 165.0)
            {
                if (NoteCenter.Y < 370.0)
                {
                    if (NoteCenter.Y >= 344) NoteCenter.Y = 344;
                    NoteCenter.Y = Math.Floor((NoteCenter.Y + 6.0) / 20.0) * 20.0 + 4.0;
                    
                }
                else
                {
                    if (NoteCenter.Y <= 395) NoteCenter.Y = 395;
                    NoteCenter.Y = Math.Floor((NoteCenter.Y + 15.0) / 20.0) * 20.0 - 5.0;
                }
                virtualCenter = NoteCenter;

                NoteCenter.Y -= offset;

                NoteCenter.X = NoteCenter.X * width / 1920.0;
                NoteCenter.Y = NoteCenter.Y * height / 1080.0;

                noteVM.SVItem.Center = NoteCenter;
                
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

                moveCenter.Completed +=new EventHandler(moveCenter_Completed);

                Note.Center = NoteCenter;

                stb.Begin(SVItem);
            }
            else
            {
                ReturnOnBubbleFormat(noteVM.SVItem.Center);
                DisplayPreviewGrid(false);
            }
        }

        void moveCenter_Completed(object sender, EventArgs e)
        {
            DisplayPreviewGrid(false);

            MyPoint noteBubbleCenter = new MyPoint(NoteCenter);
            for (int i = 0; i < sessionVM.NotesOnStave.Count && NothingAtThisPlace; i++)
            {
                if (noteBubbleCenter.QuasiEquals(sessionVM.NotesOnStave[i].SVItem.Center) && !sessionVM.NotesOnStave[i].Picked)
                {
                    NothingAtThisPlace = false;
                    noteVM = sessionVM.NotesOnStave[i];
                }
                else NothingAtThisPlace = true;
            }

            if (NothingAtThisPlace)
            {
                Converter converter = new Converter();
                int positionNote = (int)((virtualCenter.X - 120) / 60);

                double betweenStave = (350 - GlobalVariables.ManipulationGrid.ElementAtOrDefault(noteVM.Note.Position + 2)) * (sessionVM.SessionSVI.ActualHeight / 1080);

                sessionVM.Session.StaveTop.RemoveNote(noteVM.Note);
                sessionVM.Session.StaveBottom.RemoveNote(noteVM.Note);

                bool isUp = (NoteCenter.Y < betweenStave);
                noteVM.Note = new Note(converter.getOctave(virtualCenter.Y), noteVM.Note.Duration, converter.getPitch(virtualCenter.Y), positionNote, noteVM.Note.Sharp, noteVM.Note.Flat);
                noteVM.SetStyle();

                if (isUp)
                {
                    sessionVM.Session.StaveTop.AddNote(noteVM.Note, positionNote);
                    sessionVM.Session.StaveTop.CurrentInstrument.PlayNote(noteVM.Note);
                }
                else
                {
                    sessionVM.Session.StaveBottom.AddNote(noteVM.Note, positionNote);
                    sessionVM.Session.StaveBottom.CurrentInstrument.PlayNote(noteVM.Note);
                }                
            }
            else ReturnOnBubbleFormat(noteVM.SVItem.Center);
            noteVM.Picked = false;
        }

        /// <summary>
        /// Transform a NoteViewModel on a Bubble
        /// </summary>
        /// <param name="center"></param>
        public void ReturnOnBubbleFormat(Point center)
        {
            sessionVM.NotesOnStave.Remove(noteVM);
            sessionVM.Notes.Items.Remove(noteVM.SVItem);

            /*noteVM.Note.Position = -1;
            noteVM.Note.Pitch = "la";*/

            if (noteVM.Note.Sharp)
            {
                NoteBubbleViewModel nbVMA = new NoteBubbleViewModel(center, new NoteBubble(new Note(noteVM.Note)), sessionVM.Bubbles, sessionVM);
                noteVM.Note.Sharp = false;
                if (sessionVM.NbgVM.NoteBubbleVMs.Count >= GlobalVariables.MaxNoteBubbles)
                {
                    NoteBubbleViewModel toRemove = sessionVM.NbgVM.NoteBubbleVMs.First();
                    sessionVM.NbgVM.NoteBubbleVMs.Remove(toRemove);
                    sessionVM.Bubbles.Items.Remove(toRemove.SVItem);
                    toRemove = null;
                }
                sessionVM.Bubbles.Items.Add(nbVMA.SVItem);
                sessionVM.NbgVM.NoteBubbleVMs.Add(nbVMA);
                nbVMA.Animation.MoveFromLocation();
            }
            else if (noteVM.Note.Flat)
            {
                NoteBubbleViewModel nbVMA = new NoteBubbleViewModel(center, new NoteBubble(new Note(noteVM.Note)), sessionVM.Bubbles, sessionVM);
                noteVM.Note.Flat = false;
                if (sessionVM.NbgVM.NoteBubbleVMs.Count >= GlobalVariables.MaxNoteBubbles)
                {
                    NoteBubbleViewModel toRemove = sessionVM.NbgVM.NoteBubbleVMs.First();
                    sessionVM.NbgVM.NoteBubbleVMs.Remove(toRemove);
                    sessionVM.Bubbles.Items.Remove(toRemove.SVItem);
                    toRemove = null;
                }
                sessionVM.Bubbles.Items.Add(nbVMA.SVItem);
                sessionVM.NbgVM.NoteBubbleVMs.Add(nbVMA);
                nbVMA.Animation.MoveFromLocation();
            }
            NoteBubbleViewModel nbVM = new NoteBubbleViewModel(center, new NoteBubble(noteVM.Note), sessionVM.Bubbles, sessionVM);
            if (sessionVM.NbgVM.NoteBubbleVMs.Count >= GlobalVariables.MaxNoteBubbles)
            {
                NoteBubbleViewModel toRemove = sessionVM.NbgVM.NoteBubbleVMs.First();
                sessionVM.NbgVM.NoteBubbleVMs.Remove(toRemove);
                sessionVM.Bubbles.Items.Remove(toRemove.SVItem);
                toRemove = null;
            }
            sessionVM.Bubbles.Items.Add(nbVM.SVItem);
            sessionVM.NbgVM.NoteBubbleVMs.Add(nbVM);

            String effect = "pop" + (new Random()).Next(1, 5).ToString();
            AudioController.PlaySoundWithString(effect);
        }

        /// <summary>
        /// When the user touchDown on a bubble, it makes a little sound effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SVItem_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            ////Si la bulle est sur la portée et qu'on la touche, elle s'enleve de la portee
            DisplayPreviewGrid(true);
        }

        private void DisplayPreviewGrid(bool appear)
        {
            try
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
            catch (Exception exc) { }
        }
        #endregion
    }
}