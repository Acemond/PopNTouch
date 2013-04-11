using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopnTouchi2
{
    /// <summary>
    /// Defines all default notes possible in the application.
    /// </summary>
    public class NoteFactory
    {
        /// <summary>
        /// Parameter.
        /// Dictionary mapping an Integer to a specific NoteValue
        /// </summary>
        private Dictionary<int, NoteValue> notes;

        /// <summary>
        /// NoteFactory Constructor.
        /// Initializes the Notes Dictionary.
        /// </summary>
        public NoteFactory()
        {
            notes = new Dictionary<int, NoteValue>();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="noteValue"></param>
        /// <param name="pitch"></param>
        public void CreateNote(int noteValue, String pitch)
        {
            throw new System.NotImplementedException();
        }
    }
}
