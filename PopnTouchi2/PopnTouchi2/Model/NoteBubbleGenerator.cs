using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace PopnTouchi2
{
    public class NoteBubbleGenerator
    {

        public NoteBubbleGenerator()
        {
            _noteBubbles = new ObservableCollection<NoteBubble>();
        }

        public NoteBubble NoteBubbles
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public void createNoteBubble()
        {
           //algo pour creer une NoteBubbles
        }

        public void removeFromGenerator(int idNote)
        {
            for (int i = 0; i < _noteBubbles.Count; i++)
            {
                if (_noteBubbles[i].Id == idNote)
                    _noteBubbles.RemoveAt(i);
            }
        }
    }
}
