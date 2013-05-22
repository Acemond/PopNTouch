using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Infrastructure;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PopnTouchi2.ViewModel
{
    /// <summary>
    /// Graphic items descriptions link to Theme number 2.
    /// </summary>
    public class Theme2ViewModel : ViewModelBase
    {
        /// <summary>
        /// Parameter.
        /// </summary>
        private Theme theme;

        /// <summary>
        /// Property.
        /// Theme element from the Model.
        /// </summary>
        public Theme Theme
        {
            get
            {
                return theme;
            }
            set
            {
                theme = value;
                NotifyPropertyChanged("Theme");
            }
        }

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
        protected Dictionary<Melody, BitmapImage> MelodyBubbleImages { get; set; }

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

        /// <summary>
        /// Theme2ViewModel Constructor.
        /// </summary>
        /// <param name="t">The Theme</param>
        /// <param name="s">The current SessionViewModel performing</param>
        public Theme2ViewModel(Theme t, SessionViewModel s) 
            : base(s)
        {
            NoteBubbleImages = new Dictionary<NoteValue, BitmapImage>();
            MelodyBubbleImages = new Dictionary<Melody, BitmapImage>();
            Theme = t;

           //TODO Define Images
        }

        /// <summary>
        /// Retrieves the bubble bitmap image with the given name.
        /// </summary>
        /// <param name="img">Image name</param>
        /// <returns>BitmapImage corresponding</returns>
        public BitmapImage GetBitmapImage(String img)
        {
            Console.WriteLine(this.ToString());
            return new BitmapImage(new Uri(@"../../Resources/Images/Theme2/Bubbles/Notes/" + img + ".png", UriKind.Relative));
        }

        /// <summary>
        /// Find the NoteBubble's Image according to a NoteValue.
        /// </summary>
        /// <param name="noteValue">The Notevalue needed to find the Bubble Image</param>
        /// <returns>A BitmapImage linked to the Bubble</returns>
        public BitmapImage GetNoteBubbleImageSource(NoteValue noteValue)
        {
            return NoteBubbleImages[noteValue];
        }

        /// <summary>
        /// Find the MelodyBubble's Image according to a melody.
        /// </summary>
        /// <param name="melody">The Melody needed to find the Bubble Image</param>
        /// <returns>A BitmapImage linked to the Bubble</returns>
        public BitmapImage GetMelodyBubbleImageSource(Melody melody)
        {
            return MelodyBubbleImages[melody];
        }
    }
}
