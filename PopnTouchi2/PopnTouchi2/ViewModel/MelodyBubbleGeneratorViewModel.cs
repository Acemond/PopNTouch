using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Infrastructure;
using System.Windows.Controls;
using System.Windows.Input;
using PopnTouchi2.Model.Enums;

namespace PopnTouchi2.ViewModel
{
    /// <summary>
    /// Binds MelodyBubbleGenerator's properties to the View.
    /// </summary>
    public class MelodyBubbleGeneratorViewModel : ViewModelBase
    {
        /// <summary>
        /// Property.
        /// MelodyBubbleGenerator element from the Model.
        /// </summary>
        public MelodyBubbleGenerator MelodyBubbleGenerator { get; set; }

        /// <summary>
        /// Property.
        /// List Containing all MelodyBubbleViewModel created until now.
        /// </summary>
        public List<MelodyBubbleViewModel> MelodyBubbleVMs;

        /// <summary>
        /// Property.
        /// The Grid containing the melodybubblegenerator.
        /// </summary>
        public Grid Grid { get; set; }

        /// <summary>
        /// MelodyBubbleGenerator Theme related constructor
        /// </summary>
        /// <param name="mbg">The MelodyBubbleGenerator item</param>
        /// <param name="s">The current SessionViewModel</param>
        public MelodyBubbleGeneratorViewModel(MelodyBubbleGenerator mbg, SessionViewModel s) : base(s)
        {
            Grid = new Grid();
            MelodyBubbleVMs = new List<MelodyBubbleViewModel>();
            MelodyBubbleGenerator = mbg;
            //default, may change
            Grid.Width = 368;
            Grid.Height = 234;
            Grid.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            Grid.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;

            Grid.Background = SessionVM.ThemeVM.MelodyGeneratorImage;     

            Grid.TouchDown += new EventHandler<System.Windows.Input.TouchEventArgs>(touchDown);
        }

        /// <summary>
        /// Event triggered when the NoteBubbleGenerator is touch.
        /// Generates a new NoteBubble according to the MostNeeded algorithm.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">TouchEventArgs</param>
        public void touchDown(object sender, TouchEventArgs e)
        {
            MelodyBubble newBubble = MelodyBubbleGenerator.CreateMelodyBubble();
            MelodyBubbleViewModel mbVM = new MelodyBubbleViewModel(newBubble, SessionVM.Bubbles, SessionVM);
            if (GlobalVariables.MaxMelodyBubbles > MelodyBubbleVMs.Count)
            {
                MelodyBubbleVMs.Add(mbVM);
                SessionVM.Bubbles.Items.Add(mbVM.SVItem);
            }
            
            String effect = "pop" + (new Random()).Next(1, 5).ToString();
            AudioController.PlaySoundWithString(effect);
        }
    }
}
