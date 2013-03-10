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
            //Grosse base de données sur les mélodies que nous donnera Cedrick Alexandre
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
