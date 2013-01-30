using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopnTouchi2
{
    public class Instrument
    {
        private InstrumentType _name;
        private Dictionary<Pitch, String> _sounds; // à quoi ça sert?

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
            AudioController.playSound(_name + n.getClue());
        }

        /// <summary>
        /// Stops current sound from instrument.
        /// </summary>
        /// <param name="n"></param>
        public void stopNote(Note n)
        {
            AudioController.stopSound(_name + n.getClue());
        }
        #endregion
    }
}
