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
        #region Properties
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
        /// Note Pitch defined by a String, for example : "la" , "do", "re"...
        /// </summary>
        public String Pitch { get; set; }

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
        #endregion

        /// <summary>
        /// Note Constructor.
        /// Generates a new object of class Note with a given octave, duration and pitch.
        /// </summary>
        /// <param name="oct">Octave</param>
        /// <param name="d">NoteValue</param>
        /// <param name="p">Pitch</param>
        /// <param name="posit">Position</param>
        public Note(int oct, NoteValue d, String pitch, int posit)
        {
            Octave = oct;
            Duration = d;
            Pitch = pitch;
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
            if (Sharp)
            {
                alteration = "_d";
            }

            if (Flat)
            {
                switch(Pitch)
                {
                    case "do" : Pitch = "si" ; break;
                    case "re" : Pitch = "do" ; alteration = "_d"; break;
                    case "mi" : Pitch = "re" ; alteration = "_d"; break;
                    case "fa" : Pitch = "mi" ; break;
                    case "sol": Pitch = "fa" ; alteration = "_d"; break;
                    case "la" : Pitch = "sol"; alteration = "_d"; break;
                    case "si" : Pitch = "la" ; alteration = "_d"; break;
                }
            }
            return Pitch + Octave.ToString() + alteration;
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
