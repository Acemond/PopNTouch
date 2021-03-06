﻿using System;
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
        /// </summary>
        public MelodyFactory()
        {
            Melodies = new Dictionary<Gesture, Melody>();
            //quaver = Croche
            //crotchet = Noire
            //minim = Blanche

            List<Note> l = new List<Note>();
            l.Add(new Note(2,NoteValue.minim, "do", 0));
            l.Add(new Note(1, NoteValue.minim, "sol", 4));
            l.Add(new Note(1, NoteValue.crotchet, "si", 8));
            l.Add(new Note(1, NoteValue.quaver, "sol", 10));
            l.Add(new Note(1, NoteValue.quaver, "la", 11));
            l.Add(new Note(1, NoteValue.crotchet, "si", 12));
            l.Add(new Note(2, NoteValue.minim, "do", 14));
            l.Add(new Note(1, NoteValue.minim, "do", 18));
            l.Add(new Note(1, NoteValue.minim, "la", 21));
            l.Add(new Note(1, NoteValue.minim, "sol", 24));
            Melodies.Add(Gesture.infinite, new Melody(l, Gesture.infinite));

            List<Note> l1 = new List<Note>();
            l1.Add(new Note(1,NoteValue.crotchet, "si", 0));
            l1.Add(new Note(2,NoteValue.quaver, "re", 2));
            l1.Add(new Note(1, NoteValue.minim, "la", 3));
            Melodies.Add(Gesture.wave, new Melody(l1, Gesture.wave));

            List<Note> l2 = new List<Note>();
            l2.Add(new Note(2,NoteValue.quaver, "do", 0));
            l2.Add(new Note(2, NoteValue.quaver, "mi", 1));
            l2.Add(new Note(2, NoteValue.crotchet, "fa", 2, true, false));
            Melodies.Add(Gesture.s, new Melody(l2, Gesture.s));

            List<Note> l3 = new List<Note>();
            l3.Add(new Note(1, NoteValue.quaver, "sol", 0));
            l3.Add(new Note(1, NoteValue.quaver, "sol", 1));
            l3.Add(new Note(1, NoteValue.quaver, "si", 3));
            Melodies.Add(Gesture.t, new Melody(l3, Gesture.t));

            List<Note> l4 = new List<Note>();
            l4.Add(new Note(1, NoteValue.quaver, "sol", 0));
            l4.Add(new Note(1, NoteValue.quaver, "mi", 1));
            l4.Add(new Note(1, NoteValue.quaver, "sol", 2));
            l4.Add(new Note(1, NoteValue.quaver, "mi", 3));
            l4.Add(new Note(1, NoteValue.quaver, "si", 4));
            l4.Add(new Note(1, NoteValue.quaver, "sol", 5));
            l4.Add(new Note(1, NoteValue.quaver, "si", 6));
            l4.Add(new Note(1, NoteValue.crotchet, "sol", 7));
            Melodies.Add(Gesture.zigzag, new Melody(l4, Gesture.zigzag));
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
        /// </summary>
        /// <returns>The random melody</returns>
        public Melody GetMelody()
        {
            Random r = new Random();
            int rand = r.Next(Melodies.Count);
            string[] names = Enum.GetNames(typeof(Gesture));
            var ret = Enum.Parse(typeof(Gesture), names[rand]);
            return Melodies[(Gesture)ret];
        }
    }
}
