using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopnTouchi2.Model.Enums
{
    /// <summary>
    /// Sets all shared global variables.
    /// </summary>
    public class GlobalVariables
    {
        public static int bpm = 90;
        public static int position_NoteUp = 0;
        public static int position_NoteDown = 0;
        public static int position_Melody = 0;
        public static int idNoteBubble = 0;
        public static int idMelodyBubble = 0;
        public static int StaveTopFirstDo = 335;
        public static int HeightOfOctave = 175;
        public static int StaveBottomFirstDo = 630;
        public static int[] ManipulationGrid = new int[] { 0, 0, 0, 14, 25, 37, 47, 56, 64, 71, 76, 80, 83, 85, 85, 84, 80, 75, 68, 60, 50, 38, 26, 15, 4, -3, -9, -11, -12, -11, -7 };
            
    }
}
