using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PopnTouchi2
{
    /// <summary>
    /// Represents a graphic item aware of how many MelodyBubble have been created.
    /// </summary>
    public class MelodyBubbleGenerator
    {
        /// <summary>
        /// Property.
        /// List of existing MelodyBubble.
        /// </summary>
        public List<MelodyBubble> MelodyBubbles { get; set; }

        /// <summary>
        /// MelodyBubbleGenerator Constructor.
        /// Initializes the MelodyBubble's List and defines the MelodyBubbleGenerator's image.
        /// </summary>
        public MelodyBubbleGenerator()
        {
            MelodyBubbles = new List<MelodyBubble>();
        }

        /// <summary>
        /// Create a random MelodyBubble
        /// </summary>
        public MelodyBubble CreateMelodyBubble()
        {
            MelodyBubble mb = new MelodyBubble();
            MelodyBubbles.Add(mb);
            return mb;
        }

        /// <summary>
        /// Remove a melody from the Generator
        /// </summary>
        /// <param name="melody">The melody To remove</param>
        public void RemoveFromGenerator(MelodyBubble melody)
        {
            if (MelodyBubbles.Contains(melody))
            {
                MelodyBubbles.Remove(melody);
            }
        }
    }
}
