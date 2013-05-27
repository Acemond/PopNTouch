using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Model.Enums;

namespace PopnTouchi2.ViewModel
{
    public class Converter
    {
        public List<String> PositionToPitch { get; set; }

        public Converter()
        {
            PositionToPitch = new List<String>();
            PositionToPitch.Add("do");
            PositionToPitch.Add("re");
            PositionToPitch.Add("mi");
            PositionToPitch.Add("fa");
            PositionToPitch.Add("sol");
            PositionToPitch.Add("la");
            PositionToPitch.Add("si");
        }

        public double getCenterY (bool up, Note note, double centerX)
        {
            int octave = note.Octave;
            String pitch = note.Pitch;
            int positionNote = (int)(centerX - 120) / 60;
            int offset = GlobalVariables.ManipulationGrid[positionNote];
            double centerY = -offset;

            if (up)
            {
                centerY += GlobalVariables.StaveTopFirstDo - ((octave - 1) * 7 + PositionToPitch.IndexOf(pitch)) * 25;
            }
            else
            {
                centerY += GlobalVariables.StaveBottomFirstDo - ((octave - 1) * 7 + PositionToPitch.IndexOf(pitch)) * 25;
            }
            return centerY;
        }
    }
}
