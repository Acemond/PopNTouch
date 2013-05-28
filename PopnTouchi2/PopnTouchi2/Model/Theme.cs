using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework.Audio;


namespace PopnTouchi2
{
    /// <summary>
    /// Abstract Theme Class provides necessary methods to define multiples and differents themes for the application.
    /// </summary>
    public abstract class Theme
    {
        /// <summary>
        /// Property.
        /// Array of two instruments for the upper Stave.
        /// </summary>
        public Instrument[] InstrumentsTop { get; set; }

        /// <summary>
        /// Property.
        /// Array of two instruments for the lower Stave.
        /// </summary>
        public Instrument[] InstrumentsBottom { get; set; }

        /// <summary>
        /// Property
        /// Theme's sound
        /// </summary>
        public Cue sound { get; set; }

        /// <summary>
        /// Theme Constructor.
        /// Initialize instruments for each stave and a new Dictionary mapping a NoteBubble to its image.
        /// </summary>
        public Theme()
        {
            InstrumentsTop = new Instrument[2];
            InstrumentsBottom = new Instrument[2];
        }

        /// <summary>
        /// refreshSound()
        /// Used to activated the sound 
        /// of the background after the 
        /// Stop button
        /// </summary>
        /// <param name="name"></param>
        public void refreshSound(String name)
        {
            try
            {
                sound = AudioController.INSTANCE.SoundBank.GetCue(name);
            }
            catch (Exception e)
            {
                Console.WriteLine("Limited instance of background sound");
            }
        }
        
    }
}
