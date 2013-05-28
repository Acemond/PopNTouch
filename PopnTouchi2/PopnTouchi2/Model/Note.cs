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
    [Serializable]
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
        /// <param name="pitch">String representing the Pitch</param>
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
        /// Note Constructor.
        /// Generates a new object of class Note with a given octave, duration, pitch,
        /// sharp and flat
        /// </summary>
        /// <param name="oct">Octave</param>
        /// <param name="d">NoteValue</param>
        /// <param name="pitch">String representing the Pitch</param>
        /// <param name="posit">Position</param>
        public Note(int oct, NoteValue d, String pitch, int posit, Boolean sharp, Boolean flat)
        {
            Octave = oct;
            Duration = d;
            Pitch = pitch;
            Sharp = sharp;
            Flat = flat;
            Position = posit;
        }

        /// <summary>
        /// Note Constructor.
        /// Generates a new object of class Note, default Note
        /// </summary>
        public Note()
        {
            Octave = 1;
            Duration = NoteValue.crotchet;
            Pitch = "do";
            Sharp = false;
            Flat = false;
            Position = -1;
        }

        /// <summary>
        /// Generates a string describing the note.
        /// </summary>
        /// <returns>The string newly created</returns>
        public String GetCue()
        {
            String PitchTmp = Pitch;
            String alteration = "";
            if (Sharp)
            {
                alteration = "_d";
            }

            if (Flat)
            {
                switch (PitchTmp)
                {
                    case "do": PitchTmp = "si"; break;
                    case "re": PitchTmp = "do"; alteration = "_d"; break;
                    case "mi": PitchTmp = "re"; alteration = "_d"; break;
                    case "fa": PitchTmp = "mi"; break;
                    case "sol": PitchTmp = "fa"; alteration = "_d"; break;
                    case "la": PitchTmp = "sol"; alteration = "_d"; break;
                    case "si": PitchTmp = "la"; alteration = "_d"; break;
                }
            }
            return PitchTmp + "_" + Octave.ToString() + alteration;
        }

        /// <summary>
        /// Getter of duration's enum number
        /// </summary>
        /// <returns>The Duration's Enumation Rank</returns>
        public int GetDuration()
        {
            return Duration.GetHashCode();
        }

        /// <summary>
        /// Down a semitone of the current Note
        /// </summary>
        public bool DownSemiTone()
        {
            if (Sharp)
            {
                Sharp = false;
                return false;
            }

            if (Flat)
            {
                Flat = false;
                switch (Pitch)
                {
                    case "do": Pitch = "la"; Sharp = true; break;
                    case "re": Pitch = "do"; break;
                    case "mi": Pitch = "re"; break;
                    case "fa": Pitch = "re"; Sharp = true; break;
                    case "sol": Pitch = "fa"; break;
                    case "la": Pitch = "sol"; break;
                    case "si": Pitch = "la"; break;
                }
            }

            switch (Pitch)
            {
                case "do": Pitch = "si"; break;
                case "re": Pitch = "do"; Sharp = true; break;
                case "mi": Pitch = "re"; Sharp = true; break;
                case "fa": Pitch = "mi"; break;
                case "sol": Pitch = "fa"; Sharp = true; break;
                case "la": Pitch = "sol"; Sharp = true; break;
                case "si": Pitch = "la"; Sharp = true; break;
            }

            return true;

            
        }

        /// <summary>
        /// Up a semitone of the current Note
        /// </summary>
        /// <returns>True if the note change its position</returns>
        public bool UpSemiTone()
        {
            if (Flat)
            {
                Flat = false;
                return false;
            }

            if (Sharp)
            {
                Sharp = false;
                switch (Pitch)
                {
                    case "do": Pitch = "re"; break;
                    case "re": Pitch = "mi"; break;
                    case "mi": Pitch = "fa"; Sharp = true; break;
                    case "fa": Pitch = "sol"; break;
                    case "sol": Pitch = "la"; break;
                    case "la": Pitch = "si"; break;
                    case "si": Pitch = "do"; Sharp = true; break;
                }
                return true;
            }

            switch (Pitch)
            {
                case "do": Pitch = "do"; Sharp = true; return false;
                case "re": Pitch = "re"; Sharp = true; return false;
                case "mi": Pitch = "fa"; return true;
                case "fa": Pitch = "fa"; Sharp = true; return false;
                case "sol": Pitch = "sol"; Sharp = true; return false;
                case "la": Pitch = "la"; Sharp = true; return false;
                case "si": Pitch = "do"; return true;
            }

            return true;
        }
    }
}
