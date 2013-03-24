using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using PopnTouchi2.Infrastructure;
using System.Threading.Tasks;

namespace PopnTouchi2
{
    public class MainViewModel : ViewModelBase
    {
        public SessionViewModel sessionViewModel;
        private ICommand _play;
        private ICommand _stop;
        private ICommand _changeTheme;

        /// <summary>
        /// Constructeur de la VueModele pricipale
        /// </summary>
        public MainViewModel()
        {
            sessionViewModel = new SessionViewModel(new Session());
        }


        public ICommand Play
        {
            get
            {
                if (_play == null)
                    _play = new RelayCommand(playAction);
                return _play;
            }
            set
            {
            }
        }

        public ICommand Stop
        {
            get
            {
                if (_stop == null)
                    _stop = new RelayCommand(stopAction);
                return _stop;
            }
            set
            {
            }
        }

        public ICommand ChangeTheme
        {
            get
            {
                if (_changeTheme == null)
                    _changeTheme = new RelayCommand(changeThemeAction);
                return _changeTheme;
            }
            set
            {
            }
        }

        public void playAction()
        {
            Task.Factory.StartNew(() =>
            {
                sessionViewModel.Session.StaveTop.playAllNotes();
                sessionViewModel.Session.StaveBottom.playAllNotes();
            });
        }

        public void stopAction()
        {
            Task.Factory.StartNew(() =>
            {
                sessionViewModel.Session.StaveTop.stopMusic();
                sessionViewModel.Session.StaveBottom.stopMusic();
            });
        }

        public void changeThemeAction()
        {
            /*Task.Factory.StartNew(() =>
            {
                sessionViewModel.Session
                sessionViewModel.Session.StaveBottom.stopMusic();
            });*/ 
        }
    }
}
