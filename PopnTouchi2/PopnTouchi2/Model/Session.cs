﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Model.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Surface.Presentation.Controls;

namespace PopnTouchi2
{
    public class Session : Grid
    {
        #region Attributes
        public MelodyBubbleGenerator MelodyBubbleGenerator { get; set; }
        public NoteBubbleGenerator NoteBubbleGenerator { get; set; }
        public Stave StaveTop { get; set; }
        public Stave StaveBottom { get; set; }
        public Theme Theme { get; set; }
        public ScatterView Bubbles { get; set; }
        #endregion

        #region Constructors
        public Session()
        {
            Theme = new Theme1(); //Could be randomized
            MelodyBubbleGenerator = new MelodyBubbleGenerator(Theme);
            NoteBubbleGenerator = new NoteBubbleGenerator(Theme);
            Bubbles = new ScatterView();
            Bubbles.Visibility = Visibility.Visible;
            this.Children.Add(Bubbles);

            Children.Add(NoteBubbleGenerator);
            Children.Add(MelodyBubbleGenerator);
            StaveTop = new Stave(Theme.getInstrumentsTop()[0]);
            StaveBottom = new Stave(Theme.getInstrumentsBottom()[0]);
            Background = Theme._backgroundImage;
        }
        #endregion

        #region Methods
        public void changeBpm(int newBpm)
        {
            GlobalVariables.bpm = newBpm;
        }

        public void reduce()
        {
            throw new System.NotImplementedException();
        }

        public void enlarge()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
