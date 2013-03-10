using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopnTouchi2
{
    public abstract class Theme
    {
        protected List<Instrument> _instrumentsTop;
        protected List<Instrument> _instrumentsBottom;

        public Theme()
        {
            _instrumentsTop = new List<Instrument>();
            _instrumentsBottom = new List<Instrument>();
        }

        public void generateObjects()
        {
            throw new System.NotImplementedException();
        }

        public List<Instrument> getInstrumentsTop()
        {
            return _instrumentsTop;
        }

        public List<Instrument> getInstrumentsBottom()
        {
            return _instrumentsBottom;
        }
    }
}
