using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopnTouchi2
{
    public class Note
    {
        private int _octave;
        private NoteValue _duration;
        private Pitch _pitch;

        //Attributs pour dièse et bémol
        private bool _sharp;
        private bool _flat;
        
        /// <summary>
        /// Generates a new object of class Note with a given octave, duration and pitch.
        /// </summary>
        /// <param name="octave"></param>
        /// <param name="d"></param>
        /// <param name="p"></param>
        public Note(int octave, NoteValue d, Pitch p)
        {
            _octave = octave;
            _duration = d;
            _pitch = p;
            _sharp = false;
            _flat = false;
        }

        public String getClue()
        {
            String alteration = "";
            if (_sharp)
                alteration = "d";
            if (_flat)
                alteration = "b";
            return _pitch.ToString() + _octave.ToString() + alteration;
        }

        public int getDuration()
        {
            return _duration.GetHashCode();
        }
    }
}
