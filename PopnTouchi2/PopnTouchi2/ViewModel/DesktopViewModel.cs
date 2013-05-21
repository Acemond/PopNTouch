using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PopnTouchi2.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Surface.Presentation.Controls;

namespace PopnTouchi2.ViewModel
{
    /// <summary>
    /// TODO
    /// </summary>
    public class DesktopViewModel : ViewModelBase
    {
        /// <summary>
        /// Parameter.
        /// TODO
        /// </summary>
        private ICommand play;
        /// <summary>
        /// Parameter.
        /// TODO
        /// </summary>
        private ICommand stop;
        /// <summary>
        /// Parameter.
        /// TODO
        /// </summary>
        private ICommand changeTheme;

        /// <summary>
        /// Property.
        /// ScatterView Containing the session as photos.
        /// </summary>
        public ScatterView Photos { get; set; }

        /// <summary>
        /// Property.
        /// Temporary Button.
        /// </summary>
        public SurfaceButton CreateSession { get; set; }

        /// <summary>
        /// Grid containing all items.
        /// </summary>
        public Grid Grid { get; set; }

        /// <summary>
        /// MainViewModel Constructor.
        /// Initializes the SessionViewModel.
        /// </summary>
        public DesktopViewModel() : base()
        {
            Grid = new Grid();
            CreateSession = new SurfaceButton();
            CreateSession.Width = 200;
            CreateSession.Height = 75;
            CreateSession.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CreateSession.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            CreateSession.Content = "Generate new Session !!!";
            CreateSession.Click += new RoutedEventHandler(CreateSession_Click);
            Grid.Children.Add(CreateSession);

            Photos = new ScatterView();
            Grid.Children.Add(Photos);

            Grid.Children.Add((new SessionViewModel(new Session())).Grid);
            SessionVM = new SessionViewModel(new Session());
        }

        /// <summary>
        /// Command which play the music
        /// </summary>
        public ICommand Play
        {
            get
            {
                if (play == null)
                    play = new RelayCommand(PlayAction);
                return play;
            }
            set
            {
            }
        }

        /// <summary>
        /// Command which stop the played music
        /// </summary>
        public ICommand Stop
        {
            get
            {
                if (stop == null)
                    stop = new RelayCommand(StopAction);
                return stop;
            }
            set
            {
            }
        }

        /// <summary>
        /// The command to change the theme
        /// </summary>
        public ICommand ChangeTheme
        {
            get
            {
                if (changeTheme == null)
                    changeTheme = new RelayCommand(ChangeThemeAction);
                return changeTheme;
            }
            set
            {
            }
        }

        /// <summary>
        /// The action of playing the music
        /// </summary>
        public void PlayAction()
        {
            Task.Factory.StartNew(() =>
            {
                AudioController.FadeOutBackgroundSound();
                Thread.Sleep(500);
                SessionVM.Session.StaveTop.PlayAllNotes();
                SessionVM.Session.StaveBottom.PlayAllNotes();
            });
        }

        /// <summary>
        /// The action of stoping the music
        /// </summary>
        public void StopAction()
        {
            Task.Factory.StartNew(() =>
            {
                SessionVM.Session.StaveTop.StopMusic();
                SessionVM.Session.StaveBottom.StopMusic();
                Thread.Sleep(10000);
                AudioController.FadeInBackgroundSound();
            });
        }

        /// <summary>
        /// The action of changing the theme
        /// </summary>
        public void ChangeThemeAction()
        {
            /*Task.Factory.StartNew(() =>
            {
                SessionVM.Session
                SessionVM.Session.StaveBottom.stopMusic();
            });*/ 
        }

        void CreateSession_Click(object sender, RoutedEventArgs e)
        {
            Grid.Children.Add((new SessionViewModel(new Session())).Grid);
        }
    }
}
