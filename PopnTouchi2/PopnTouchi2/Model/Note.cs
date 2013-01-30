using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopnTouchi2
{
    public class Note
    {
        public int Octave { get; set; }
        public NoteValue Duration { get; set; }
        public Pitch Pitch { get; set; }

        // Attributs pour dièse et bémol
        public bool Flat { get; set; }
        public bool Sharp { get; set; }
        
        /// <summary>
        /// Generates a new object of class Notes with a given octave, duration and pitch.
        /// </summary>
        /// <param name="oct">Octave</param>
        /// <param name="d">NoteValue</param>
        /// <param name="p">Pitch</param>
        public Note(int oct, NoteValue d, Pitch p)
        {
            Octave = oct;
            Duration = d;
            Pitch = p;
            Sharp = false;
            Flat = false;
        }

        /// <summary>
        /// Returns a string describing the note
        /// </summary>
        /// <returns></returns>
        public String getClue()
        {
            String alteration = "";
            if (Sharp)
                alteration = "d";
            if (Flat)
                alteration = "b";
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
