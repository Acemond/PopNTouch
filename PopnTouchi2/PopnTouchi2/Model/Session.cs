using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Model.Enums;
using Microsoft.Xna.Framework.Audio;

namespace PopnTouchi2
{
    /// <summary>
    /// Manages a game session environment contained in a Grid.
    /// </summary>
    public class Session
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
        /// The Theme number.
        /// </summary>
        public int ThemeID { get; set; }
        /// <summary>
        /// Property
        /// Session's Background sound
        /// </summary>
        public Cue BackgroundSound { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Session Constructor.
        /// Initializes interface elements accordingly to a default Theme.
        /// </summary>
        public Session()
        {
            Theme = new Theme1(); //Could be randomized

            NoteBubbleGenerator = new NoteBubbleGenerator();
            MelodyBubbleGenerator = new MelodyBubbleGenerator();

            StaveTop = new Stave(true, Theme.InstrumentsTop[0]);
            StaveBottom = new Stave(false, Theme.InstrumentsBottom[0]);
            
            //sound methods
            BackgroundSound = Theme.sound;
            BackgroundSound.Play();
            AudioController.FadeInBackgroundSound();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Stops all the background environnement's sounds.
        /// </summary>
        public void StopBackgroundSound()
        {
            AudioController.FadeOutBackgroundSound();
            BackgroundSound.Stop(AudioStopOptions.Immediate);
        }

        /// <summary>
        /// Changes the global Bpm with a new value.
        /// </summary>
        /// <param name="newBpm">The new Bpm value</param>
        public void ChangeBpm(int newBpm)
        {
            GlobalVariables.bpm = newBpm;
        }
        #endregion
    }
}
