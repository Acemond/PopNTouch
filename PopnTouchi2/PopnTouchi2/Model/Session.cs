using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using PopnTouchi2.Model.Enums;
using Microsoft.Xna.Framework.Audio;
using PopnTouchi2.Model;

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
        
        #endregion

        #region Constructors
        /// <summary>
        /// Session Constructor.
        /// Initializes interface elements accordingly to a default Theme.
        /// </summary>
        public Session()
        {
            Theme = new Theme1(); //Could be randomized
            ThemeID = 1;

            NoteBubbleGenerator = new NoteBubbleGenerator();
            MelodyBubbleGenerator = new MelodyBubbleGenerator();

            StaveTop = new Stave(Theme.InstrumentsTop[0], Theme);
            StaveBottom = new Stave(Theme.InstrumentsBottom[0], Theme);
            
            //sound methods
            try
            {
                Theme.sound.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Limited number of sound instance");
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Stops all the background environnement's sounds.
        /// </summary>
        public void StopBackgroundSound()
        {
            try
            {
                Theme.sound.Stop(AudioStopOptions.AsAuthored);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Limited number of sound instance");
            }
        }

        /// <summary>
        /// Start all the background environnement's sounds.
        /// </summary>
        public void PlayBackgroundSound()
        {
            try
            {
                Theme.refreshSound(Theme.sound.ToString());
                Theme.sound.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Limited number of sound instance");
            }
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
