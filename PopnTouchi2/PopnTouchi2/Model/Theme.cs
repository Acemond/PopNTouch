﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace PopnTouchi2
{
    public abstract class Theme
    {
        protected Instrument[] _instrumentsTop;
        protected Instrument[] _instrumentsBottom;
        public ImageBrush _backgroundImage { get; set; } //Theme defines Background
        public ImageBrush _noteGeneratorImage { get; set; }  //Theme defines Elements' aspect
        protected Dictionary<NoteValue, ImageBrush> _noteBubbleImages { get; set; }  //Theme defines Elements' aspect
        public ImageBrush _melodyGeneratorImage { get; set; }  //Theme defines Elements' aspect

        public Theme()
        {
            _instrumentsTop = new Instrument[2];
            _instrumentsBottom = new Instrument[2];
            _noteBubbleImages = new Dictionary<NoteValue, ImageBrush>();
        }

        public void generateObjects()
        {
            throw new System.NotImplementedException();
        }

        public virtual ImageBrush getNoteBubbleImage(NoteValue noteValue)
        {
            throw new System.NotImplementedException();
        }

        public Instrument[] getInstrumentsTop()
        {
            return _instrumentsTop;
        }

        public Instrument[] getInstrumentsBottom()
        {
            return _instrumentsBottom;
        }
    }
}
