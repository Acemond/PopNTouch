using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Model.Enums;

namespace PopnTouchi2
{
    public class MelodyBubble
    {

        /// <summary>
        /// Construct when clicking on the generator
        /// </summary>
        public MelodyBubble()
        {
            MelodyFactory factory = new MelodyFactory();
            Melody = factory.getMelody();
            Id = GlobalVariables.idMelodyBubble++;
        }

        /// <summary>
        /// Construct when the gesture is made
        /// </summary>
        /// <param name="gesture">The gesture to create the melody</param>
        public MelodyBubble(Gesture gesture)
        {
            MelodyFactory factory = new MelodyFactory();
            Melody = factory.getMelody(gesture);
            Id = GlobalVariables.idMelodyBubble++;
        }

        public Melody Melody
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
