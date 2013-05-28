using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopnTouchi2
{
    /// <summary>
    /// Theme4 inherits from Abstract Class Theme. Defines Application's Theme number 4.
    /// </summary>
    public class Theme4 : Theme
    {
        /// <summary>
        /// Theme3 Constructor.
        /// Calls Theme Constructor. Initialize instruments and interface's elements according to the theme desired.
        /// </summary>
        public Theme4() : base()
        {
            InstrumentsTop[0] = new Instrument(InstrumentType.flute);
            InstrumentsTop[1] = new Instrument(InstrumentType.piano);
            InstrumentsBottom[0] = new Instrument(InstrumentType.bass);
            InstrumentsBottom[1] = new Instrument(InstrumentType.vibraphone);

            // TODO sound = AudioController.INSTANCE.SoundBank.GetCue("loop_eveningWater");
        }

        /// <summary>
        /// refreshSound()
        /// Used to activated the sound 
        /// of the background after the 
        /// Stop button
        /// </summary>
        public override void refreshSound()
        {
            sound = AudioController.INSTANCE.SoundBank.GetCue("loop_eveningWater");
        }
    }
}
