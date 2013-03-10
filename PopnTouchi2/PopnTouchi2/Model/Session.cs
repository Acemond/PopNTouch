using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Model.Enums;

namespace PopnTouchi2
{
    public class Session
    {
        //<<<<<<< HEAD
        private Theme theme;


        public MelodyBubbleGenerator MelodyBubbleGenerator
        //>>>>>>> cfc74d4739031189b8bf39e1f3ea6da345159809
        {
            get;
            set;
        }

        public NoteBubbleGenerator NoteBubbleGenerator
        {
            get;
            set;
        }

        public Stave StaveTop
        {
            get;
            set;
        }

        public Stave StaveBottom
        {
            get;
            set;
        }

        public Theme Theme
        {
            get
            {
                return this.theme;
            }
            set
            {
            }
        }

        public Session()
        {
            theme = new Theme1();
            MelodyBubbleGenerator = new MelodyBubbleGenerator();
            NoteBubbleGenerator = new NoteBubbleGenerator();
            StaveTop = new Stave(theme.getInstrumentsTop());
            StaveBottom = new Stave(theme.getInstrumentsBottom());
        }


        public void changeBpm(int newBpm)
        {
            GlobalVariables.bpm = newBpm;
        }

        public void reduce()
        {
            throw new System.NotImplementedException();
        }

        public void enlarge()
        {
            throw new System.NotImplementedException();
        }
    }
}
