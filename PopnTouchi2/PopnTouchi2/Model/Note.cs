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
    /// <summary>
    /// Sets and defines all attributes and methods needed to manage a music Note.
    /// </summary>
    public class Note
    {
        /// <summary>
        /// Property.
        /// Note Octave between 1 or 2
        /// </summary>
        public int Octave { get; set; }

        /// <summary>
        /// Property.
        /// Note Duration defined by the NoteValue Enumeration.
        /// </summary>
        public NoteValue Duration { get; set; }

        /// <summary>
        /// Property.
        /// Note Pitch defined by the Pitch Enumeration.
        /// </summary>
        public Pitch Pitch { get; set; }

        /// <summary>
        /// Property.
        /// Note Position on the stave
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Property.
        /// Flat Note attribute : Set to True if the Note is flatten.
        /// </summary>
        public bool Flat { get; set; }

        /// <summary>
        /// Property.
        /// Sharp Note attribute : Set to True if the Note is sharpen.
        /// </summary>
        public bool Sharp { get; set; }
        
        /// <summary>
        /// Note Constructor.
        /// Generates a new object of class Note with a given octave, duration and pitch.
        /// </summary>
        /// <param name="oct">Octave</param>
        /// <param name="d">NoteValue</param>
        /// <param name="p">Pitch</param>
        /// <param name="posit">Position</param>
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
        /// Generates a string describing the note.
        /// </summary>
        /// <returns>The string newly created</returns>
        public String GetCue()
        {
            String alteration = "";
            if (Sharp)  alteration = "d";
            if (Flat)   alteration = "b";
            return Pitch.ToString() + Octave.ToString() + alteration;
        }

        /// <summary>
        /// Getter of duration's enum number
        /// </summary>
        /// <returns>The Duration's Enumation Rank</returns>
        public int GetDuration()
        {
            return Duration.GetHashCode();
        }
    }
}
