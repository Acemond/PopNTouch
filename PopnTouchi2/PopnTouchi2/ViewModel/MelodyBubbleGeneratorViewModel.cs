using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Infrastructure;
using System.Windows.Controls;
using System.Windows.Input;

namespace PopnTouchi2.ViewModel
{
    /// <summary>
    /// Binds MelodyBubbleGenerator's properties to the View.
    /// </summary>
    class MelodyBubbleGeneratorViewModel : ViewModelBase
    {
        /// <summary>
        /// Parameter.
        /// MelodyBubbleGenerator element from the Model.
        /// </summary>
        private MelodyBubbleGenerator melodyBubbleGenerator;

        /// <summary>
        /// Property.
        /// The Grid containing the melodybubblegenerator.
        /// </summary>
        private Grid Grid { get; set; }

        /// <summary>
        /// MelodyBubbleGeneratorViewModel Constructor
        /// </summary>
        /// <param name="nbg">The NoteBubbleGenerator item</param>
        public MelodyBubbleGeneratorViewModel(MelodyBubbleGenerator mbg)
        {
            melodyBubbleGenerator = mbg;
            //TODO : set relative to Grid size
            Grid.Width = 368;
            Grid.Height = 234;
            Grid.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            Grid.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;

            Grid.TouchDown += new EventHandler<System.Windows.Input.TouchEventArgs>(touchDown);
        }

        /// <summary>
        /// NoteBubbleGenerator Theme related constructor
        /// </summary>
        /// <param name="nbg">The NoteBubbleGenerator item</param>
        /// <param name="theme">The Theme item</param>
        public MelodyBubbleGeneratorViewModel(MelodyBubbleGenerator mbg, Theme theme)
            : this(mbg)
        {
            Grid.Background = theme.NoteGeneratorImage;
        }

        /// <summary>
        /// Event triggered when the NoteBubbleGenerator is touch.
        /// Generates a new NoteBubble according to the MostNeeded algorithm.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">TouchEventArgs</param>
        public void touchDown(object sender, TouchEventArgs e)
        {
            MelodyBubble newBubble = melodyBubbleGenerator.CreateMelodyBubble();

            ((Session)Grid.Parent).Bubbles.Items.Add(newBubble);
        }
    }
}
