using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using PopnTouchi2.Infrastructure;
using System.Threading.Tasks;

namespace PopnTouchi2
{
    /// <summary>
    /// TODO
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Parameter.
        /// Session's View Model.
        /// </summary>
        public SessionViewModel sessionViewModel;
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
        /// MainViewModel Constructor.
        /// Initializes the SessionViewModel.
        /// </summary>
        public MainViewModel()
        {
            sessionViewModel = new SessionViewModel(new Session());
        }

        /// <summary>
        /// TODO
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
        /// TODO
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
        /// TODO
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
        /// TODO
        /// </summary>
        public void PlayAction()
        {
            Task.Factory.StartNew(() =>
            {
                sessionViewModel.Session.StaveTop.PlayAllNotes();
                sessionViewModel.Session.StaveBottom.PlayAllNotes();
            });
        }

        /// <summary>
        /// TODO
        /// </summary>
        public void StopAction()
        {
            Task.Factory.StartNew(() =>
            {
                sessionViewModel.Session.StaveTop.StopMusic();
                sessionViewModel.Session.StaveBottom.StopMusic();
            });
        }

        /// <summary>
        /// TODO
        /// </summary>
        public void ChangeThemeAction()
        {
            /*Task.Factory.StartNew(() =>
            {
                sessionViewModel.Session
                sessionViewModel.Session.StaveBottom.stopMusic();
            });*/ 
        }
    }
}
