﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Infrastructure;
using System.Windows.Controls;
using System.Windows.Input;

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
        /// The Grid containing the notebubblegenerator.
        /// </summary>
        public Grid Grid { get; set; }

        /// <summary>
        /// NoteBubbleGenerator Theme related constructor
        /// </summary>
        /// <param name="nbg">The NoteBubbleGenerator item</param>
        /// <param name="theme">The Theme item</param>
        public NoteBubbleGeneratorViewModel(NoteBubbleGenerator nbg, SessionViewModel s)
            : base(s)
        {
            NoteBubbleGenerator = nbg;
            //TODO : set relative to Grid size
            Grid.Width = 368;
            Grid.Height = 234;
            Grid.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Grid.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            switch (s.Session.ThemeID)
            {
                case 1:
                    Grid.Background = (new Theme1ViewModel(s.Session.Theme, s)).NoteGeneratorImage;
                    break;
                case 2:
                    Grid.Background = (new Theme2ViewModel(s.Session.Theme, s)).NoteGeneratorImage;
                    break;
                case 3:
                    Grid.Background = (new Theme3ViewModel(s.Session.Theme, s)).NoteGeneratorImage;
                    break;
                case 4:
                    Grid.Background = (new Theme4ViewModel(s.Session.Theme, s)).NoteGeneratorImage;
                    break;
            }

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
            NoteBubble newBubble = NoteBubbleGenerator.CreateNoteBubble();

            SessionVM.Bubbles.Items.Add(newBubble);
        }
    }
}
