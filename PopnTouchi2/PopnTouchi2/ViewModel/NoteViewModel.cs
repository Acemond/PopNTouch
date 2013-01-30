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
                NotifyPropertyChanged("Note");
            }
        }

        bool _isOnStave;
        public bool IsOnStave
        {
            get
            {
                return _isOnStave;
            }
            set
            {
                _isOnStave = value;
                NotifyPropertyChanged("IsOnStave");
            }
        }
    }
}
