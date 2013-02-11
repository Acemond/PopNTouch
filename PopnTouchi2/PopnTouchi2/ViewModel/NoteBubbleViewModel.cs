using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopnTouchi2
{
    public class NoteBubbleViewModel
    {
        private NoteBubble _noteBubble;
        private bool _isOnStave;

        public event ContainerManipulationCompletedEventArgs dropBubble;
    
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
