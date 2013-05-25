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
        /// TODO
        /// </summary>
        public MelodyBubble CreateMelodyBubble()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="melody"></param>
        public void RemoveFromGenerator(MelodyBubble melody)
        {
            throw new System.NotImplementedException();
        }
    }
}
