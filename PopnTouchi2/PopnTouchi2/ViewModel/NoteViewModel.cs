using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Infrastructure;

namespace PopnTouchi2
{
    public class NoteViewModel : ViewModelBase
    {
        private Note _note;
        public Note Note
        {
            get
            {
                return _note;
            }
            set
            {
                _note = value;
                NotifyPropertyChanged("Notes");
            }
        }
    }
}
