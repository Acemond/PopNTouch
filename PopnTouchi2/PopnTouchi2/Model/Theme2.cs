using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopnTouchi2
{
    /// <summary>
    /// Theme2 inherits from Abstract Class Theme. Defines Application's Theme number 2.
    /// </summary>
    public class Theme2 : Theme
    {
        /// <summary>
        /// Theme2 Constructor.
        /// Calls Theme Constructor. Initialize instruments and interface's elements according to the theme desired.
        /// </summary>
        public Theme2() : base()
        {
            InstrumentsTop[0] = new Instrument(InstrumentType.clarinette);
            InstrumentsTop[1] = new Instrument(InstrumentType.saxo);
            InstrumentsBottom[0] = new Instrument(InstrumentType.bass);
            InstrumentsBottom[1] = new Instrument(InstrumentType.vibraphone);

            // TODO sound = AudioController.INSTANCE.SoundBank.GetCue("loop_eveningWater");
        }

        public override void refreshSound()
        {
            sound = AudioController.INSTANCE.SoundBank.GetCue("loop_eveningWater");
        }
    }
}
