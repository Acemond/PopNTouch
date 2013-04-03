using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace PopnTouchi2
{
    public class MelodyBubbleGenerator : Grid
    {
        public MelodyBubbleGenerator()
        {
            MelodyBubbles = new List<MelodyBubble>();

            //Defines size and position
            this.Width = 368;
            this.Height = 234;
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;

            this.TouchDown += new EventHandler<System.Windows.Input.TouchEventArgs>(MelodyBubbleGenerator_TouchDown);
        }

        public MelodyBubbleGenerator(Theme theme): this()
        {
            Background = theme._melodyGeneratorImage;
        }

        public List<MelodyBubble> MelodyBubbles
        {
            get;
            set;
        }

        public void createMelodyBubble()
        {
            throw new System.NotImplementedException();
        }

        public void removeFromGenerator(MelodyBubble melody)
        {
            throw new System.NotImplementedException();
        }

        private void MelodyBubbleGenerator_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
