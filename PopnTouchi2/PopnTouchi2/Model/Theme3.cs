using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopnTouchi2
{
    /// <summary>
    /// Theme3 inherits from Abstract Class Theme. Defines Application's Theme number 3.
    /// </summary>
    public class Theme3 : Theme
    {
        /// <summary>
        /// Theme3 Constructor.
        /// Calls Theme Constructor. Initialize instruments and interface's elements according to the theme desired.
        /// </summary>
        public Theme3() : base()
        {
            InstrumentsTop[0] = new Instrument(InstrumentType.violon);
            InstrumentsTop[1] = new Instrument(InstrumentType.piano);
            InstrumentsBottom[0] = new Instrument(InstrumentType.contrebass);
            InstrumentsBottom[1] = new Instrument(InstrumentType.vibraphone);

            // TODO sound = AudioController.INSTANCE.SoundBank.GetCue("loop_eveningWater");
        }

        public override void refreshSound()
        {
            sound = AudioController.INSTANCE.SoundBank.GetCue("loop_eveningWater");
        }
    }
}
