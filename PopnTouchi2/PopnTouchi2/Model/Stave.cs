using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Timers;
using PopnTouchi2.Model.Enums;
using PopnTouchi2.Model;
using System.Windows;
using PopnTouchi2.ViewModel;
using System.Windows.Threading;

namespace PopnTouchi2
{
    /// <summary>
    /// Defines and manages a Stave object.
    /// </summary>
    public class Stave
    {
        /// <summary>
        /// Property.
        /// A list of Notes observable by the StaveViewModel.
        /// </summary>
        public ObservableCollection<Note> Notes { get; set; }

        /// <summary>
        /// Property.
        /// The Current Instrument playing for the stave.
        /// </summary>
        public Instrument CurrentInstrument { get; set; }

        /// <summary>
        /// Property.
        /// Position of the last Note of the stave
        /// </summary>
        public int MaxPosition { get; set; }

        /// <summary>
        /// Property.
        /// Iterator of the Notes list
        /// </summary>
        public int IteratorNotes { get; set; }

        /// <summary>
        /// Parameter.
        /// A Timer object needed to play Notes' sounds.
        /// </summary>
        private DispatcherTimer Timer;

        /// <summary>
        /// Parameter.
        /// A Timer object needed to play the Melody sound.
        /// </summary>
        private DispatcherTimer TimerMelody;

        /// <summary>
        /// Parameter.
        /// Used to play a MelodyBubble
        /// </summary>
        public Melody Melody { get; set; }

        /// <summary>
        /// Property.
        /// Iterator of the Melody list
        /// </summary>
        public int IteratorMelody { get; set; }

        /// <summary>
        /// Property.
        /// The position of a note in the melody
        /// </summary>
        public int PositionMelody { get; set; }

        /// <summary>
        /// Property.
        /// The position of a note in the list Notes
        /// </summary>
        public int PositionNote { get; set; }

        /// <summary>
        /// Stave Constructor.
        /// Initializes a new empty list of Notes, a Timer, the MaxPosition to 0 and the instrument
        /// </summary>
        /// <param name="instru">The instrument to be used</param>
        /// <param name="theme">The theme</param>
        public Stave(Instrument instru)
        {
            MaxPosition = 0;
            Notes = new ObservableCollection<Note>();
            CurrentInstrument = instru;
            Timer = new DispatcherTimer();
            TimerMelody = new DispatcherTimer();
            IteratorNotes = 0;
            IteratorMelody = 0;
            PositionMelody = 0;
            PositionNote = 0;
        }

        /// <summary>
        /// Adds a group of Notes contained in a MelodyBubble on the stave at a defined position.
        /// </summary>
        /// <param name="mb">The MelodyBubble</param>
        /// <param name="position">The Position</param>
        public void AddMelody(MelodyBubble mb, int position)
        {
            int cardMelody = mb.Melody.Notes.Count;
            int i;
            for(i = 0; i< cardMelody ; i++)
            {
                mb.Melody.Notes[i].Position += position;
                if (mb.Melody.Notes[i].Position >= GlobalVariables.MaxPositionOnStave) break;
                int posArray = 0;
                while ((posArray < Notes.Count) && (Notes[posArray].Position < mb.Melody.Notes[i].Position))
                {
                    posArray++;
                }

                Notes.Insert(posArray, mb.Melody.Notes[i]);
       
            }

            MaxPosition = Math.Max(MaxPosition, mb.Melody.Notes[i-1].Position);
        }

        /// <summary>
        /// Adds a Note on the stave at a defined position.
        /// </summary>
        /// <param name="note">The note to be added</param>
        /// <param name="position">The Note position</param>
        public void AddNote(Note note, int position)
        {
            note.Position = position;
            if (!Notes.Contains(note))
            {
                int i = 0;
                for (i=0; (i < Notes.Count && Notes[i].Position < position); i++);

                Notes.Insert(i, note);
                MaxPosition = Math.Max(MaxPosition, note.Position);
            }
        }

        /// <summary>
        /// Remove the note from the list Notes
        /// </summary>
        /// <param name="note">The note to remove</param>
        public void RemoveNote(Note note) { Notes.Remove(note); }

        /// <summary>
        /// Plays the melody contained by the MelodyBubble.
        /// Calls the specific Event "PlayMelody"
        /// </summary>
        public int PlayMelody()
        {
            int time = 0;
            if (Notes.Count != 0)
                time = (Notes.Last().Position + 1) * (30000 / CurrentInstrument.Bpm);
            TimerMelody.Stop();
            TimerMelody.Interval = TimeSpan.FromMilliseconds(30000 / CurrentInstrument.Bpm);
            TimerMelody.Tick += new EventHandler(PlayMelody);
            TimerMelody.Start();
            return time;
        }

        /// <summary>
        /// Event triggered when the MelodyBubble is touched.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void PlayMelody(object source, EventArgs e)
        {
            bool play = true;

            if (PositionMelody <= Melody.Notes.Last().Position + 1)
            {
                while (play && (IteratorMelody < Melody.Notes.Count))
                {
                    if (Melody.Notes[IteratorMelody].Position == PositionMelody)
                    {
                        CurrentInstrument.PlayNote(Melody.Notes[IteratorMelody]);
                        IteratorMelody++;
                    }
                    else play = false;
                }

                PositionMelody++;

            }
            else
            {
                StopMelody();
            }
        }

        /// <summary>
        /// Plays all the notes contained by the stave.
        /// Calls the specific Event "PlayList"
        /// </summary>
        public int PlayAllNotes()
        {
            int time = 1000;
            if(Notes.Count != 0)
                time = (Notes.Last().Position+4)* (30000/CurrentInstrument.Bpm);
            Timer.Interval = TimeSpan.FromMilliseconds(30000 / CurrentInstrument.Bpm);
            Timer.Tick += new EventHandler(PlayList);
            Timer.Start();
            return time;
        }

        /// <summary>
        /// Event triggered when the play button is touched.
        /// Play all the notes on the current stave
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void PlayList(object source, EventArgs e)
        {
            bool play = true;

            if (PositionNote <= MaxPosition + 4)
            {
                while (play && (IteratorNotes < Notes.Count))
                {
                    if (Notes[IteratorNotes].Position == PositionNote)
                    {
                        CurrentInstrument.PlayNote(Notes[IteratorNotes]);
                        IteratorNotes++;
                    }
                    else play = false;

                }
                PositionNote++;
            }   
            else StopMusic();
        }

        /// <summary>
        /// Stops the music currently playing.
        /// </summary>
        public void StopMusic()
        {
            Timer.Stop();
            Timer.Tick -= new EventHandler(PlayList);
            PositionNote = 0;
            IteratorNotes = 0;
        }

        /// <summary>
        /// Stops the melody currently playing.
        /// </summary>
        public void StopMelody()
        {
            TimerMelody.Stop();
            TimerMelody.Tick -= new EventHandler(PlayMelody);
            PositionMelody = 0;
            IteratorMelody = 0;
        }
    }
}
