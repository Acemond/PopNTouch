using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Infrastructure;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PopnTouchi2.ViewModel
{
    class Theme4ViewModel : ViewModelBase
    {
        /// <summary>
        /// Parameter.
        /// Theme element from the Model.
        /// </summary>
        private Theme theme;

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
        /// Property.
        /// Theme defines Play button aspect.
        /// </summary>
        public ImageBrush PlayImage { get; set; }

        public Theme4ViewModel(Theme t)
        {
            NoteBubbleImages = new Dictionary<NoteValue, BitmapImage>();
            theme = t;

           //TODO Define Images
        }

        public BitmapImage GetBitmapImage(String img)
        {
            Console.WriteLine(this.ToString());
            return new BitmapImage(new Uri(@"../../Resources/Images/Theme1/Bubbles/Notes/" + img + ".png", UriKind.Relative));
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
    }
}
