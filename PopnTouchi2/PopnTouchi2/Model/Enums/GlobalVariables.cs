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
        public static int[] ManipulationGrid = new int[] { 0, 0, 0, 11, 25, 35, 47, 55, 63, 70, 76, 80, 82, 84, 84, 83, 81, 76, 69, 61, 50, 39, 27, 15, 5, -3, -8, -11, -10, -6 };
            
    }
}
