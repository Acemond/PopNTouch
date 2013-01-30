using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopnTouchi2
{
    public class NoteFactory
    {
        private Dictionary<int, NoteValue> _notes;

        public NoteFactory()
        {
            _notes = new Dictionary<int, NoteValue>();
        }

        public void createNote(NoteValue noteValue, Pitch pitch)
        {
            throw new System.NotImplementedException();
        }
    }
}
