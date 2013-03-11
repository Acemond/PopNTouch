using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace PopnTouchi2
{
    public abstract class Theme
    {
        protected Instrument[] _instrumentsTop;
        protected Instrument[] _instrumentsBottom;
        public ImageBrush _backgroundImage { get; set; } //Le thème définira l'image de fond d'une session

        public Theme()
        {
            _instrumentsTop = new Instrument[2];
            _instrumentsBottom = new Instrument[2];
        }

        public void generateObjects()
        {
            throw new System.NotImplementedException();
        }

        public Instrument[] getInstrumentsTop()
        {
            return _instrumentsTop;
        }

        public Instrument[] getInstrumentsBottom()
        {
            return _instrumentsBottom;
        }
    }
}
