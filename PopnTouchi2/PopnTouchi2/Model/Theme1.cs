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
            _instrumentsTop[0] = new Instrument(InstrumentType.ocarina);
            _instrumentsTop[1] = new Instrument(InstrumentType.piano);
            _instrumentsBottom[0] = new Instrument(InstrumentType.flute);
            _instrumentsBottom[1] = new Instrument(InstrumentType.ocarina);
            
            _backgroundImage = new ImageBrush();
            _backgroundImage.ImageSource =
                new BitmapImage(
                    new Uri(@"../../Resources/Images/Theme1/bullesBack.png", UriKind.Relative)
                );
            _noteGeneratorImage = new ImageBrush();
            _noteGeneratorImage.ImageSource =
                new BitmapImage(
                    new Uri(@"../../Resources/Images/Theme1/genenote.png", UriKind.Relative)
                );
            _melodyGeneratorImage = new ImageBrush();
            _melodyGeneratorImage.ImageSource =
                new BitmapImage(
                    new Uri(@"../../Resources/Images/Theme1/generythme.png", UriKind.Relative)
                );

            //noteBubbles dictionary creation
            ImageBrush crotchetImage = new ImageBrush();
            crotchetImage.ImageSource =
                new BitmapImage(
                    new Uri(@"../../Resources/Images/Theme1/crotchetBubble.png", UriKind.Relative)
                );

            ImageBrush minimImage = new ImageBrush();
            minimImage.ImageSource =
                new BitmapImage(
                    new Uri(@"../../Resources/Images/Theme1/minimBubble.png", UriKind.Relative)
                );

            ImageBrush quaverImage = new ImageBrush();
            quaverImage.ImageSource =
                new BitmapImage(
                    new Uri(@"../../Resources/Images/Theme1/quaverBubble.png", UriKind.Relative)
                );

            _noteBubbleImages.Add(NoteValue.crotchet, crotchetImage);
            _noteBubbleImages.Add(NoteValue.minim, minimImage);
            _noteBubbleImages.Add(NoteValue.quaver, quaverImage);
        }

        public override ImageBrush getNoteBubbleImage(NoteValue noteValue)
        {
            return _noteBubbleImages[noteValue];
        }

        public void generateObjects()
        {
            throw new System.NotImplementedException();
        }
    }
}
