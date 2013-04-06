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
                    new Uri(@"../../Resources/Images/Theme1/background.png", UriKind.Relative)
                );
            _noteGeneratorImage = new ImageBrush();
            _noteGeneratorImage.ImageSource =
                new BitmapImage(
                    new Uri(@"../../Resources/Images/Theme1/notefactory.png", UriKind.Relative)
                );
            _melodyGeneratorImage = new ImageBrush();
            _melodyGeneratorImage.ImageSource =
                new BitmapImage(
                    new Uri(@"../../Resources/Images/Theme1/melodyfactory.png", UriKind.Relative)
                );

            //noteBubbles dictionary creation
            BitmapImage crotchetImageSource = new BitmapImage(
                new Uri(@"../../Resources/Images/Theme1/Bubbles/Notes/bullenoire.png", UriKind.Relative)
            );

            BitmapImage minimImageSource = new BitmapImage(
                new Uri(@"../../Resources/Images/Theme1/Bubbles/Notes/bulleblanche.png", UriKind.Relative)
            );

            BitmapImage quaverImageSource = new BitmapImage(
                new Uri(@"../../Resources/Images/Theme1/Bubbles/Notes/bullecroche.png", UriKind.Relative)
            );

            _noteBubbleImages.Add(NoteValue.crotchet, crotchetImageSource);
            _noteBubbleImages.Add(NoteValue.minim, minimImageSource);
            _noteBubbleImages.Add(NoteValue.quaver, quaverImageSource);
        }

        public override BitmapImage getNoteBubbleImageSource(NoteValue noteValue)
        {
            return _noteBubbleImages[noteValue];
        }

        public void generateObjects()
        {
            throw new System.NotImplementedException();
        }
    }
}
