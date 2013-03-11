using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopnTouchi2
{
    public class Theme2 : Theme
    {
        public Theme2()
            : base()
        {
            _instrumentsTop[0] = new Instrument(InstrumentType.ocarina);
            _instrumentsTop[1] = new Instrument(InstrumentType.piano);
            _instrumentsBottom[0] = new Instrument(InstrumentType.flute);
            _instrumentsBottom[1] = new Instrument(InstrumentType.ocarina);
        }


        public void generateObjects()
        {
            throw new System.NotImplementedException();
        }
    }
}
