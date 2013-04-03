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
        public SoundBank _soundBank;
        private WaveBank _waveBank;

        private AudioController()
        {
            String path = System.Environment.CurrentDirectory;
            path = path.Replace(@"\bin\Debug", @"\Resources");

            _audioEngine = new AudioEngine(path + @"\sound.xgs");
            _waveBank = new WaveBank(_audioEngine, path + @"\Wave Bank.xwb");
            _soundBank = new SoundBank(_audioEngine, path + @"\Sound Bank.xsb");
        }

        public static void playSound(Cue cue)
        {
            cue.Play();
        }

        public static void playSoundWithString(String son)
        {
            AudioController.INSTANCE._soundBank.PlayCue(son);
        }

        public static void stopSound(Cue cue)
        {
            cue.Stop(AudioStopOptions.Immediate);
        }
    }
}
