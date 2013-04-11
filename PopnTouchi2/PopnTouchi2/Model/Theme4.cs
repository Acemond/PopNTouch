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
        /// TODO Define images. See Theme1 for instance.
        /// </summary>
        public Theme4()
            : base()
        {
            InstrumentsTop[0] = new Instrument(InstrumentType.ocarina);
            InstrumentsTop[1] = new Instrument(InstrumentType.piano);
            InstrumentsBottom[0] = new Instrument(InstrumentType.flute);
            InstrumentsBottom[1] = new Instrument(InstrumentType.ocarina);
        }

        /// <summary>
        /// TODO
        /// </summary>
        public void GenerateObjects()
        {
            throw new System.NotImplementedException();
        }
    }
}
