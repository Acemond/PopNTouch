using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopnTouchi2
{
    public class Theme3 : Theme
    {
        public Theme3()
            : base()
        {
            _instrumentsTop.Add(new Instrument(InstrumentType.ocarina));
            _instrumentsTop.Add(new Instrument(InstrumentType.piano));
            _instrumentsBottom.Add(new Instrument(InstrumentType.flute));
            _instrumentsBottom.Add(new Instrument(InstrumentType.ocarina));
            //TODO : define background image, see Theme1 for instance.
        }


        public void generateObjects()
        {
            throw new System.NotImplementedException();
        }
    }
}
