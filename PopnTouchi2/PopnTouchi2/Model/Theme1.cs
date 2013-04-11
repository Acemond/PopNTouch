using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PopnTouchi2
{
    /// <summary>
    /// Theme1 inherits from Abstract Class Theme. Defines Application's Theme number 1.
    /// </summary>
    public class Theme1 : Theme
    {
        /// <summary>
        /// Theme1 Constructor.
        /// Calls Theme Constructor. Initialize instruments and interface's elements according to the theme desired.
        /// </summary>
        public Theme1()
            : base()
        {
            InstrumentsTop[0] = new Instrument(InstrumentType.piano);
            InstrumentsTop[1] = new Instrument(InstrumentType.flute);
            InstrumentsBottom[0] = new Instrument(InstrumentType.contrebass);
            InstrumentsBottom[1] = new Instrument(InstrumentType.bass);
            
            BackgroundImage = new ImageBrush();
            BackgroundImage.ImageSource =
                new BitmapImage(
                    new Uri(@"../../Resources/Images/Theme1/background.png", UriKind.Relative)
                );
            NoteGeneratorImage = new ImageBrush();
            NoteGeneratorImage.ImageSource =
                new BitmapImage(
                    new Uri(@"../../Resources/Images/Theme1/notefactory.png", UriKind.Relative)
                );
            MelodyGeneratorImage = new ImageBrush();
            MelodyGeneratorImage.ImageSource =
                new BitmapImage(
                    new Uri(@"../../Resources/Images/Theme1/melodyfactory.png", UriKind.Relative)
                );

            BitmapImage crotchetImageSource = new BitmapImage(
                new Uri(@"../../Resources/Images/Theme1/Bubbles/Notes/bullenoire.png", UriKind.Relative)
            );

            BitmapImage minimImageSource = new BitmapImage(
                new Uri(@"../../Resources/Images/Theme1/Bubbles/Notes/bulleblanche.png", UriKind.Relative)
            );

            BitmapImage quaverImageSource = new BitmapImage(
                new Uri(@"../../Resources/Images/Theme1/Bubbles/Notes/bullecroche.png", UriKind.Relative)
            );

            NoteBubbleImages.Add(NoteValue.crotchet, crotchetImageSource);
            NoteBubbleImages.Add(NoteValue.minim, minimImageSource);
            NoteBubbleImages.Add(NoteValue.quaver, quaverImageSource);

            sound = AudioController.INSTANCE.SoundBank.GetCue("loop_eveningWater");
        }

        /// <summary>
        /// Find the NoteBubble's Image according to a NoteValue.
        /// </summary>
        /// <param name="noteValue">The Notevalue needed to find the Bubble Image</param>
        /// <returns>A BitmapImage linked to the Bubble</returns>
        public override BitmapImage GetNoteBubbleImageSource(NoteValue noteValue)
        {
            return NoteBubbleImages[noteValue];
        }

        /// <summary>
        /// TODO
        /// </summary>
        public void GenerateObjects()
        {
            throw new System.NotImplementedException();
        }
    }
}
