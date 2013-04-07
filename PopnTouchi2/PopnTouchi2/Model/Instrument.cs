using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using System.Threading;
using System.Timers;
using PopnTouchi2.Model.Enums;

namespace PopnTouchi2
{
    /// <summary>
    /// Defines an instrument and methods to play sounds with it.
    /// </summary>
    public class Instrument
    {
        /// <summary>
        /// Property.
        /// Type of an instrument.
        /// </summary>
        public InstrumentType Name { get; set; }
        
        /// <summary>
        /// TODO
        /// </summary>
        //private Dictionary<Pitch, String> _sounds; // à quoi ça sert?

        #region constructors
        /// <summary>
        /// Instrument Constructor.
        /// Generates an object of class Instrument with a given type.
        /// </summary>
        /// <param name="instru">The type for the new instrument</param>
        public Instrument(InstrumentType instru)
        {
            Name = instru;
        }
        #endregion

        #region methods
        /// <summary>
        /// Plays the given note with the instrument.
        /// </summary>
        /// <param name="n">The note to be played</param>
        public void PlayNote(Note n)
        {
            Instrument tmp = new Instrument(Name);
            Thread t = new Thread(tmp.ActionPlay);
            t.Start(n);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="n"></param>
        public void ActionPlay(object n)
        {
            Note note = n as Note;
            TimeSpan t = new TimeSpan(0, 0, 0, 0, (note.Duration.GetHashCode() * 30000) / GlobalVariables.bpm);
            Cue cue = AudioController.INSTANCE.SoundBank.GetCue(Name.ToString() + "_" + note.GetCue());
            AudioController.PlaySound(cue);
            Thread.Sleep(t);
            AudioController.StopSound(cue);
        }
        #endregion
    }
}
