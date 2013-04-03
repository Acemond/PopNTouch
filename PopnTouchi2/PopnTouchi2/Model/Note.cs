using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using PopnTouchi2.Model.Enums;
using Microsoft.Xna.Framework.Audio;
using System.Threading;

namespace PopnTouchi2
{
    public class Note
    {
        public int Octave { get; set; }
        public NoteValue Duration { get; set; }
        public Pitch Pitch { get; set; }
        public int Position { get; set; }

        // Attributs pour dièse et bémol
        public bool Flat { get; set; }
        public bool Sharp { get; set; }
        
        /// <summary>
        /// Generates a new object of class Notes with a given octave, duration and pitch.
        /// </summary>
        /// <param name="oct">Octave</param>
        /// <param name="d">NoteValue</param>
        /// <param name="p">Pitch</param>
        public Note(int oct, NoteValue d, Pitch p, int posit)
        {
            Octave = oct;
            Duration = d;
            Pitch = p;
            Sharp = false;
            Flat = false;
            Position = posit;
        }

        /// <summary>
        /// Returns a string describing the note
        /// </summary>
        /// <returns></returns>
        public String getCue()
        {
            String alteration = "";
            if (Sharp)  alteration = "d";
            if (Flat)   alteration = "b";
            return Pitch.ToString() + Octave.ToString() + alteration;
        }

        /// <summary>
        /// Getter of duration's enum number
        /// </summary>
        /// <returns></returns>
        public int getDuration()
        {
            return Duration.GetHashCode();
        }
    }
}
