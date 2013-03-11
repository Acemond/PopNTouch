using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace PopnTouchi2
{
    public abstract class Theme
    {
        protected List<Instrument> _instrumentsTop;
        protected List<Instrument> _instrumentsBottom;
        public ImageBrush _backgroundImage { get; set; } //Le thème définira l'image de fond d'une session

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
