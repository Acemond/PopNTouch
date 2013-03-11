using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace PopnTouchi2
{
    public class NoteBubbleGenerator
    {
        private ObservableCollection<NoteBubble> _noteBubbles;
        public NoteBubble NoteBubbles { get; set; }

        public NoteBubbleGenerator()
        {
            _noteBubbles = new ObservableCollection<NoteBubble>();
        }

        public void createNoteBubble()
        {
            //algo pour creer une NoteBubbles
            int[] values = { 0, 0, 0 };
            for (int i = 0; i < _noteBubbles.Count; i++)
            {
                switch (_noteBubbles[i].Note.Duration)
                {
                    case NoteValue.quaver: values[0]++;
                        break;
                    case NoteValue.crotchet: values[1]++;
                        break;
                    case NoteValue.minim: values[2]++;
                        break;
                }
            }

            NoteBubble n = new NoteBubble(values.Min());
            _noteBubbles.Add(n);
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
