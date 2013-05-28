using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopnTouchi2
{
    /// <summary>
    /// Represents a set of notes.
    /// </summary>
    public class Melody
    {
        /// <summary>
        /// Property.
        /// List of notes defining the Melody.
        /// </summary>
        public List<Note> Notes { get; set; }

        /// <summary>
        /// Property
        /// Gesture links to the melody
        /// </summary>
        public Gesture gesture { get; set; }

        /// <summary>
        /// Melody Constructor.
        /// Generates an object of class Melody with a given list of notes.
        /// </summary>
        /// <param name="listnote">The notes list to copy</param>
        /// /// <param name="g">The Gesture</param>
        public Melody(List<Note> listnote, Gesture g)
        {
            Notes = listnote;
            gesture = g;
        }
    }
}
