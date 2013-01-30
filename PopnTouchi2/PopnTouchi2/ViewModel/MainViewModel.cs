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
        public StaveViewModel stave1;
        public StaveViewModel stave2;

        /// <summary>
        /// Constructeur de la VueModele pricipale
        /// </summary>
        public MainViewModel()
        {
          
        }
/*
        private ICommand _play;
        public ICommand Play
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
                stave1.Stave.playAllNotes();
                stave2.Stave.playAllNotes();
            });
        }*/
    }
}
