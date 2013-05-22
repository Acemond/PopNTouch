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
        /// Property.
        /// How many notes of each type are currently available on the screen.
        /// Used in creation algorithm.
        /// </summary>
        public Dictionary<NoteValue, int> WildBubbles { get; set; }

        /// <summary>
        /// NoteBubbleGenerator Constructor.
        /// Initializes a new Observable collection for its NoteBubbles.
        /// Defines the Generator Image and its position.
        /// </summary>
        public NoteBubbleGenerator()
        {
            NoteBubbles = new ObservableCollection<NoteBubble>();
            WildBubbles = new Dictionary<NoteValue, int>();
            WildBubbles.Add(NoteValue.crotchet, 0);
            WildBubbles.Add(NoteValue.minim, 0);
            WildBubbles.Add(NoteValue.quaver, 0);
        }

        /// <summary>
        /// Creates a new NoteBubble with a given theme.
        /// </summary>
        /// <returns>A newly created NoteBubble</returns>
        public NoteBubble CreateNoteBubble()
        {
            NoteBubble newBubble = new NoteBubble(MostNeeded());
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
