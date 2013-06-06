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
        /// Parameter.
        /// Dictionary counting the number of each MelodyBubble
        /// On the screen
        /// </summary>
        private Dictionary<Gesture, int> WildBubbles;

        /// <summary>
        /// MelodyBubbleGenerator Constructor.
        /// Initializes the MelodyBubble's List and defines the MelodyBubbleGenerator's image.
        /// </summary>
        public MelodyBubbleGenerator()
        {
            MelodyBubbles = new List<MelodyBubble>();
            WildBubbles = new Dictionary<Gesture, int>();
            WildBubbles.Add(Gesture.infinite, 0);
            WildBubbles.Add(Gesture.s, 0);
            WildBubbles.Add(Gesture.t, 0);
            WildBubbles.Add(Gesture.wave, 0);
            WildBubbles.Add(Gesture.zigzag, 0);
        }

        /// <summary>
        /// Create a random MelodyBubble
        /// </summary>
        public MelodyBubble CreateMelodyBubble(List<MelodyBubble> bubbles)
        {
            WildBubbles[Gesture.infinite] = 0;
            WildBubbles[Gesture.s] = 0;
            WildBubbles[Gesture.t] = 0;
            WildBubbles[Gesture.wave] = 0;
            WildBubbles[Gesture.zigzag] = 0;

            foreach (MelodyBubble mb in bubbles)
                WildBubbles[mb.Melody.gesture]++;

            MelodyBubble newMelodyBubble = new MelodyBubble(MostNeeded());
            MelodyBubbles.Add(newMelodyBubble);
            return newMelodyBubble;
        }


        /// <summary>
        /// Generates the most needed note according to a random algorithm.
        /// A NoteBubble will be needed if it doesn't appear anymore on the user interface.
        /// </summary>
        /// <returns>The NoteValue needed to create a new NoteBubble</returns>
        private Gesture MostNeeded()
        {
            Gesture mostNeededMelody = Gesture.infinite;
            Random rand = new Random();

            try
            {
                int min = WildBubbles.Values.Min();
                List<Gesture> res = new List<Gesture>(3);
                foreach (System.Collections.Generic.KeyValuePair<Gesture, int> nv in WildBubbles)
                    if (nv.Value == min) res.Add(nv.Key);

                mostNeededMelody = res[rand.Next(res.Count)];
                WildBubbles[mostNeededMelody]++;
            }
            catch (Exception)
            {
                switch (rand.Next(4))
                {
                    case 0: mostNeededMelody = Gesture.s; break;
                    case 1: mostNeededMelody = Gesture.t; break;
                    case 2: mostNeededMelody = Gesture.wave; break;
                    case 3: mostNeededMelody = Gesture.zigzag; break;
                    default: mostNeededMelody = Gesture.infinite; break;
                }
            }
            return mostNeededMelody;
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
