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
        /// <summary>
        /// The tempo of the staves
        /// </summary>
        public static int bpm = 90;

        /// <summary>
        /// identify the notebubble
        /// </summary>
        public static int idNoteBubble = 0;

        /// <summary>
        /// identify the notebubble
        /// </summary>
        public static int idMelodyBubble = 0;

        public static Random GlobalRandom = new Random();

        /// <summary>
        /// The array containing the offsets of the staves
        /// </summary>
        public static int[] ManipulationGrid = new int[] { 0, 0, 0, 11, 25, 35, 47, 55, 63, 70, 76, 80, 82, 84, 84, 82, 80, 75, 68, 60, 49, 38, 26, 14, 4, -4, -9, -12, -12, -11, -7 };
        
        public static int StaveTopFirstDo = 335;
        public static int HeightOfOctave = 175;
        public static int StaveBottomFirstDo = 630;
        public static int MaxNoteBubbles = 14;
        public static int MaxMelodyBubbles = 6;

    }
}
