using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace PopnTouchi2
{
    /// <summary>
    /// Manages all application sounds through Xna Framework audio with AudioEngine, SoundBank and WaveBank.
    /// </summary>
    public class AudioController
    {
        /// <summary>
        /// Creates the AudioController Singleton.
        /// </summary>
        public static AudioController INSTANCE = new AudioController();

        /// <summary>
        /// Parameter.
        /// AudioController's audioEngine
        /// </summary>
        private AudioEngine audioEngine;

        /// <summary>
        /// Property.
        /// AudioController's soundBank
        /// </summary>
        public SoundBank SoundBank { get; set; }

        /// <summary>
        /// Parameter.
        /// AudioController's waveBank
        /// </summary>
        private WaveBank waveBank;

        /// <summary>
        /// AudioController Constructor.
        /// Initializes AudioEngine, SoundBank and WaveBank.
        /// </summary>
        private AudioController()
        {
            String path = System.Environment.CurrentDirectory;
            path = path.Replace(@"\bin\Debug", @"\Resources");

            audioEngine = new AudioEngine(path + @"\sound.xgs");
            waveBank = new WaveBank(audioEngine, path + @"\Wave Bank.xwb");
            SoundBank = new SoundBank(audioEngine, path + @"\Sound Bank.xsb");
        }

        /// <summary>
        /// Plays a sound passed in parameter.
        /// </summary>
        /// <param name="cue">The sound to play</param>
        public static void PlaySound(Cue cue)
        {
            cue.Play();
        }

        public static void playSoundWithString(String son)
        {
            AudioController.INSTANCE.SoundBank.PlayCue(son);
        }

        /// <summary>
        /// Immediatly stops the sound currently playing.
        /// </summary>
        /// <param name="cue">The sound to be stopped</param>
        public static void StopSound(Cue cue)
        {
            cue.Stop(AudioStopOptions.Immediate);
        }
    }
}
