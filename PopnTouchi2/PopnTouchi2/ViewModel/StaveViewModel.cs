using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Infrastructure;

namespace PopnTouchi2
{
    public class StaveViewModel : ViewModelBase
    {
        private int _instrument;
        private Stave _stave;

        public int CurrentInstrument
        {
            get
            {
                return _instrument; 
            }
            set
            {
               _instrument = value;
                NotifyPropertyChanged("CurrentInstrument");
            }
        }

        public Stave Stave
        {
            get
            {
                return _stave; 
            }
            set
            {
                _stave = value;
                NotifyPropertyChanged("Stave");
            }
        }
    }
}
