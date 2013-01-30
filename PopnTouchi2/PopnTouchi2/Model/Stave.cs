using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace PopnTouchi2
{
    public class Stave
    {
        private int _maxNote;
        private int _nbNoteChord;
        private Instrument _currentInstrument;

        public Stave(List<Instrument> list, int max, int nbNoteChord)
        {
            throw new System.NotImplementedException();
        }

        public ObservableCollection<Note> Notes
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public List<Instrument> Instruments
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public void addMelody(MelodyBubble mb)
        {
            throw new System.NotImplementedException();
        }

        public void addNote(NoteBubble nb)
        {
            throw new System.NotImplementedException();
        }

        public void playAllNotes()
        {
            throw new System.NotImplementedException();
        }
    }
}
