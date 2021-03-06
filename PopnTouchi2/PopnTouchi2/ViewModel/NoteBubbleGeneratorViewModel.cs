﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Infrastructure;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using PopnTouchi2.Model.Enums;

namespace PopnTouchi2.ViewModel
{
    /// <summary>
    /// Binds NoteBubbleGenerator's properties to the View.
    /// </summary>
    public class NoteBubbleGeneratorViewModel : ViewModelBase
    {
        /// <summary>
        /// Property.
        /// NoteBubbleGenerator element from the Model.
        /// </summary>
        public NoteBubbleGenerator NoteBubbleGenerator { get; set; }
        
        /// <summary>
        /// Property.
        /// List Containing all NoteBubbleViewModel created until now.
        /// </summary>
        public List<NoteBubbleViewModel> NoteBubbleVMs { get; set; }

        /// <summary>
        /// Property.
        /// The Grid containing the notebubblegenerator.
        /// </summary>
        public Grid Grid { get; set; }

        /// <summary>
        /// NoteBubbleGenerator Theme related constructor
        /// </summary>
        /// <param name="nbg">The NoteBubbleGenerator item</param>
        /// <param name="s">The current SessionVM</param>
        public NoteBubbleGeneratorViewModel(NoteBubbleGenerator nbg, SessionViewModel s)
            : base(s)
        {
            Grid = new Grid();
            NoteBubbleVMs = new List<NoteBubbleViewModel>();
            NoteBubbleGenerator = nbg;
            //default, may change
            Grid.Width = 368;
            Grid.Height = 234;
            Grid.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Grid.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
 
            Grid.Background = SessionVM.ThemeVM.NoteGeneratorImage;
              
            Grid.TouchDown += new EventHandler<TouchEventArgs>(touchDown);
        }

        /// <summary>
        /// Event triggered when the NoteBubbleGenerator is touch.
        /// Generates a new NoteBubble according to the MostNeeded algorithm.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">TouchEventArgs</param>
        public void touchDown(object sender, TouchEventArgs e)
        {
            if (NoteBubbleVMs.Count < GlobalVariables.MaxNoteBubbles)
            {
                List<NoteBubble> bubblesList = new List<NoteBubble>();
                foreach (NoteBubbleViewModel nbvm in NoteBubbleVMs)
                    bubblesList.Add(nbvm.NoteBubble);
                NoteBubble newBubble = NoteBubbleGenerator.CreateNoteBubble(bubblesList);
                NoteBubbleViewModel nbVM = new NoteBubbleViewModel(newBubble, SessionVM.Bubbles, SessionVM);
                NoteBubbleVMs.Add(nbVM);
                SessionVM.Bubbles.Items.Add(nbVM.SVItem);
            }
            else
            {
                NoteBubbleViewModel toRemove = NoteBubbleVMs.First();
                NoteBubbleVMs.Remove(toRemove);
                SessionVM.Bubbles.Items.Remove(toRemove.SVItem);
                List<NoteBubble> bubblesList = new List<NoteBubble>();
                foreach (NoteBubbleViewModel nbvm in NoteBubbleVMs)
                    bubblesList.Add(nbvm.NoteBubble);
                NoteBubble newBubble = NoteBubbleGenerator.CreateNoteBubble(bubblesList);
                NoteBubbleViewModel nbVM = new NoteBubbleViewModel(newBubble, SessionVM.Bubbles, SessionVM);
                NoteBubbleVMs.Add(nbVM);
                SessionVM.Bubbles.Items.Add(nbVM.SVItem);
            }

            String effect = "pop" + (new Random()).Next(1, 5).ToString();
            AudioController.PlaySoundWithString(effect);
        }
    }
}
