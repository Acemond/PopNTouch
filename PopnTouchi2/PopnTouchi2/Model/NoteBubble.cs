using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Model.Enums;

namespace PopnTouchi2
{
    public class NoteBubble
    {

        public NoteBubble(int noteValue)
        {
            NoteValue n;
            switch (noteValue)
            {
                case 1: n = NoteValue.quaver;
                    break;
                case 2: n = NoteValue.crotchet;
                    break;
                case 3: n = NoteValue.minim;
                    break;
                default: n = NoteValue.crotchet;
                    break;
            }

            Note = new Note(0, n, Pitch.A, -1);
            Id = GlobalVariables.idNoteBubble++;
        }

        public Note Note
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }
    }
}
