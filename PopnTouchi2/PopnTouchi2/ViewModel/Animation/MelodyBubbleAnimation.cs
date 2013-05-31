using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Input;
using PopnTouchi2.Model.Enums;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace PopnTouchi2.ViewModel.Animation
{
    /// <summary>
    /// Defines all animations linked to a Melody Bubble Item.
    /// </summary>
    public class MelodyBubbleAnimation : Animation
    {
        #region Properties
        /// <summary>
        /// Parameter.
        /// Private attribute of current sessionVM.
        /// </summary>
        private SessionViewModel sessionVM;

        /// <summary>
        /// Parameter.
        /// Private melody in the MelodyBubbleViewModel
        /// </summary>
        private MelodyBubbleViewModel melodyBubbleVM;

        /// <summary>
        /// Property
        /// True if the melody is played with the upper instrument
        /// </summary>
        public int playUp;

        /// <summary>
        /// Property.
        /// Center of the given bubble to animate
        /// </summary>
        private Point bubbleCenter;

        /// <summary>
        /// Property.
        /// The position of the melody
        /// </summary>
        public Point PositionMelody { get; set; }

        private DispatcherTimer topHighlightDt;
        private DispatcherTimer bottomHighlightDt;
        private DispatcherTimer previewDt;

        private ScatterView previewNotesGrid;

        private bool melodyDroppedTopStave;

        #endregion

         #region Constructors
        /// <summary>
        /// MelodyBubbleAnimation's Constructor.
        /// </summary>
        /// <param name="mbVM"></param>
        /// <param name="s"></param>
        public MelodyBubbleAnimation(MelodyBubbleViewModel mbVM, SessionViewModel s) 
            : base()
        {
            melodyBubbleVM = mbVM;
            sessionVM = s;
            SVItem = mbVM.SVItem;
            ParentSV = mbVM.ParentSV;
            canAnimate = true;
            playUp = 0;

            DispatcherTimer.Tick += new EventHandler(t_Tick);

            SVItem.ContainerManipulationCompleted += touchLeave;

            SVItem.PreviewTouchDown += touchDown;
            SVItem.PreviewTouchUp += new EventHandler<TouchEventArgs>(SVItem_PreviewTouchUp);
            Animate();
        }

        void SVItem_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            RemovePreview();
        }
        #endregion

        #region Methods
        private void DisplayPreview()
        {
            previewNotesGrid = new ScatterView();
            previewNotesGrid.Margin = new Thickness(0);

            previewDt = new DispatcherTimer();
            previewDt.Interval = TimeSpan.FromSeconds(.05);
            previewDt.Tick += new EventHandler(previewDt_Tick);
            previewDt.Start();
        }

        void previewDt_Tick(object sender, EventArgs e)
        {
            ScatterViewItem bubble = SVItem;
            Point virtualBubbleCenter = new Point(bubble.ActualCenter.X, bubble.ActualCenter.Y);

            int width = (int)sessionVM.Grid.ActualWidth;
            int height = (int)sessionVM.Grid.ActualHeight;
            virtualBubbleCenter.X = virtualBubbleCenter.X * 1920.0 / width;
            virtualBubbleCenter.Y = virtualBubbleCenter.Y * 1080.0 / height;

            if (virtualBubbleCenter.X < 150.0) virtualBubbleCenter.X = 150.0;
            else if (virtualBubbleCenter.X >= 1830) virtualBubbleCenter.X = 1800;
            else virtualBubbleCenter.X = Math.Floor((virtualBubbleCenter.X + 30) / 60) * 60;

            //"Applatissement" de la portée
            int offset = GlobalVariables.ManipulationGrid.ElementAtOrDefault((int)((double)virtualBubbleCenter.X / 60.0));
            virtualBubbleCenter.Y += offset;

            PositionMelody = new Point(virtualBubbleCenter.X, virtualBubbleCenter.Y);
            

            //Y dans le cadre portée ?
            //Si oui, animation
            //pas de else
            if (virtualBubbleCenter.Y < 576 && virtualBubbleCenter.Y > 165)
            {
                if (virtualBubbleCenter.Y < 370)
                {
                    if (virtualBubbleCenter.Y >= 344) virtualBubbleCenter.Y = 344;
                    virtualBubbleCenter.Y = Math.Floor((virtualBubbleCenter.Y + 6.0) / 20.0) * 20.0 + 4.0;
                }
                else
                {
                    if (virtualBubbleCenter.Y <= 395) virtualBubbleCenter.Y = 395;
                    virtualBubbleCenter.Y = Math.Floor((virtualBubbleCenter.Y + 15.0) / 20.0) * 20.0 - 5.0;
                }
            }

            previewNotesGrid.Items.Clear();

            List<NoteViewModel> ListOfNotes = melodyBubbleVM.melodyToListOfNote(PositionMelody);
            Converter cvrt = new Converter();

            double ratio = sessionVM.Grid.ActualWidth / 1920.0;

            for (int i = 0; i < ListOfNotes.Count; i++)
            {
                ScatterViewItem previewNote = new ScatterViewItem();
                FrameworkElementFactory notePreviewImage = new FrameworkElementFactory(typeof(Image));
                int offset2 = GlobalVariables.ManipulationGrid.ElementAtOrDefault(ListOfNotes[i].Note.Position + 2);
                double betweenStave = (350 - offset2) * ratio;
                bool up;

                previewNote.CanScale = false;
                previewNote.HorizontalAlignment = HorizontalAlignment.Center;
                previewNote.CanRotate = false;
                previewNote.HorizontalAlignment = HorizontalAlignment.Center;
                
                if (ListOfNotes[i].SVItem.Center.Y < betweenStave)
                {
                    up = true;
                    if (ListOfNotes[i].Note.Flat)
                        notePreviewImage.SetValue(Image.SourceProperty, new BitmapImage(new Uri(@"../../Resources/Images/UI_items/PreviewNotes/black/" + ListOfNotes[i].Note.Duration + "_bemol.png", UriKind.Relative)));
                    else if (ListOfNotes[i].Note.Sharp)
                        notePreviewImage.SetValue(Image.SourceProperty, new BitmapImage(new Uri(@"../../Resources/Images/UI_items/PreviewNotes/black/" + ListOfNotes[i].Note.Duration + "_diese.png", UriKind.Relative)));
                    else
                        notePreviewImage.SetValue(Image.SourceProperty, new BitmapImage(new Uri(@"../../Resources/Images/UI_items/PreviewNotes/black/" + ListOfNotes[i].Note.Duration + ".png", UriKind.Relative)));
                }
                else
                {
                    up = false;
                    if (ListOfNotes[i].Note.Flat)
                        notePreviewImage.SetValue(Image.SourceProperty, new BitmapImage(new Uri(@"../../Resources/Images/UI_items/PreviewNotes/white/" + ListOfNotes[i].Note.Duration + "_bemol.png", UriKind.Relative)));
                    else if (ListOfNotes[i].Note.Sharp)
                        notePreviewImage.SetValue(Image.SourceProperty, new BitmapImage(new Uri(@"../../Resources/Images/UI_items/PreviewNotes/white/" + ListOfNotes[i].Note.Duration + "_diese.png", UriKind.Relative)));
                    else
                        notePreviewImage.SetValue(Image.SourceProperty, new BitmapImage(new Uri(@"../../Resources/Images/UI_items/PreviewNotes/white/" + ListOfNotes[i].Note.Duration + ".png", UriKind.Relative)));
                }

                double Xpos = virtualBubbleCenter.X + melodyBubbleVM.MelodyBubble.Melody.Notes[i].Position * 60.0;
                double Ypos = cvrt.getCenterY(up, melodyBubbleVM.MelodyBubble.Melody.Notes[i]) + GlobalVariables.ManipulationGrid.ElementAtOrDefault(melodyBubbleVM.MelodyBubble.Melody.Notes[i].Position + 2) - offset;
                previewNote.Center = new Point(Xpos * ratio, Ypos * ratio);

                notePreviewImage.SetValue(Image.IsHitTestVisibleProperty, false);

                notePreviewImage.SetValue(Image.WidthProperty, (125.0 / 1920.0) * sessionVM.Grid.ActualWidth);
                notePreviewImage.SetValue(Image.HeightProperty, (260.0 / 1080.0) * sessionVM.Grid.ActualHeight);

                FrameworkElementFactory grid = new FrameworkElementFactory(typeof(Grid));
                grid.AppendChild(notePreviewImage);

                ControlTemplate ct = new ControlTemplate(typeof(ScatterViewItem));
                ct.VisualTree = grid;

                Style notePreviewStyle = new Style(typeof(ScatterViewItem));
                notePreviewStyle.Setters.Add(new Setter(ScatterViewItem.TemplateProperty, ct));
                previewNote.Style = notePreviewStyle;
                previewNote.Opacity = 0.6;

                previewNotesGrid.Items.Add(previewNote);
            }
            try { sessionVM.Grid.Children.Add(previewNotesGrid); }
            catch (Exception exc) { }
        }

        private void RemovePreview()
        {
            previewDt.Stop();
            sessionVM.Grid.Children.Remove(previewNotesGrid);
        }

        public void BeginBubbleAnimation()
        {
            Random r = GlobalVariables.GlobalRandom;
            DispatcherTimer.Interval = TimeSpan.FromMilliseconds(r.Next(100, 10000));
            DispatcherTimer.Start();
        }

        /// <summary>
        /// Note Bubble Animation 
        /// Moves the NoteBubble randomly on the screen.
        /// </summary>
        private void Animate()
        {
            if (canAnimate)
            {
                PointAnimation centerAnimation = new PointAnimation();
                SineEase ease = new SineEase();
                ease.EasingMode = EasingMode.EaseInOut;
                Random r = GlobalVariables.GlobalRandom;
                Double xOffset = (-2) * (r.Next() % 2 - .5) * r.Next(50, 100);
                Double yOffset = (-2) * (r.Next() % 2 - .5) * r.Next(50, 100);

                centerAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(r.Next(9000, 21000)));
                centerAnimation.AccelerationRatio = .3;
                centerAnimation.DecelerationRatio = .3;

                if (SVItem.Center.X + xOffset > ParentSV.ActualWidth)
                    xOffset = (ParentSV.ActualWidth - SVItem.Center.X) - (double)r.Next(50);
                if (SVItem.Center.X + xOffset < 0)
                    xOffset = 0 - SVItem.Center.X + (double)r.Next(50);
                if (SVItem.Center.Y + yOffset > ParentSV.ActualHeight)
                    yOffset = ParentSV.ActualHeight - SVItem.Center.Y - (double)r.Next(50);
                if (SVItem.Center.Y + yOffset < 630.0 * ParentSV.ActualHeight / 1080.0)
                    yOffset = ((630.0 * ParentSV.ActualHeight / 1080.0) - SVItem.Center.Y) + (double)r.Next(50);
                if (SVItem.Center.Y < 630.0 * ParentSV.ActualHeight / 1080.0)
                {
                    centerAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(r.Next(2000, 4000)));
                    centerAnimation.DecelerationRatio = .7;
                }

                centerAnimation.From = SVItem.Center;
                centerAnimation.To = new Point(SVItem.Center.X + xOffset, SVItem.Center.Y + yOffset);
                centerAnimation.FillBehavior = FillBehavior.HoldEnd;
                Storyboard.Children.Add(centerAnimation);
                Storyboard.SetTarget(centerAnimation, SVItem);
                Storyboard.SetTargetProperty(centerAnimation, new PropertyPath(ScatterViewItem.CenterProperty));

                centerAnimation.Completed += new EventHandler(centerAnimation_Completed);

                Storyboard.Begin();
            }
        }

        /// <summary>
        /// Stops a current animation performing.
        /// </summary>
        public void StopAnimation()
        {
            canAnimate = false;
            DispatcherTimer.Stop();
            Storyboard.Pause();
            SVItem.Center = SVItem.ActualCenter;
            SVItem.Orientation = SVItem.Orientation;
            Storyboard.Remove();
        }
        #endregion

        #region Events
        /// <summary>
        /// Event used to animate bubbles
        /// Used as a clock
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void t_Tick(object sender, EventArgs e)
        {
            DispatcherTimer.Stop();
            Animate();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void centerAnimation_Completed(object sender, EventArgs e)
        {
            SVItem.Center = SVItem.ActualCenter;
            Storyboard.Remove();
            Storyboard.Children = new TimelineCollection();
            Random r = new Random();
            DispatcherTimer.Interval = TimeSpan.FromMilliseconds(r.Next(3000, 9000));
            DispatcherTimer.Start();
        }
        /// <summary>
        /// Event occured when a MelodyBubble is released
        /// Magnetise the current bubble
        /// Add the melody in the right stave
        /// Move the bubble to its final place
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void touchLeave(object sender, ContainerManipulationCompletedEventArgs e)
        {
            ScatterViewItem bubble = new ScatterViewItem();
            bubble = e.Source as ScatterViewItem;
            bubbleCenter = bubble.ActualCenter;

            // int width = int.Parse(GetWidth.Text);
            // int height = int.Parse(GetHeight.Text);
            int width = (int)sessionVM.Grid.ActualWidth;
            int height = (int)sessionVM.Grid.ActualHeight;
            bubbleCenter.X = bubbleCenter.X * 1920 / width;
            bubbleCenter.Y = bubbleCenter.Y * 1080 / height;



            if (bubbleCenter.X < 150.0) bubbleCenter.X = 150.0;
            else if (bubbleCenter.X >= 1830) bubbleCenter.X = 1800;
            else bubbleCenter.X = Math.Floor((bubbleCenter.X + 30) / 60) * 60;

            //"Applatissement" de la portée (MAJ : Switch -> Tableau !)

            int offset = GlobalVariables.ManipulationGrid.ElementAtOrDefault((int)((long)bubbleCenter.X / 60));
            bubbleCenter.Y += offset;

            PositionMelody = new Point(bubbleCenter.X, bubbleCenter.Y);
            

            //Y dans le cadre portée ?
            //Si oui, animation
            //pas de else
            if (bubbleCenter.Y < 576 && bubbleCenter.Y > 165)
            {
                if (bubbleCenter.Y < 370.0)
                {
                    if (bubbleCenter.Y >= 344) bubbleCenter.Y = 344;
                    bubbleCenter.Y = Math.Floor((bubbleCenter.Y + 6.0) / 20.0) * 20.0 + 4.0;

                    sessionVM.Session.StaveTop.StopMelody();
                    sessionVM.Session.StaveTop.AddMelody(melodyBubbleVM.MelodyBubble, ((int)PositionMelody.X - 120) / 60);
                    melodyDroppedTopStave = true;

                }
                else
                {
                    if (bubbleCenter.Y <= 395) bubbleCenter.Y = 395;
                    bubbleCenter.Y = Math.Floor((bubbleCenter.Y + 15.0) / 20.0) * 20.0 - 5.0;

                    sessionVM.Session.StaveBottom.StopMelody();
                    sessionVM.Session.StaveBottom.AddMelody(melodyBubbleVM.MelodyBubble, ((int)PositionMelody.X - 120) / 60);
                    melodyDroppedTopStave = false;
                }

                bubbleCenter.Y -= offset;

                bubbleCenter.X = bubbleCenter.X * width / 1920;
                bubbleCenter.Y = bubbleCenter.Y * height / 1080;

                #region STB
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
                #endregion
          
                bubble.Center = bubbleCenter;
                moveCenter.Completed += new EventHandler(moveCenter_Completed);

                stb.Begin(SVItem);

            }
            else
            {
                canAnimate = true;
                Animate();
            }
        }

        void moveCenter_Completed(object sender, EventArgs e)
        {
            List<NoteViewModel> ListOfNotes = melodyBubbleVM.melodyToListOfNote(PositionMelody);
            for (int i = 0; i < ListOfNotes.Count; i++)
            {
                for (int j = 0; j < sessionVM.NotesOnStave.Count; j++)
                {
                    if (ListOfNotes[i].Note.Position == sessionVM.NotesOnStave[j].Note.Position
                        && !sessionVM.NotesOnStave[j].Picked
                        && ListOfNotes[i].Note.Octave == sessionVM.NotesOnStave[j].Note.Octave
                        && ListOfNotes[i].Note.Pitch == sessionVM.NotesOnStave[j].Note.Pitch
                        && ((melodyDroppedTopStave && sessionVM.Session.StaveTop.Notes.Contains(sessionVM.NotesOnStave[j].Note)) ||
                            (!melodyDroppedTopStave && sessionVM.Session.StaveBottom.Notes.Contains(sessionVM.NotesOnStave[j].Note))))
                    {
                        sessionVM.NotesOnStave[j].Animation.BackToBubbleFormat(true);
                        break;
                    }
                }
                sessionVM.NotesOnStave.Add(ListOfNotes[i]);
                sessionVM.Notes.Items.Add(ListOfNotes[i].SVItem);
            }
            sessionVM.Bubbles.Items.Remove(melodyBubbleVM.SVItem);
            sessionVM.MbgVM.MelodyBubbleVMs.Remove(melodyBubbleVM);

        }

        /// <summary>
        /// When the user touchDown on a bubble, it makes a little sound effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void touchDown(object sender, TouchEventArgs e)
        {
            DisplayPreview();
            StopAnimation();
            int time = (melodyBubbleVM.MelodyBubble.Melody.Notes.Last().Position + 1) * (30000 / sessionVM.Session.Bpm);
            if ((playUp % 4) == 0)
            {
                sessionVM.Session.StaveTop.melody = melodyBubbleVM.MelodyBubble.Melody;
                try
                {
                    DisplayHighlightGrid(true, true);
                    topHighlightDt = new DispatcherTimer();
                    topHighlightDt.Interval = TimeSpan.FromMilliseconds(time);
                    topHighlightDt.Tick += new EventHandler(topHighlightDt_Tick);
                    topHighlightDt.Start();
                }
                catch (Exception exc) { }
                sessionVM.Session.StaveTop.StopMelody();
                sessionVM.Session.StaveTop.PlayMelody();
                playUp++;
            }
            else if ((playUp % 4) == 2)
            {
                sessionVM.Session.StaveBottom.melody = melodyBubbleVM.MelodyBubble.Melody;
                try
                {
                    DisplayHighlightGrid(true, false);
                    bottomHighlightDt = new DispatcherTimer();
                    bottomHighlightDt.Interval = TimeSpan.FromMilliseconds(time);
                    bottomHighlightDt.Tick += new EventHandler(bottomHighlightDt_Tick);
                    bottomHighlightDt.Start();
                }
                catch (Exception exc) { }
                sessionVM.Session.StaveBottom.StopMelody();
                sessionVM.Session.StaveBottom.PlayMelody();
                playUp++;
            }
            else
            {
                DisplayHighlightGrid(false, true);
                DisplayHighlightGrid(false, false);
                sessionVM.Session.StaveTop.StopMelody();
                sessionVM.Session.StaveBottom.StopMelody();
                playUp++;
            }
     
        }

        void topHighlightDt_Tick(object sender, EventArgs e)
        {
            topHighlightDt.Stop();
            DisplayHighlightGrid(false, true);
        }

        void bottomHighlightDt_Tick(object sender, EventArgs e)
        {
            bottomHighlightDt.Stop();
            DisplayHighlightGrid(false, false);
        }



        private void DisplayHighlightGrid(bool appear, bool top)
        {
            Storyboard pGSTB = new Storyboard();
            DoubleAnimation previewGridAnimation = new DoubleAnimation();

            if (appear)
            {
                if (top) previewGridAnimation.From = sessionVM.topStaveHighlight.Opacity;
                else previewGridAnimation.From = sessionVM.bottomStaveHighlight.Opacity;
                previewGridAnimation.To = 1;
            }
            else
            {
                if (top) previewGridAnimation.From = sessionVM.topStaveHighlight.Opacity;
                else previewGridAnimation.From = sessionVM.bottomStaveHighlight.Opacity;
                previewGridAnimation.To = 0;
            }
            previewGridAnimation.Duration = new Duration(TimeSpan.FromSeconds(.2));
            previewGridAnimation.FillBehavior = FillBehavior.HoldEnd;
            pGSTB.Children.Add(previewGridAnimation);
            if(top) Storyboard.SetTarget(previewGridAnimation, sessionVM.topStaveHighlight);
            else Storyboard.SetTarget(previewGridAnimation, sessionVM.bottomStaveHighlight);
            Storyboard.SetTargetProperty(previewGridAnimation, new PropertyPath(Grid.OpacityProperty));

            pGSTB.Begin();
        }
        #endregion
    }
}
