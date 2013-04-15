using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using System.Timers;
using PopnTouchi2.Model.Enums;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Controls;

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
        /// Parameter
        /// Music Category of the waveBank
        /// </summary>
        public AudioCategory musicCategory;

        /// <summary>
        /// Parameter
        /// The background Sound's volume
        /// </summary>
        public float backgroundVolume;

        /// <summary>
        /// Parameter
        /// The timer used to fade in/out the background sound
        /// </summary>
        public Timer t;

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

            //get the background sound category to change the volume of these sounds
            musicCategory = audioEngine.GetCategory("Background sound");
            backgroundVolume = 1.0f;           
            t = new Timer();
        }

        /// <summary>
        /// Plays a sound passed in parameter.
        /// </summary>
        /// <param name="cue">The sound to play</param>
        public static void PlaySound(Cue cue)
        {
            cue.Play();
        }

        /// <summary>
        /// Play a sound without using a cue, but a String
        /// </summary>
        /// <param name="sound">the String of the sound to play</param>
        public static void PlaySoundWithString(String sound)
        {
            AudioController.INSTANCE.SoundBank.PlayCue(sound);
        }

        /// <summary>
        /// Immediatly stops the sound currently playing.
        /// </summary>
        /// <param name="cue">The sound to be stopped</param>
        public static void StopSound(Cue cue)
        {
            cue.Stop(AudioStopOptions.Immediate);
        }

        /// <summary>
        /// Change the volume of the sounds in the "Background sounds" category
        /// </summary>
        /// <param name="volume">the Volume to change</param>
        public static void UpdateVolume(float volume)
        {
            AudioController.INSTANCE.backgroundVolume = volume;
            AudioController.INSTANCE.musicCategory.SetVolume(volume);
        }

        /// <summary>
        /// Fade Out the background Sound
        /// </summary>
        public static void FadeOutBackgroundSound()
        {
            stopTimer();
            AudioController.INSTANCE.t.Interval = 300;
            AudioController.INSTANCE.t.Start();
            AudioController.INSTANCE.t.Elapsed += new ElapsedEventHandler(FadeOut_Action);
        }

        /// <summary>
        /// The action of fading out
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private static void FadeOut_Action(object source, ElapsedEventArgs e)
        {
            if (AudioController.INSTANCE.backgroundVolume > 0.05f)
            {
                AudioController.INSTANCE.backgroundVolume *= 0.60f;
                AudioController.UpdateVolume(AudioController.INSTANCE.backgroundVolume);
                Console.WriteLine(AudioController.INSTANCE.backgroundVolume.ToString());
            }
            else
            {
                stopTimer();
                AudioController.INSTANCE.backgroundVolume = 0;
                AudioController.UpdateVolume(AudioController.INSTANCE.backgroundVolume);
                Console.WriteLine(AudioController.INSTANCE.backgroundVolume.ToString());
            }
        }

        /// <summary>
        /// Fade in the background Sound
        /// </summary>
        public static void FadeInBackgroundSound()
        {
            stopTimer();
            AudioController.INSTANCE.t.Interval = 600;
            AudioController.INSTANCE.t.Start();
            AudioController.INSTANCE.t.Elapsed += new ElapsedEventHandler(FadeIn_Action);
        }

        /// <summary>
        /// The action of fading in
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private static void FadeIn_Action(object source, ElapsedEventArgs e)
        {
            if (AudioController.INSTANCE.backgroundVolume == 0)
            {
                AudioController.INSTANCE.backgroundVolume = 0.05f;
                AudioController.UpdateVolume(AudioController.INSTANCE.backgroundVolume);
                Console.WriteLine(AudioController.INSTANCE.backgroundVolume.ToString());
            }
            else if (AudioController.INSTANCE.backgroundVolume < GlobalVariables.maxVolume)
            {
                AudioController.INSTANCE.backgroundVolume *= 1.2f;
                AudioController.UpdateVolume(AudioController.INSTANCE.backgroundVolume);
                Console.WriteLine(AudioController.INSTANCE.backgroundVolume.ToString());
            }
            else stopTimer();
        }

        /// <summary>
        /// Stop the AudioController's Timer, used to avoid conflit between fade in/out
        /// </summary>
        private static void stopTimer()
        {
            AudioController.INSTANCE.t.Stop();
            AudioController.INSTANCE.t.EndInit();
            AudioController.INSTANCE.t.Elapsed -= new ElapsedEventHandler(FadeOut_Action);
            AudioController.INSTANCE.t.Elapsed -= new ElapsedEventHandler(FadeIn_Action);  
                 
        }
    }
}
