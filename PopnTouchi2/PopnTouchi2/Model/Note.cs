﻿using System;
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
    }
}
