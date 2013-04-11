﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Model.Enums;
using Microsoft.Surface.Presentation.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Input;

namespace PopnTouchi2
{
    /// <summary>
    /// Represents a graphic item containing an instance of Note.
    /// </summary>
    public class NoteBubble : ScatterViewItem
    {
        /// <summary>
        /// Property.
        /// Defines the vertical offset of the manipulation grid
        /// </summary>
        private int[] Offsettab { get; set; }

        /// <summary>
        /// Property.
        /// NoteBubble's contained Note
        /// </summary>
        public Note Note { get; set; }

        /// <summary>
        /// Property.
        /// NoteBubble unique ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// NoteBubble Constructor.
        /// Creates a new NoteBubble object and its Note.
        /// Defines a global offset array for the stave flattening.
        /// Defines the position of the newly created NoteBubble in its grid.
        /// </summary>
        /// <param name="noteValue">The NoteValue needed to create a Note</param>
        public NoteBubble(NoteValue noteValue)
        {
            Offsettab = new int[] { 0, 0, 0, 14, 25, 37, 47, 56, 64, 71, 76, 80, 83, 85, 85, 84, 80, 75, 68, 60, 50, 38, 26, 15, 4, -3, -9, -11, -12, -11, -7 };

            Note = new Note(0, noteValue, "la", -1);
            Id = GlobalVariables.idNoteBubble++;

            CanScale = false;
            HorizontalAlignment = HorizontalAlignment.Center;
            CanRotate = false;
            HorizontalAlignment = HorizontalAlignment.Center;

            ContainerManipulationCompleted += TouchLeaveBubble;
            //PreviewTouchDown += NoteBubble_TouchDown;

        }

        /// <summary>
        /// NoteBubble Theme Constructor.
        /// Specifies a Theme for the new NoteBubble object.
        /// Sets all its graphics attributes and view behaviours.
        /// </summary>
        /// <param name="noteValue">The NoteValue needed to create a Note</param>
        /// <param name="theme">The Theme needed to find the NoteBubble's image</param>
        public NoteBubble(NoteValue noteValue, Theme theme): this(noteValue)
        {
            FrameworkElementFactory bubbleImage = new FrameworkElementFactory(typeof(Image));
            bubbleImage.SetValue(Image.SourceProperty, theme.GetNoteBubbleImageSource(noteValue));
            bubbleImage.SetValue(Image.IsHitTestVisibleProperty, false);
            bubbleImage.SetValue(Image.WidthProperty, 85.0);
            bubbleImage.SetValue(Image.HeightProperty, 85.0);

            FrameworkElementFactory touchZone = new FrameworkElementFactory(typeof(Ellipse));
            touchZone.SetValue(Ellipse.FillProperty, Brushes.Transparent);
            touchZone.SetValue(Ellipse.MarginProperty, new Thickness(15));

            FrameworkElementFactory grid = new FrameworkElementFactory(typeof(Grid));
            grid.AppendChild(bubbleImage);
            grid.AppendChild(touchZone);

            ControlTemplate ct = new ControlTemplate(typeof(ScatterViewItem));
            ct.VisualTree = grid;

            Style bubbleStyle = new Style(typeof(ScatterViewItem));
            bubbleStyle.Setters.Add(new Setter(TemplateProperty, ct));
            this.Style = bubbleStyle;
        }

        /// <summary>
        /// TODO Description détaillée de ce que fait cette méthode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TouchLeaveBubble(object sender, ContainerManipulationCompletedEventArgs e)
        {
            ScatterViewItem bubble = new ScatterViewItem();
            bubble = e.Source as ScatterViewItem;
            Point bubbleCenter = bubble.ActualCenter;

            // int width = int.Parse(GetWidth.Text);
            // int height = int.Parse(GetHeight.Text);
            int width = (int)((Session)(((ScatterView)(this.Parent)).Parent)).ActualWidth;
            int height = (int)((Session)(((ScatterView)(this.Parent)).Parent)).ActualHeight;
            bubbleCenter.X = bubbleCenter.X * 1920 / width;
            bubbleCenter.Y = bubbleCenter.Y * 1080 / height;



            if (bubbleCenter.X <= 90) bubbleCenter.X = 120;
            else if (bubbleCenter.X >= 1830) bubbleCenter.X = 1800;
            else bubbleCenter.X = Math.Floor((bubbleCenter.X + 30) / 60) * 60;

            //"Applatissement" de la portée (MAJ : Switch -> Tableau !)
            int offset= Offsettab[((long)bubbleCenter.X/60)];
            bubbleCenter.Y += offset;

          
            //Y dans le cadre portée ?
            //Si oui, animation
            //pas de else
            if (bubbleCenter.Y < 630 && bubbleCenter.Y > 105)
            {
                if (bubbleCenter.Y < 370)
                {
                    if (bubbleCenter.Y >= 335) bubbleCenter.Y = 335;
                    bubbleCenter.Y = Math.Floor((bubbleCenter.Y - 20) / 25) * 25 + 35; //-20 et 35 pour 50

                }
                else
                {
                    if (bubbleCenter.Y <= 405) bubbleCenter.Y = 405;
                    bubbleCenter.Y = Math.Floor((bubbleCenter.Y + 10) / 25) * 25 + 5; //20 et 5 pour 50
                }

                bubbleCenter.Y -= offset;

                bubbleCenter.X = bubbleCenter.X * width / 1920;
                bubbleCenter.Y = bubbleCenter.Y * height / 1080;

                Storyboard stb = new Storyboard();
                PointAnimation moveCenter = new PointAnimation();

                moveCenter.From = bubble.ActualCenter;
                moveCenter.To = bubbleCenter;
                moveCenter.Duration = new Duration(TimeSpan.FromSeconds(0.15));
                bubble.Center = bubbleCenter;
                moveCenter.FillBehavior = FillBehavior.Stop;

                stb.Children.Add(moveCenter);

                Storyboard.SetTarget(moveCenter, bubble);
                Storyboard.SetTargetProperty(moveCenter, new PropertyPath(ScatterViewItem.CenterProperty));

                stb.Begin(this);

                bubble.Visibility = Visibility.Collapsed;
                bubble.Visibility = Visibility.Visible;
            }
        }

        //TODO
        /// <summary>
        /// When the user touchDown on a bubble, it makes a little sound effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /*private void NoteBubble_TouchDown(object sender, TouchEventArgs e)
        {
            String effect = "discovery";
            Random r = new Random();
            int nb = r.Next(1, 5);
            effect += nb.ToString();
            AudioController.PlaySoundWithString(effect);
        }*/
    }
}
