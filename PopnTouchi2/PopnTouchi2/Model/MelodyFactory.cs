using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopnTouchi2
{
    public class MelodyFactory
    {
        public Dictionary<Gesture, Melody> Melodies { get; set; }

        public MelodyFactory()
        {
            Melodies = new Dictionary<Gesture, Melody>();
            //Grosse base de données sur les mélodies que nous donnera Cedrick Alexandre*
            //quaver = Croche
            //crotchet = Noire
            //minim = Blanche

            List<Note> l = new List<Note>();
            l.Add(new Note(6,NoteValue.crotchet, Pitch.F, 0));
            l.Add(new Note(6,NoteValue.quaver, Pitch.F, 1));
            l.Add(new Note(6, NoteValue.minim, Pitch.F, 3));

            Melodies.Add(Gesture.infinite, new Melody(l));
        }

        public Melody getMelody(Gesture gesture)
        {
            return Melodies[gesture];
        }

        public Melody getMelody()
        {
            //Faire un algo qui détermine quelle mélodie extraire du générateur
            return Melodies[Gesture.infinite];
        }
    }
}
