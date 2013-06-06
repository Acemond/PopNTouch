using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Controls;
using Microsoft.Surface.Presentation.Controls;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Input;

namespace PopnTouchi2
{
    /// <summary>
    /// Represents a graphic item aware of how many NoteBubble have been created.
    /// </summary>
    public class NoteBubbleGenerator
    {
        /// <summary>
        /// Parameter.
        /// List of NoteBubbles created observable by the NoteBubbleViewModel Class.
        /// </summary>
        public ObservableCollection<NoteBubble> NoteBubbles;

        /// <summary>
        /// Parameter.
        /// Dictionary counting the number of each Bubble
        /// On the screen
        /// </summary>
        private Dictionary<NoteValue, int> WildBubbles;

        /// <summary>
        /// Parameter.
        /// Used to count the number of (#) bubble
        /// </summary>
        private int SharpBubblesCount;

        /// <summary>
        /// Parameter.
        /// Used to count the number of (b) bubble
        /// </summary>
        private int FlatBubblesCount;

        /// <summary>
        /// Parameter.
        /// Used to count the number of bubble
        /// </summary>
        private int NoteBubblesCount;

        /// <summary>
        /// NoteBubbleGenerator Constructor.
        /// Initializes a new Observable collection for its NoteBubbles.
        /// Defines the Generator Image and its position.
        /// </summary>
        public NoteBubbleGenerator()
        {
            NoteBubbles = new ObservableCollection<NoteBubble>();
            WildBubbles = new Dictionary<NoteValue, int>();
            WildBubbles.Add(NoteValue.quaver, 0);
            WildBubbles.Add(NoteValue.crotchet, 0);
            WildBubbles.Add(NoteValue.minim, 0);
        }

        /// <summary>
        /// Creates a new NoteBubble with a given theme.
        /// </summary>
        /// <returns>A newly created NoteBubble</returns>
        public NoteBubble CreateNoteBubble(List<NoteBubble> bubbles)
        {
            WildBubbles[NoteValue.quaver] = 0;
            WildBubbles[NoteValue.crotchet] = 0;
            WildBubbles[NoteValue.minim] = 0;
            SharpBubblesCount = 0;
            FlatBubblesCount = 0;
            NoteBubblesCount = 0;

            foreach (NoteBubble nb in bubbles)
            {
                if (nb.Note.Duration != NoteValue.alteration)
                {
                    WildBubbles[nb.Note.Duration]++;
                    NoteBubblesCount++;
                }
                else
                {
                    if (nb.Note.Sharp) SharpBubblesCount++;
                    else FlatBubblesCount++;
                }
            }

            NoteValue needed = MostNeeded();
            NoteBubble newBubble;
            if (needed == NoteValue.alteration)
                if(SharpBubblesCount == FlatBubblesCount)
                {
                    Random rand = new Random();
                    newBubble = (rand.Next(1) == 1)? new NoteBubble(false, true) : new NoteBubble(true, false);
                }
                else newBubble = (SharpBubblesCount > FlatBubblesCount) ? new NoteBubble(false, true) : new NoteBubble(true, false);
            else newBubble = new NoteBubble(needed);

            NoteBubbles.Add(newBubble);
            return newBubble;
        }

        /// <summary>
        /// Generates the most needed note according to a random algorithm.
        /// A NoteBubble will be needed if it doesn't appear anymore on the user interface.
        /// </summary>
        /// <returns>The NoteValue needed to create a new NoteBubble</returns>
        private NoteValue MostNeeded()
        {
            NoteValue mostNeededNote = NoteValue.crotchet;
            Random rand = new Random();

            if (rand.Next(4) == 0 && SharpBubblesCount + FlatBubblesCount < 4)
                return NoteValue.alteration;

            try
            {
                int min = WildBubbles.Values.Min();
                List<NoteValue> res = new List<NoteValue>(3);
                foreach (System.Collections.Generic.KeyValuePair<NoteValue, int> nv in WildBubbles)
                    if (nv.Value == min) res.Add(nv.Key);

                mostNeededNote = res[rand.Next(res.Count)];
                WildBubbles[mostNeededNote]++;
            }
            catch (Exception)
            {
                switch (rand.Next(2))
                {
                    case 0: mostNeededNote = NoteValue.minim; break;
                    case 1: mostNeededNote = NoteValue.quaver; break;
                    default: mostNeededNote = NoteValue.crotchet; break;
                }
            }
            return mostNeededNote;
        }

        /// <summary>
        /// Removes a NoteBubble from the environment.
        /// Updates the Generator's NoteBubble list.
        /// </summary>
        /// <param name="idNote">The unique ID of the NoteBubble to be removed</param>
        public void removeFromGenerator(int idNote)
        {
            for (int i = 0; i < NoteBubbles.Count; i++)
            {
                if (NoteBubbles[i].Id == idNote) NoteBubbles.RemoveAt(i);
            }
        }
    }
}
