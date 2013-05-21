﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Infrastructure;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PopnTouchi2.ViewModel
{
    public class Theme1ViewModel : ViewModelBase
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

        public Theme1ViewModel(Theme t, SessionViewModel s) : base(s)
        {
            NoteBubbleImages = new Dictionary<NoteValue, BitmapImage>();
            theme = t;

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
            PlayImage = new ImageBrush();
            PlayImage.ImageSource =
                new BitmapImage(
                    new Uri(@"../../Resources/Images/Theme1/playdrop.png", UriKind.Relative)
                );

            BitmapImage crotchetImageSource = new BitmapImage(
                new Uri(@"../../Resources/Images/Theme1/Bubbles/Notes/bullenoire.png", UriKind.Relative)
            );

            BitmapImage minimImageSource = new BitmapImage(
                new Uri(@"../../Resources/Images/Theme1/Bubbles/Notes/bulleblanche.png", UriKind.Relative)
            );

            /*BitmapImage quaverImageSource = new BitmapImage(
                new Uri(@"../../Resources/Images/Theme1/Bubbles/Notes/bullecroche.png", UriKind.Relative)
            );*/

            BitmapImage quaverImageSource = GetBitmapImage("bullecroche");

            NoteBubbleImages.Add(NoteValue.crotchet, crotchetImageSource);
            NoteBubbleImages.Add(NoteValue.minim, minimImageSource);
            NoteBubbleImages.Add(NoteValue.quaver, quaverImageSource);
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
        public BitmapImage GetNoteBubbleImageSource(NoteValue noteValue)
        {
            return NoteBubbleImages[noteValue];
        }

        /// <summary>
        /// Find the NoteBubble's Image according to a NoteValue.
        /// </summary>
        /// <param name="noteValue">The Notevalue needed to find the Bubble Image</param>
        /// <returns>A BitmapImage linked to the Bubble</returns>
        public BitmapImage GetMelodyBubbleImageSource(Melody melody)
        {
            return MelodyBubbleImages[melody];
        }
    }
}
