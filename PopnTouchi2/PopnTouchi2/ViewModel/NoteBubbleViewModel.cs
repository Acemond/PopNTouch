using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;
using PopnTouchi2.Infrastructure;

namespace PopnTouchi2
{
    public class NoteBubbleViewModel : PopnTouchi2.Infrastructure.ViewModelBase
    {
        private NoteBubble _noteBubble;
        private bool _isOnStave;

        public event EventHandler dropBubble;

     
        public NoteBubble Note
        {
            get
            {
                return _noteBubble;
            }
            set
            {
                _noteBubble = value;
                NotifyPropertyChanged("Note");
            }
        }

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
