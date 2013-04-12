using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//using Microsoft.Xna.Framework.Audio;


namespace PopnTouchi2
{
    /// <summary>
    /// Abstract Theme Class provides necessary methods to define multiples and differents themes for the application.
    /// </summary>
    public abstract class Theme
    {
        /// <summary>
        /// Property.
        /// Array of two instruments for the upper Stave.
        /// </summary>
        public Instrument[] InstrumentsTop { get; set; }
        /// <summary>
        /// Property.
        /// Array of two instruments for the lower Stave.
        /// </summary>
        public Instrument[] InstrumentsBottom { get; set; }
        /// <summary>
        /// Property.
        /// Theme defines Background.
        /// </summary>
        public ImageBrush BackgroundImage { get; set; }
        /// <summary>
        /// Property.
        /// Theme defines Elements' aspect.
        /// </summary>
        public ImageBrush NoteGeneratorImage { get; set; }
        /// <summary>
        /// Property.
        /// Theme defines Elements' aspect.
        /// </summary>
        protected Dictionary<NoteValue, BitmapImage> NoteBubbleImages { get; set; }
        /// <summary>
        /// Property.
        /// Theme defines Elements' aspect.
        /// </summary>
        public ImageBrush MelodyGeneratorImage { get; set; }

        /// <summary>
        /// Property
        /// Theme's sound
        /// </summary>
        //public Cue sound { get; set; } //TODO

        /// <summary>
        /// Theme Constructor.
        /// Initialize instruments for each stave and a new Dictionary mapping a NoteBubble to its image.
        /// </summary>
        public Theme()
        {
            InstrumentsTop = new Instrument[2];
            InstrumentsBottom = new Instrument[2];
            NoteBubbleImages = new Dictionary<NoteValue, BitmapImage>();
        }

        /// <summary>
        /// TODO
        /// </summary>
        public void GenerateObjects()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="noteValue"></param>
        /// <returns></returns>
        public virtual BitmapImage GetNoteBubbleImageSource(NoteValue noteValue)
        {
            throw new System.NotImplementedException();
        }
    }
}
