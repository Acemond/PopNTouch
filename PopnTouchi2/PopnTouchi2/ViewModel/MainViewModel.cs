using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using PopnTouchi2.Infrastructure;
using System.Threading.Tasks;

namespace PopnTouchi2
{
    public class MainViewModel : PopnTouchi2.Infrastructure.ViewModelBase
    {
        public PopnTouchi2.SessionViewModel sessionViewModel;
        private ICommand play;
        private ICommand stop;
        private int changeTheme;

        /// <summary>
        /// Constructeur de la VueModele pricipale
        /// </summary>
        public MainViewModel()
        {
          
        }

        public System.Windows.Input.ICommand Play
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public System.Windows.Input.ICommand Stop
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public System.Windows.Input.ICommand ChangeTheme
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public void playAction()
        {
            throw new System.NotImplementedException();
        }

        public void stopAction()
        {
            throw new System.NotImplementedException();
        }

        public void changeThemeAction()
        {
            throw new System.NotImplementedException();
        }
/*
        private Play _play;
        public Play Play
        {
            get
            {
                if (_play == null)
                    _play = new RelayCommand(play_action);
                return _play;
            }
        }

        private void play_action()
        {
            // création d'un thread pour lancer le calcul du tour suivant sans que cela soit bloquant pour l'IHM
            Task.Factory.StartNew(() =>
            {
                sessionViewModel.StaveTop.playAllNotes();
                stave2.StaveTop.playAllNotes();
            });
        }*/
    }
}
