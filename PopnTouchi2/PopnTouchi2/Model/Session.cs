using System;
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
    /// <summary>
    /// Manages a game session environment contained in a Grid.
    /// </summary>
    public class Session : Grid
    {
        #region Properties
        /// <summary>
        /// Property.
        /// Session's MelodyBubbleGenerator instance.
        /// </summary>
        public MelodyBubbleGenerator MelodyBubbleGenerator { get; set; }
        /// <summary>
        /// Property.
        /// Session's NoteBubbleGenerator instance.
        /// </summary>
        public NoteBubbleGenerator NoteBubbleGenerator { get; set; }
        /// <summary>
        /// Property.
        /// Session's upper Stave instance.
        /// </summary>
        public Stave StaveTop { get; set; }
        /// <summary>
        /// Property.
        /// Session's lower Stave instance.
        /// </summary>
        public Stave StaveBottom { get; set; }
        /// <summary>
        /// Property.
        /// Session's Theme instance.
        /// </summary>
        public Theme Theme { get; set; }
        /// <summary>
        /// Property.
        /// Session's Bubbles' ScatterView instance.
        /// </summary>
        public ScatterView Bubbles { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Session Constructor.
        /// Initializes interface elements accordingly to a default Theme.
        /// </summary>
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
            StaveTop = new Stave(Theme.GetInstrumentsTop()[0]);
            StaveBottom = new Stave(Theme.GetInstrumentsBottom()[0]);
            Background = Theme.BackgroundImage;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Changes the global Bpm with a new value.
        /// </summary>
        /// <param name="newBpm">The new Bpm value</param>
        public void ChangeBpm(int newBpm)
        {
            GlobalVariables.bpm = newBpm;
        }

        /// <summary>
        /// TODO
        /// </summary>
        public void Reduce()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// TODO
        /// </summary>
        public void Enlarge()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
