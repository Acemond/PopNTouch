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

      //  public event ContainerManipulationCompletedEventArgs dropBubble;
    
        public NoteBubble Note
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public bool IsOnStave
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    }
}
