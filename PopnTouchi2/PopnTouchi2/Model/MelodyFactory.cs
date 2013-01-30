using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopnTouchi2
{
    public class MelodyFactory
    {
        private Dictionary<Gesture, Melody> _melodies;
        private List<Note> list;

        public MelodyFactory()
        {
            _melodies = new Dictionary<Gesture, Melody>();
            Note n = new Note(1,NoteValue.crotchet,Pitch.A);
            Note n2 = new Note(1,NoteValue.crotchet,Pitch.B);
            list = new List<Note>() { n , n2 };
            _melodies.Add(Gesture.circle, new Melody(list));
        }

        public Melody getMelody(Gesture gesture)
        {
            return _melodies[gesture];
        }
    }
}
