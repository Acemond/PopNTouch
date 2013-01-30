using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopnTouchi2
{
    public class MelodyFactory
    {
        public Dictionary<Gesture, Melody> Melodies { get; set; }
        private List<Note> list;

        public MelodyFactory()
        {
            Melodies = new Dictionary<Gesture, Melody>();
            Note n = new Note(1,NoteValue.crotchet,Pitch.A);
            Note n2 = new Note(1,NoteValue.crotchet,Pitch.B);
            list = new List<Note>() { n , n2 };
            Melodies.Add(Gesture.circle, new Melody(new List<Note>() { n, n2 }));
        }

        public Melody getMelody(Gesture gesture)
        {
            return Melodies[gesture];
        }
    }
}
