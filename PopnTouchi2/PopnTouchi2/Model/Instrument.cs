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
    public class Instrument
    {
        private InstrumentType _name;
 //       private Dictionary<Pitch, String> _sounds; // à quoi ça sert?

        #region constructors
        /// <summary>
        /// Generates an object of class Instrument from a given name.
        /// </summary>
        /// <param name="instru"></param>
        public Instrument(InstrumentType instru)
        {
            _name = instru;
        }
        #endregion

        #region methods
        /// <summary>
        /// Plays the given note with the instrument
        /// </summary>
        /// <param name="n"></param>
        public void playNote(Note n)
        {
            Instrument tmp = new Instrument(_name);
            Thread t = new Thread(tmp.Action_Play);
            t.Start(n);
         }

 
        public void Action_Play(object n)
        {
            Note note = n as Note;
            TimeSpan t = new TimeSpan(0, 0, 0, 0, (note.Duration.GetHashCode() * 30000) / GlobalVariables.bpm);
            Cue cue = AudioController.INSTANCE._soundBank.GetCue(_name.ToString() + "_" + note.getCue());
            AudioController.playSound(cue);
            Thread.Sleep(t);
            AudioController.stopSound(cue);
        }
        #endregion
    }
}
