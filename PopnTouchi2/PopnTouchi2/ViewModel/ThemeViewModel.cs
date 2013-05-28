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
    /// Graphic items descriptions link to Theme number 1.
    /// </summary>
    public class ThemeViewModel : ViewModelBase
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
        protected Dictionary<Gesture, BitmapImage> MelodyBubbleImages { get; set; }

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
        /// Property.
        /// Theme defines Themes button aspect.
        /// </summary>
        public ImageBrush ThemesImage { get; set; }

        /// <summary>
        /// Property.
        /// Theme defines SoundPointEnable button aspect.
        /// </summary>
        public ImageBrush SoundPointEnableImage { get; set; }

        /// <summary>
        /// Property.
        /// Theme defines SoundPointDisable button aspect.
        /// </summary>
        public ImageBrush SoundPointDisableImage { get; set; }

        /// <summary>
        /// Theme1ViewModel Constructor.
        /// </summary>
        /// <param name="t">The Theme</param>
        /// <param name="s">The current SessionViewModel performing</param>
        public ThemeViewModel(Theme t, SessionViewModel s) 
            : base(s)
        {
            NoteBubbleImages = new Dictionary<NoteValue, BitmapImage>();
            MelodyBubbleImages = new Dictionary<Gesture, BitmapImage>();
            Theme = t;

            BackgroundImage = new ImageBrush();
            BackgroundImage.ImageSource =
                new BitmapImage(
                    new Uri(@"../../Resources/Images/Theme" + SessionVM.Session.ThemeID +"/background.jpg", UriKind.Relative)
                );
            NoteGeneratorImage = new ImageBrush();
            NoteGeneratorImage.ImageSource =
                new BitmapImage(
                    new Uri(@"../../Resources/Images/Theme" + SessionVM.Session.ThemeID + "/notefactory.png", UriKind.Relative)
                );
            MelodyGeneratorImage = new ImageBrush();
            MelodyGeneratorImage.ImageSource =
                new BitmapImage(
                    new Uri(@"../../Resources/Images/Theme" + SessionVM.Session.ThemeID + "/melodyfactory.png", UriKind.Relative)
                );
            PlayImage = new ImageBrush();
            PlayImage.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/Theme" + SessionVM.Session.ThemeID + "/playdrop.png", UriKind.Relative));

            SoundPointEnableImage = new ImageBrush();
            SoundPointEnableImage.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/Theme" + SessionVM.Session.ThemeID + "/soundpointenable.png", UriKind.Relative));

            SoundPointDisableImage = new ImageBrush();
            SoundPointDisableImage.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/Theme" + SessionVM.Session.ThemeID + "/soundpointdisable.png", UriKind.Relative));
            
            ThemesImage = new ImageBrush();
            ThemesImage.ImageSource = new BitmapImage(new Uri(@"../../Resources/Images/UI_items/themes.png", UriKind.Relative));


            NoteBubbleImages.Add(NoteValue.crotchet, GetNoteBitmapImage("bullenoire"));
            NoteBubbleImages.Add(NoteValue.minim, GetNoteBitmapImage("bulleblanche"));
            NoteBubbleImages.Add(NoteValue.quaver, GetNoteBitmapImage("bullecroche"));


            MelodyBubbleImages.Add(Gesture.infinite , GetMelodyBitmapImage("infinite"));
            MelodyBubbleImages.Add(Gesture.s, GetMelodyBitmapImage("s"));
            MelodyBubbleImages.Add(Gesture.t, GetMelodyBitmapImage("t"));
            MelodyBubbleImages.Add(Gesture.wave, GetMelodyBitmapImage("wave"));
            MelodyBubbleImages.Add(Gesture.zigzag, GetMelodyBitmapImage("zigzag"));

        }

        /// <summary>
        /// Retrieves the bubble bitmap image with the given name.
        /// </summary>
        /// <param name="img">Image name</param>
        /// <returns>BitmapImage corresponding</returns>
        public BitmapImage GetNoteBitmapImage(String img)
        {
            return new BitmapImage(new Uri(@"../../Resources/Images/Theme" + SessionVM.Session.ThemeID +"/Bubbles/Notes/" + img + ".png", UriKind.Relative));
        }

        /// <summary>
        /// Retrieves the bubble bitmap image with the given name.
        /// </summary>
        /// <param name="img">Image name</param>
        /// <returns>BitmapImage corresponding</returns>
        public BitmapImage GetMelodyBitmapImage(String img)
        {
            return new BitmapImage(new Uri(@"../../Resources/Images/Theme" + SessionVM.Session.ThemeID + "/Bubbles/Melodies/" + img + ".png", UriKind.Relative));
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
        /// Find the NoteBubble's Image according to the alterations.
        /// </summary>
        /// <param name="sharp">sharp (#)</param>
        /// <returns>A BitmapImage linked to the Bubble</returns>
        public BitmapImage GetNoteBubbleImageSource(bool sharp)
        {
            if (sharp)
            {
                return GetNoteBitmapImage("diese");
            }
            else
            {
                return GetNoteBitmapImage("bemol");
            }
            
        }

        /// <summary>
        /// Find the MelodyBubble's Image according to a melody.
        /// </summary>
        /// <param name="melody">The Melody needed to find the Bubble Image</param>
        /// <returns>A BitmapImage linked to the Bubble</returns>
        public BitmapImage GetMelodyBubbleImageSource(Gesture gesture)
        {
            return MelodyBubbleImages[gesture];
        }
    }
}
