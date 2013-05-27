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
        public static float maxVolume = 0.7f;

        public static int StaveTopFirstDo = 335;
        public static int HeightOfOctave = 175;
        public static int StaveBottomFirstDo = 630;
    }
}
