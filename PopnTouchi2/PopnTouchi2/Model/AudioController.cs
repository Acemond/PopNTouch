﻿using System;
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
        private WaveBank WaveBank;

        /// <summary>
        /// Parameter
        /// Music Category of the waveBank
        /// </summary>
        public AudioCategory BackgroundCategory;

        /// <summary>
        /// Parameter
        /// The background Sound's volume
        /// </summary>
        public float backgroundVolume;

        /// <summary>
        /// AudioController Constructor.
        /// Initializes AudioEngine, SoundBank and WaveBank.
        /// </summary>
        private AudioController()
        {
            String path = System.Environment.CurrentDirectory;
            path = path.Replace(@"\bin\Debug", @"\Resources");

            audioEngine = new AudioEngine(path + @"\sound.xgs");
            WaveBank = new WaveBank(audioEngine, path + @"\Wave Bank.xwb");
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
            cue.Stop(AudioStopOptions.AsAuthored);
        }

        /// <summary>
        /// Change the volume of the sounds in the "Background sounds" category
        /// </summary>
        /// <param name="volume">the Volume to change</param>
        public static void UpdateVolume(float volume)
        {
            AudioController.INSTANCE.backgroundVolume = volume;
            AudioController.INSTANCE.BackgroundCategory.SetVolume(volume);
        }
    }
}
