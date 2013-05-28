using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PopnTouchi2
{
    /// <summary>
    /// Theme1 inherits from Abstract Class Theme. Defines Application's Theme number 1.
    /// </summary>
    public class Theme1 : Theme
    {
        /// <summary>
        /// Theme1 Constructor.
        /// Calls Theme Constructor. Initialize instruments and interface's elements according to the theme desired.
        /// </summary>
        public Theme1() : base()
        {
            InstrumentsTop[0] = new Instrument(InstrumentType.piano);
            InstrumentsTop[1] = new Instrument(InstrumentType.flute);
            InstrumentsBottom[0] = new Instrument(InstrumentType.bass);
            InstrumentsBottom[1] = new Instrument(InstrumentType.contrebass);

            refreshSound("loop_eveningWater");
        }
    }
}
