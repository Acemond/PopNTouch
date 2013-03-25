using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Controls;
using Microsoft.Surface.Presentation.Controls;
using System.Windows;

namespace PopnTouchi2
{
    public class NoteBubbleGenerator : Grid
    {
        private ObservableCollection<NoteBubble> _noteBubbles;
        public Dictionary<NoteValue, int> wildBubbles { get; set; }

        public NoteBubbleGenerator()
        {
            _noteBubbles = new ObservableCollection<NoteBubble>();
            wildBubbles = new Dictionary<NoteValue, int>();
            wildBubbles.Add(NoteValue.crotchet, 0);
            wildBubbles.Add(NoteValue.minim, 0);
            wildBubbles.Add(NoteValue.quaver, 0);

            //Defines size and position
            this.Width = 368;
            this.Height = 234;
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;

            this.TouchDown += new EventHandler<System.Windows.Input.TouchEventArgs>(NoteBubbleGenerator_TouchDown);
        }

        public NoteBubbleGenerator(Theme theme): this()
        {
            Background = theme._noteGeneratorImage;
        }

        //Creates a new bubble with a given theme
        public NoteBubble createNoteBubble(Theme theme)
        {
            return new NoteBubble(mostNeeded(), theme);
        }

        //Returns the most needed note according to random algorithm
        private NoteValue mostNeeded()
        {
            NoteValue mostNeededNote = NoteValue.crotchet;
            Random rand = new Random();
            try
            {
                int min = wildBubbles.Values.Min();
                List<NoteValue> res = new List<NoteValue>(3);
                foreach (System.Collections.Generic.KeyValuePair<NoteValue, int> nv in wildBubbles)
                    if (nv.Value == min) res.Add(nv.Key);

                mostNeededNote = res[rand.Next(res.Count)];
                wildBubbles[mostNeededNote]++;
            }
            catch (Exception e)
            {
                switch (rand.Next(2))
                {
                    case 0: mostNeededNote = NoteValue.minim; break;
                    case 1: mostNeededNote = NoteValue.quaver; break;
                    default: mostNeededNote = NoteValue.crotchet; break;
                }
            }
            return mostNeededNote;
        }

        public void removeFromGenerator(int idNote)
        {
            for (int i = 0; i < _noteBubbles.Count; i++)
            {
                if (_noteBubbles[i].Id == idNote)
                    _noteBubbles.RemoveAt(i);
            }
        }

        void NoteBubbleGenerator_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            NoteBubble newBubble = createNoteBubble(((Session)this.Parent).Theme);
            _noteBubbles.Add(newBubble);

            ((Session)this.Parent).Bubbles.Items.Add(newBubble);
        }
    }
}
