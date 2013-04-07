using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PopnTouchi2
{
    /// <summary>
    /// Abstract Theme Class provides necessary methods to define multiples and differents themes for the application.
    /// </summary>
    public abstract class Theme
    {
        /// <summary>
        /// Parameter.
        /// Array of two instruments for the upper Stave.
        /// </summary>
        protected Instrument[] instrumentsTop;
        /// <summary>
        /// Parameter.
        /// Array of two instruments for the lower Stave.
        /// </summary>
        protected Instrument[] instrumentsBottom;
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
        /// Theme Constructor.
        /// Initialize instruments for each stave and a new Dictionary mapping a NoteBubble to its image.
        /// </summary>
        public Theme()
        {
            instrumentsTop = new Instrument[2];
            instrumentsBottom = new Instrument[2];
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

        /// <summary>
        /// TODO Pourquoi faire un getter ? Alors qu'on pourrait faire une propriété?
        /// </summary>
        /// <returns></returns>
        public Instrument[] GetInstrumentsTop()
        {
            return instrumentsTop;
        }

        /// <summary>
        /// TODO Pourquoi faire un getter ? Alors qu'on pourrait faire une propriété?
        /// </summary>
        /// <returns></returns>
        public Instrument[] GetInstrumentsBottom()
        {
            return instrumentsBottom;
        }
    }
}
