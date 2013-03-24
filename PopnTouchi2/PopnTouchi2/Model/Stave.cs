using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Timers;
using PopnTouchi2.Model.Enums;

namespace PopnTouchi2
{
    public class Stave
    {

        public Stave(Instrument instru)
        {
            MaxPosition = 0;
            Notes = new ObservableCollection<Note>();
            CurrentInstrument = instru;
            timer = new Timer();
        }

        public ObservableCollection<Note> Notes { get; set; }

        public Instrument CurrentInstrument { get; set; }

        public int MaxPosition { get; set; } 

        private Timer timer;


        public void addMelody(MelodyBubble mb, int position)
        {
            int cardMelody = mb.Melody.Notes.Count;
            for(int i = 0; i< cardMelody ; i++)
            {
                mb.Melody.Notes[i].Position += position;
                int posArray = position;
                while (Notes[posArray++].Position < mb.Melody.Notes[i].Position) ;

                Notes.Insert(posArray, mb.Melody.Notes[i]);
                }
        }

        public void addNote(Note note, int position)
        {
            note.Position = position;
            int i=0;
            while (Notes[i++].Position < position) ;

            Notes.Insert(i, note);
            MaxPosition = Math.Max(MaxPosition, note.Position);
        }

        public void trier()
        {
            Notes.OrderBy(note => note.Position);
        }

        public void playAllNotes()
        {
            timer.Interval = 30000 / GlobalVariables.bpm;
            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(Play_List);
        }

        private void Play_List(object source, ElapsedEventArgs e)
        {
            bool play = true;
            if (GlobalVariables.position_Note <= MaxPosition + 4)
            {
                //for (int i = 0; i < Notes.Count; i++)
                //{

                //    if (Notes[i].Position == GlobalVariables.position_Note)
                //    {
                //        CurrentInstrument.playNote(Notes[i]);
                //    }
                //}
               
                while (play && (GlobalVariables.it_Notes < Notes.Count))
                {
                    if (Notes[GlobalVariables.it_Notes].Position == GlobalVariables.position_Note)
                    {
                        CurrentInstrument.playNote(Notes[GlobalVariables.it_Notes]);
                        GlobalVariables.it_Notes++;
                    }
                    else
                    {
                        play = false;
                    }
                }
               
                GlobalVariables.position_Note++;

            }
            else
            {
                timer.Stop();
                timer.EndInit();
                timer.Elapsed -= new ElapsedEventHandler(Play_List);
                GlobalVariables.position_Note = 0;
                GlobalVariables.it_Notes = 0;
            }
        }

        public void stopMusic()
        {
            timer.Stop();
            timer.EndInit();
            timer.Elapsed -= new ElapsedEventHandler(Play_List);
            GlobalVariables.position_Note = 0;
            GlobalVariables.it_Notes = 0;
        }

    }
}
