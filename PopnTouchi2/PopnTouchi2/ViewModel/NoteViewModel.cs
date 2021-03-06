﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Shapes;
using PopnTouchi2.Infrastructure;
using PopnTouchi2.ViewModel.Animation;
using PopnTouchi2.Model.Enums;

namespace PopnTouchi2.ViewModel
{
    /// <summary>
    /// Binds Note's properties to the View.
    /// </summary>
    public class NoteViewModel : ViewModelBase
    {
        /// <summary>
        /// Property.
        /// The Note in the NoteViewModel
        /// </summary>
        public Note Note { get; set; }

        /// <summary>
        /// Property.
        /// The parent ScatterView.
        /// </summary>
        public ScatterView ParentSV { get; set; }

        /// <summary>
        /// Property.
        /// The ScatterViewItem containing the note.
        /// </summary>
        public ScatterViewItem SVItem { get; set; }

        /// <summary>
        /// Parameter.
        /// The NoteAnimation item handling all animations for the note.
        /// </summary>
        public NoteAnimation Animation { get; set; }

        /// <summary>
        /// Defines if the note is being moved by user
        /// </summary>
        public bool Picked { get; set; }

        
        /// <summary>
        /// Constructor
        /// Create un NoteViewModel
        /// </summary>
        /// <param name="center">The center point of the ScatterViewItem </param>
        /// <param name="n">The note in the NoteViewModel</param>
        /// <param name="sv">The ScatterView Parent (here, SessionVM.Notes)</param>
        /// <param name="s">The current SessionViewModel</param>
        public NoteViewModel(Point center, Note n, ScatterView sv, SessionViewModel s)
            : base(s)
        {
            Note = n;

            SVItem = new ScatterViewItem();
            ParentSV = sv;

            SVItem.Center = center;

            SVItem.CanScale = false;
            SVItem.HorizontalAlignment = HorizontalAlignment.Center;
            SVItem.CanRotate = false;
            SVItem.HorizontalAlignment = HorizontalAlignment.Center;

            SetStyle();

            Animation = new NoteAnimation(this, SessionVM);
        }

        /// <summary>
        /// Constructor by copie.
        /// </summary>
        /// <param name="noteVM"></param>
        public NoteViewModel(NoteViewModel noteVM)
        {
            Note = new Note(noteVM.Note);
            ParentSV = noteVM.ParentSV;
            SVItem = noteVM.SVItem;
            Animation = noteVM.Animation;
            Picked = noteVM.Picked;
        }

        /// <summary>
        /// Set the Style of the bubbleImage
        /// </summary>
        public void SetStyle()
        {
            FrameworkElementFactory bubbleImage = new FrameworkElementFactory(typeof(Image));

            String noteValue = Note.Duration.ToString();
            int offset = GlobalVariables.ManipulationGrid.ElementAtOrDefault(Note.Position + 2);
            double betweenStave = (350 - offset) * (SessionVM.SessionSVI.ActualHeight / 1080);

            if (SVItem.Center.Y < betweenStave)
            {
                if (Note.Flat)
                    bubbleImage.SetValue(Image.SourceProperty, new BitmapImage(new Uri(@"../../Resources/Images/UI_items/Notes/black/" + noteValue + "_bemol.png", UriKind.Relative)));
                else if (Note.Sharp)
                    bubbleImage.SetValue(Image.SourceProperty, new BitmapImage(new Uri(@"../../Resources/Images/UI_items/Notes/black/" + noteValue + "_diese.png", UriKind.Relative)));
                else
                    bubbleImage.SetValue(Image.SourceProperty, new BitmapImage(new Uri(@"../../Resources/Images/UI_items/Notes/black/" + noteValue + ".png", UriKind.Relative)));
            }
            else
            {
                if (Note.Flat)
                    bubbleImage.SetValue(Image.SourceProperty, new BitmapImage(new Uri(@"../../Resources/Images/UI_items/Notes/white/" + noteValue + "_bemol.png", UriKind.Relative)));
                else if (Note.Sharp)
                    bubbleImage.SetValue(Image.SourceProperty, new BitmapImage(new Uri(@"../../Resources/Images/UI_items/Notes/white/" + noteValue + "_diese.png", UriKind.Relative)));
                else
                    bubbleImage.SetValue(Image.SourceProperty, new BitmapImage(new Uri(@"../../Resources/Images/UI_items/Notes/white/" + noteValue + ".png", UriKind.Relative)));
            }

            bubbleImage.SetValue(Image.IsHitTestVisibleProperty, false);

            bubbleImage.SetValue(Image.WidthProperty, (125.0 / 1920.0) * SessionVM.Grid.ActualWidth);
            bubbleImage.SetValue(Image.HeightProperty, (260.0 / 1080.0) * SessionVM.Grid.ActualHeight);

            FrameworkElementFactory touchZone = new FrameworkElementFactory(typeof(Ellipse));
            //touchZone.SetValue(Ellipse.OpacityProperty, 0.3);
            touchZone.SetValue(Ellipse.FillProperty, Brushes.Transparent);
            touchZone.SetValue(Ellipse.WidthProperty, (47.0 / 1920.0) * SessionVM.Grid.ActualWidth);
            touchZone.SetValue(Ellipse.HeightProperty, (40.0 / 1080.0) * SessionVM.Grid.ActualHeight);


            FrameworkElementFactory grid = new FrameworkElementFactory(typeof(Grid));
            grid.AppendChild(bubbleImage);
            grid.AppendChild(touchZone);

            ControlTemplate ct = new ControlTemplate(typeof(ScatterViewItem));
            ct.VisualTree = grid;

            Style bubbleStyle = new Style(typeof(ScatterViewItem));
            bubbleStyle.Setters.Add(new Setter(ScatterViewItem.TemplateProperty, ct));
            SVItem.Style = bubbleStyle;
        }
    }
}
