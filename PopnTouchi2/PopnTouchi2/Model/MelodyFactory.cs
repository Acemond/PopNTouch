using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopnTouchi2
{
    /// <summary>
    /// Defines all default melodies contained in the application.
    /// </summary>
    public class MelodyFactory
    {
        /// <summary>
        /// Property.
        /// Map a Gesture to its specific melody.
        /// </summary>
        public Dictionary<Gesture, Melody> Melodies { get; set; }

        /// <summary>
        /// MelodyFactory Constructor.
        /// Defines and creates all known melodies.
        /// TODO Add DataBase from Cedrick Alexandre
        /// </summary>
        public MelodyFactory()
        {
            Melodies = new Dictionary<Gesture, Melody>();
            //quaver = Croche
            //crotchet = Noire
            //minim = Blanche

            List<Note> l = new List<Note>();
            l.Add(new Note(6,NoteValue.crotchet, "fa", 0));
            l.Add(new Note(6,NoteValue.quaver, "fa", 1));
            l.Add(new Note(6, NoteValue.minim, "la", 3));

            Melodies.Add(Gesture.infinite, new Melody(l));
        }

        /// <summary>
        /// Finds the melody associated to a gesture from Melodies Dictionary.
        /// </summary>
        /// <param name="gesture">The gesture</param>
        /// <returns>The melody found</returns>
        public Melody GetMelody(Gesture gesture)
        {
            return Melodies[gesture];
        }

        /// <summary>
        /// Randomly returns a melody known in the Dictionary.
        /// TODO Faire un algo qui détermine quelle mélodie extraire du générateur
        /// </summary>
        /// <returns>The random melody</returns>
        public Melody GetMelody()
        {
            return Melodies[Gesture.infinite];
        }
    }
}
