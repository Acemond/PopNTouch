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
        /// Generates a new object of class Note with a given octave, duration, pitch,
        /// sharp and flat
        /// </summary>
        /// <param name="oct">Octave</param>
        /// <param name="d">NoteValue</param>
        /// <param name="pitch">String representing the Pitch</param>
        /// <param name="posit">Position</param>
        public Note(Note note)
        {
            Octave = note.Octave;
            Duration = note.Duration;
            Pitch = note.Pitch;
            Sharp = note.Sharp;
            Flat = note.Flat;
            Position = note.Position;
        }

        /// <summary>
        /// Note Constructor.
        /// Generates a new object of class Note, default Note
        /// </summary>
        public Note()
        {
            Octave = 1;
            Duration = NoteValue.quaver;
            Pitch = "do";
            Sharp = false;
            Flat = false;
            Position = -1;
        }

        /// <summary>
        /// Note Constructor.
        /// Generates a new object of class Note, default Note
        /// </summary>
        public Note(NoteValue d)
        {
            Octave = 1;
            Duration = d;
            Pitch = "do";
            Sharp = false;
            Flat = false;
            Position = -1;
        }

        /// <summary>
        /// Return true if two notes are equals
        /// Don't mind about duration
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public bool  equals(Note n)
        {
            return ((Octave == n.Octave) && (Position == n.Position) && (Pitch == n.Pitch));
        }

        /// <summary>
        /// Generates a string describing the note.
        /// </summary>
        /// <returns>The string newly created</returns>
        public String GetCue()
        {
            String PitchTmp = Pitch;
            int OctaveTmp = Octave;
            String alteration = "";
            if (Sharp)
            {
                switch (PitchTmp)
                {
                    case "do": alteration = "_d"; break;
                    case "re": alteration = "_d"; break;
                    case "mi": PitchTmp = "fa"; break;
                    case "fa": alteration = "_d"; break;
                    case "sol": alteration = "_d"; break;
                    case "la": alteration = "_d"; break;
                    case "si": PitchTmp = "do"; OctaveTmp++; break;
                }
            }

            if (Flat)
            {
                switch (PitchTmp)
                {
                    case "do": PitchTmp = "si"; OctaveTmp--;  break;
                    case "re": PitchTmp = "do"; alteration = "_d"; break;
                    case "mi": PitchTmp = "re"; alteration = "_d"; break;
                    case "fa": PitchTmp = "mi"; break;
                    case "sol": PitchTmp = "fa"; alteration = "_d"; break;
                    case "la": PitchTmp = "sol"; alteration = "_d"; break;
                    case "si": PitchTmp = "la"; alteration = "_d"; break;
                }
            }
            return PitchTmp + "_" + OctaveTmp.ToString() + alteration;
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
            bool res = false;
            if (Sharp)
            {
                Sharp = false;
                return res;
            }

            else if (Flat)
            {
                Flat = false;
                switch (Pitch)
                {
                    case "do": Pitch = "si"; Octave--; Flat = true; break;
                    case "re": Pitch = "do"; break;
                    case "mi": Pitch = "re"; break;
                    case "fa": Pitch = "mi"; Flat = true; break;
                    case "sol": Pitch = "fa"; break;
                    case "la": Pitch = "sol"; break;
                    case "si": Pitch = "la"; break;
                }
                res = true;
                return res;
            }

            else
            {
                switch (Pitch)
                {
                    case "do": Pitch = "si"; res = true; break;
                    case "re": Pitch = "re"; Flat = true; res = false; break;
                    case "mi": Pitch = "mi"; Flat = true; res = false; break;
                    case "fa": Pitch = "mi"; res = true; break;
                    case "sol": Pitch = "sol"; Flat = true; res = false; break;
                    case "la": Pitch = "la"; Flat = true; res = false; break;
                    case "si": Pitch = "si"; Flat = true; res = false; break;
                }
                return res;
            }            
        }

        /// <summary>
        /// Up a semitone of the current Note
        /// </summary>
        /// <returns>True if the note change its position</returns>
        public bool UpSemiTone()
        {
            bool res = false;
            if (Flat)
            {
                Flat = false;
                return res;
            }

            else if (Sharp)
            {
                Sharp = false;
                switch (Pitch)
                {
                    case "do": Pitch = "re"; res = true; break;
                    case "re": Pitch = "mi"; res = true; break;
                    case "mi": Pitch = "fa"; Sharp = true; res = true; break;
                    case "fa": Pitch = "sol"; res = true; break;
                    case "sol": Pitch = "la"; res = true; break;
                    case "la": Pitch = "si"; res = true; break;
                    case "si": Pitch = "do"; Octave++; Sharp = true; res = true; break;
                }
                return res;
            }

            else
            {
                switch (Pitch)
                {
                    case "do": Pitch = "do"; Sharp = true; res = false; break;
                    case "re": Pitch = "re"; Sharp = true; res = false; break;
                    case "mi": Pitch = "fa"; res = true; break;
                    case "fa": Pitch = "fa"; Sharp = true; res = false; break;
                    case "sol": Pitch = "sol"; Sharp = true; res = false; break;
                    case "la": Pitch = "la"; Sharp = true; res = false; break;
                    case "si": Pitch = "do"; Octave++; res = true; break;
                }
                return res;
            }
        }
    }
}
