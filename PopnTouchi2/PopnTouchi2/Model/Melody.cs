using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopnTouchi2
{
    public class Melody
    {
        private List<Note> _notes;

        /// <summary>
        /// Generates a object of class Melody with a given List of notes.
        /// </summary>
        /// <param name="listnote"></param>
        public Melody(List<Note> listnote)
        {
            _notes = listnote;
        }
    }
}
