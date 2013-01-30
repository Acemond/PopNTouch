using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopnTouchi2
{
    public class Melody
    {

        /// <summary>
        /// Generates a object of class Melody with a given List of notes.
        /// </summary>
        /// <param name="listnote"></param>
        public Melody(List<Note> listnote)
        {
            Notes = listnote;
        }

        public List<Note> Notes
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    }
}
