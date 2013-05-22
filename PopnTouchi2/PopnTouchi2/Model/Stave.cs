using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Timers;
using PopnTouchi2.Model.Enums;

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
        /// TODO
        /// </summary>
        public int MaxPosition { get; set; }

        /// <summary>
        /// Parameter.
        /// True if this instance is for the upper stave.
        /// </summary>
        public Boolean isUp;

        /// <summary>
        /// Parameter.
        /// A Timer object needed to play Notes' sounds.
        /// </summary>
        private Timer Timer;

        /// <summary>
        /// Stave Constructor.
        /// Initializes a new empty list of Notes, a Timer, the MaxPosition to 0 and the instrument
        /// </summary>
        /// <param name="up">True if the current instance is the upper stave</param>
        /// <param name="instru">The instrument to be used</param>
        public Stave(Boolean up, Instrument instru)
        {
            MaxPosition = 0;
            Notes = new ObservableCollection<Note>();
            CurrentInstrument = instru;
            Timer = new Timer();
            if (up)
            {
                isUp = true;
            }
            else
            {
                isUp = false;
            }
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
                int posArray = 0;
                while ((posArray < Notes.Count) && (Notes[posArray].Position < mb.Melody.Notes[i].Position))
                    posArray++;

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
                for (i = 0; (i < Notes.Count && Notes[i].Position < position); i++);

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
        /// Plays all the notes contained by the stave.
        /// Calls the specific Event "PlayList"
        /// </summary>
        public void PlayAllNotes()
        {
            //Decrease the background sound
            AudioController.UpdateVolume(0f);

            Timer.Interval = 30000 / GlobalVariables.bpm;
            Timer.Start();
            Timer.Elapsed += new ElapsedEventHandler(PlayList);
        }

        /// <summary>
        /// Event triggered when the play button is touched.
        /// TODO Plus de détails
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void PlayList(object source, ElapsedEventArgs e)
        {
            bool play = true;
            if (GlobalVariables.position_Note <= MaxPosition + 4)
            {
                while (play && (GlobalVariables.it_Notes < Notes.Count))
                {
                    if (Notes[GlobalVariables.it_Notes].Position == GlobalVariables.position_Note)
                    {
                        CurrentInstrument.PlayNote(Notes[GlobalVariables.it_Notes]);
                        GlobalVariables.it_Notes++;
                    }
                    else play = false;
                }
               
                GlobalVariables.position_Note++;

            }
            else StopMusic();
        }

        /// <summary>
        /// Stops the music currently playing.
        /// </summary>
        public void StopMusic()
        {
            Timer.Stop();
            Timer.EndInit();
            Timer.Elapsed -= new ElapsedEventHandler(PlayList);
            GlobalVariables.position_Note = 0;
            GlobalVariables.it_Notes = 0;

            //begin the fade In of the background sound
            AudioController.FadeInBackgroundSound();
        }
    }
}
