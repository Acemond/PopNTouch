using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace PopnTouchi2
{
    /// <summary>
    /// Represents a graphic item aware of how many MelodyBubble have been created.
    /// </summary>
    public class MelodyBubbleGenerator : Grid
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

            //Defines size and position
            this.Width = 368;
            this.Height = 234;
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;

            this.TouchDown += new EventHandler<System.Windows.Input.TouchEventArgs>(MelodyBubbleGeneratorTouchDown);
        }

        /// <summary>
        /// MelodyBubbleGenerator Theme Constructor.
        /// Adds a specific Theme to the MelodyBubbleGenerator newly created.
        /// </summary>
        /// <param name="theme">The Theme to be used</param>
        public MelodyBubbleGenerator(Theme theme): this()
        {
            Background = theme.MelodyGeneratorImage;
        }

        /// <summary>
        /// TODO
        /// </summary>
        public void CreateMelodyBubble()
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

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MelodyBubbleGeneratorTouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
