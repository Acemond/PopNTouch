using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Model.Enums;

namespace PopnTouchi2
{
    /// <summary>
    /// Defines a Bubble containing a Melody item.
    /// </summary>
    public class MelodyBubble
    {
        /// <summary>
        /// Property.
        /// Melody contained by the Bubble
        /// </summary>
        public Melody Melody { get; set; }

        /// <summary>
        /// Property.
        /// The Bubble unique ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// MelodyBubble Constructor.
        /// Generates a new instance of MelodyBulle triggered by touching the generator.
        /// </summary>
        public MelodyBubble()
        {
            MelodyFactory factory = new MelodyFactory();
            Melody = factory.GetMelody();
            Id = GlobalVariables.idMelodyBubble++;
        }

        /// <summary>
        /// MelodyBubble Gesture Constructor.
        /// Generates a new instance of MelodyBulle triggered by doing a specific gesture.
        /// </summary>
        /// <param name="gesture">The gesture to create the melody</param>
        public MelodyBubble(Gesture gesture)
        {
            MelodyFactory factory = new MelodyFactory();
            Melody = factory.GetMelody(gesture);
            Id = GlobalVariables.idMelodyBubble++;
        }
    }
}
