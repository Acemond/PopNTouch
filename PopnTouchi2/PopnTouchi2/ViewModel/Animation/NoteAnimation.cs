using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Input;

namespace PopnTouchi2.ViewModel.Animation
{
    /// <summary>
    /// Defines all animations linked to a NoteBubble item.
    /// </summary>
    public class NoteAnimation : Animation
    {
        #region Properties
        /// <summary>
        /// Property.
        /// Defines the vertical offset of the manipulation grid
        /// </summary>
        private int[] ManipulationGrid { get; set; }

        /// <summary>
        /// Parameter.
        /// Private
        /// </summary>
        private SessionViewModel sessionVM;

        /// <summary>
        /// Property.
        /// The NoteBubbleViewModel associated
        /// </summary>
        private NoteViewModel noteVM { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// NoteBubbleAnimation Constructor.
        /// </summary>
        /// <param name="nbVM">Linked with the NoteBubbleVM</param>
        /// <param name="s">The current SessionViewModel</param>
        public NoteAnimation(NoteViewModel nVM, SessionViewModel s)
            : base()
        {
            sessionVM = s;
            ManipulationGrid = new int[] { 0, 0, 0, 14, 25, 37, 47, 56, 64, 71, 76, 80, 83, 85, 85, 84, 80, 75, 68, 60, 50, 38, 26, 15, 4, -3, -9, -11, -12, -11, -7 };
            noteVM = nVM;
            SVItem = nVM.SVItem;
            ParentSV = nVM.ParentSV;
            canAnimate = true;

            DispatcherTimer.Tick += new EventHandler(t_Tick);

            SVItem.ContainerManipulationCompleted += touchLeave;

            SVItem.PreviewTouchDown += touchDown;
            Animate();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Note Bubble Animation 
        /// Moves the NoteBubble randomly on the screen.
        /// </summary>
        private void Animate()
        {
            
        
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
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void t_Tick(object sender, EventArgs e)
        {
            DispatcherTimer.Stop();
            Animate();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void centerAnimation_Completed(object sender, EventArgs e)
        {
            SVItem.Center = SVItem.ActualCenter;
            Storyboard.Remove();
            Storyboard.Children = new TimelineCollection();
            Random r = new Random();
            DispatcherTimer.Interval = TimeSpan.FromMilliseconds(r.Next(3000, 9000));
            DispatcherTimer.Start();
        }


        /// <summary>
        /// TODO Description détaillée de ce que fait cette méthode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void touchLeave(object sender, ContainerManipulationCompletedEventArgs e)
        {
            ScatterViewItem bubble = new ScatterViewItem();
            bubble = e.Source as ScatterViewItem;
            Point bubbleCenter = bubble.ActualCenter;

            // int width = int.Parse(GetWidth.Text);
            // int height = int.Parse(GetHeight.Text);
            int width = (int)sessionVM.Grid.ActualWidth;
            int height = (int)sessionVM.Grid.ActualHeight;
            bubbleCenter.X = bubbleCenter.X * 1920 / width;
            bubbleCenter.Y = bubbleCenter.Y * 1080 / height;



            if (bubbleCenter.X <= 90) bubbleCenter.X = 120;
            else if (bubbleCenter.X >= 1830) bubbleCenter.X = 1800;
            else bubbleCenter.X = Math.Floor((bubbleCenter.X + 30) / 60) * 60;

            //"Applatissement" de la portée (MAJ : Switch -> Tableau !)
            int offset = ManipulationGrid[((long)bubbleCenter.X / 60)];
            bubbleCenter.Y += offset;

            //   MessageBox.Show(bubbleCenter.X.ToString() + "," + bubbleCenter.Y.ToString());
            int positionNote = (int)(bubbleCenter.X - 120) / 60;

            //Y dans le cadre portée ?
            //Si oui, animation
            //pas de else
            if (bubbleCenter.Y < 630 && bubbleCenter.Y > 105)
            {
                if (bubbleCenter.Y < 370)
                {
                    if (bubbleCenter.Y >= 335) bubbleCenter.Y = 335;
                    bubbleCenter.Y = Math.Floor((bubbleCenter.Y - 20) / 25) * 25 + 35; //-20 et 35 pour 50

                    sessionVM.Session.StaveTop.AddNote(noteVM.Note, positionNote);

                }
                else
                {
                    if (bubbleCenter.Y <= 405) bubbleCenter.Y = 405;
                    bubbleCenter.Y = Math.Floor((bubbleCenter.Y + 10) / 25) * 25 + 5; //20 et 5 pour 50

                    sessionVM.Session.StaveBottom.AddNote(noteVM.Note, positionNote);
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

                stb.Begin(SVItem);

                bubble.Visibility = Visibility.Collapsed;
                bubble.Visibility = Visibility.Visible;

                //sessionVM.Grid.Children.Remove(noteBubbleVM.SVItem);
                //sessionVM.Grid.Children.Add(noteVM.ParentSV);


            }
            else
            {
                canAnimate = true;
                Animate();
            }
        }

        /// <summary>
        /// When the user touchDown on a bubble, it makes a little sound effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void touchDown(object sender, TouchEventArgs e)
        {
            StopAnimation();

            ScatterViewItem note = new ScatterViewItem();
            note = e.Source as ScatterViewItem;
            Point noteCenter = note.ActualCenter;

            ////Si la bulle est sur la portée et qu'on la touche, elle s'enleve de la portee

            sessionVM.Session.StaveTop.RemoveNote(noteVM.Note);
            sessionVM.Session.StaveBottom.RemoveNote(noteVM.Note);

            NoteBubble nb = new NoteBubble(noteVM.Note.Duration);
            NoteBubbleViewModel noteBubbleVM = new NoteBubbleViewModel(noteCenter, nb, sessionVM.Notes, sessionVM);
            sessionVM.Bubbles.Items.Add(noteBubbleVM.SVItem);
            sessionVM.Notes.Items.Remove(noteVM.SVItem);

            /*String effect = "discovery";
            Random r = new Random();
            int nb = r.Next(1, 5);
            effect += nb.ToString();
            AudioController.PlaySoundWithString(effect);*/
        }
        #endregion
    }
}
