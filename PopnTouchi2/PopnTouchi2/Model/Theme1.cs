using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PopnTouchi2
{
    public class Theme1 : Theme
    {
        public Theme1()
            : base()
        {
            _instrumentsTop[0] = new Instrument(InstrumentType.piano);
            _instrumentsTop[1] = new Instrument(InstrumentType.ocarina);
            _instrumentsBottom[0] = new Instrument(InstrumentType.ocarina);
            _instrumentsBottom[1] = new Instrument(InstrumentType.flute);
            
            _backgroundImage = new ImageBrush();
            _backgroundImage.ImageSource =
                new BitmapImage(
                    new Uri(@"../../Image/bullesBack.png", UriKind.Relative)
                );
        }


        public void generateObjects()
        {
            throw new System.NotImplementedException();
        }
    }
}
