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
            _instrumentsTop.Add(new Instrument(InstrumentType.ocarina));
            _instrumentsTop.Add(new Instrument(InstrumentType.piano));
            _instrumentsBottom.Add(new Instrument(InstrumentType.flute));
            _instrumentsBottom.Add(new Instrument(InstrumentType.ocarina));
            
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
