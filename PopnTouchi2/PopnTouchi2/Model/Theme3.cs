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
        /// TODO Define images. See Theme1 for instance.
        /// </summary>
        public Theme3()
            : base()
        {
            instrumentsTop[0] = new Instrument(InstrumentType.ocarina);
            instrumentsTop[1] = new Instrument(InstrumentType.piano);
            instrumentsBottom[0] = new Instrument(InstrumentType.flute);
            instrumentsBottom[1] = new Instrument(InstrumentType.ocarina);
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
