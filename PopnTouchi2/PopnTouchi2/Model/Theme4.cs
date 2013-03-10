using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopnTouchi2
{
    public class Theme4 : Theme
    {
        public Theme4()
            : base()
        {
            _instrumentsTop.Add(new Instrument(InstrumentType.ocarina));
            _instrumentsTop.Add(new Instrument(InstrumentType.piano));
            _instrumentsBottom.Add(new Instrument(InstrumentType.flute));
            _instrumentsBottom.Add(new Instrument(InstrumentType.ocarina));
        }


        public void generateObjects()
        {
            throw new System.NotImplementedException();
        }
    }
}
