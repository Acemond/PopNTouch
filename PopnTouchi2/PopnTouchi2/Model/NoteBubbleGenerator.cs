using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace PopnTouchi2
{
    public class NoteBubbleGenerator
    {
        public ObservableCollection<NoteBubble> _noteBubbles;

        public NoteBubbleGenerator()
        {
            _noteBubbles = new ObservableCollection<NoteBubble>();
        }

        public void createNoteBubble()
        {
           //algo pour creer une NoteBubble
        }

        public void removeFromGenerator(int idNote)
        {
            for (int i = 0; i < _noteBubbles.Count; i++)
            {
                if (_noteBubbles[i]._id == idNote)
                    _noteBubbles.RemoveAt(i);
            }
        }
    }
}
