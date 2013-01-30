using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace PopnTouchi2
{
    public class AudioController
    {
        public static AudioController INSTANCE = new AudioController();
        private AudioEngine _audioEngine;
        private SoundBank _soundBank;
        private WaveBank _waveBank;

        private AudioController()
        {
            String path = System.Environment.CurrentDirectory;
            path = path.Replace(@"\bin\Debug", @"\Resources");

            _audioEngine = new AudioEngine(path + @"\sound.xgs");
            _waveBank = new WaveBank(_audioEngine, path + @"\Wave Bank.xwb");
            _soundBank = new SoundBank(_audioEngine, path + @"\Sound Bank.xsb");
        }

        public static void playSound(string sound)
        {
            AudioController.INSTANCE._soundBank.PlayCue(sound);
        }

        public static void stopSound(string sound)
        {
            Cue cue = AudioController.INSTANCE._soundBank.GetCue(sound);
            cue.Stop(AudioStopOptions.Immediate);
        }
    }
}
