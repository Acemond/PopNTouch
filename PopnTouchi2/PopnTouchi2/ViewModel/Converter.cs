using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Model.Enums;
using System.Windows;

namespace PopnTouchi2.ViewModel
{
    /// <summary>
    /// Class used to convert Positions to Pitch and vice versa
    /// </summary>
    public class Converter
    {
        /// <summary>
        /// Dictionary(double,String)
        /// A link between positions and pitches
        /// </summary>
        public Dictionary<double, String> PositionToPitch { get; set; }

        /// <summary>
        /// Constructor.
        /// Create the dictionary
        /// </summary>
        public Converter()
        {
            PositionToPitch = new Dictionary<double, String>();
    //        PositionToPitch.Add(164, "top_sol_2");
            PositionToPitch.Add(184, "top_fa_2");
            PositionToPitch.Add(204, "top_mi_2");
            PositionToPitch.Add(224, "top_re_2");
            PositionToPitch.Add(244, "top_do_2");
            PositionToPitch.Add(264, "top_si_1");
            PositionToPitch.Add(284, "top_la_1");
            PositionToPitch.Add(304, "top_sol_1");
            PositionToPitch.Add(324, "top_fa_1");
            PositionToPitch.Add(344, "top_mi_1");

            PositionToPitch.Add(355, "bottom_la_2");
            PositionToPitch.Add(375, "bottom_sol_2");
            PositionToPitch.Add(395, "bottom_fa_2");
            PositionToPitch.Add(415, "bottom_mi_2");
            PositionToPitch.Add(435, "bottom_re_2");
            PositionToPitch.Add(455, "bottom_do_2");
            PositionToPitch.Add(475, "bottom_si_1");
            PositionToPitch.Add(495, "bottom_la_1");
            PositionToPitch.Add(515, "bottom_sol_1");
            PositionToPitch.Add(535, "bottom_fa_1");
            PositionToPitch.Add(555, "bottom_mi_1");
            PositionToPitch.Add(575, "bottom_re_1");
        }

        /// <summary>
        /// getCenterY(bool up, Note note)
        /// Return the positionY of a note 
        /// which is on the staveTop or the staveBottom
        /// </summary>
        /// <param name="up">up = staveTop , !up = staveBottom</param>
        /// <param name="note">The note</param>
        /// <returns>The positionY of the note</returns>
        public double getCenterY (bool up, Note note)
        {
            String Pitch = "";
            if (up) Pitch = "top_";
            else Pitch = "bottom_";

            Pitch += note.Pitch + "_";
            Pitch += note.Octave.ToString();

            double res = 0;
            foreach(double d in PositionToPitch.Keys)
            {
                if(PositionToPitch[d] == Pitch)
                    res = d;
            }
            return res - GlobalVariables.ManipulationGrid.ElementAtOrDefault(note.Position + 2);

        }

        /// <summary>
        /// getStave(double positionY)
        /// Return the "top" or "bottom" 
        /// with the dictionary
        /// </summary>
        /// <param name="positionY">The positionY of the object</param>
        /// <returns>The stave "top" or "bottom"</returns>
        public String getStave(double positionY)
        {
            String res = PositionToPitch[positionY];
            String[] split = res.Split('_');
            return split[0];
        }

        /// <summary>
        /// getPitch(double positionY)
        /// Returns the Pitch corresponding 
        /// with the dictionary
        /// </summary>
        /// <param name="positionY">The positionY</param>
        /// <returns>The pitch of the note , "do", "re",...</returns>
        public String getPitch(double positionY)
        {
            String res = PositionToPitch[positionY];
            String[] split = res.Split('_');
            return split[1];
        }

        /// <summary>
        /// getOctave(double positionY)
        /// Returns the Octave corresponding 
        /// with the dictionary
        /// </summary>
        /// <param name="positionY">The PositionY</param>
        /// <returns>Return the Octave : {1,2}</returns>
        public int getOctave(double positionY)
        {
            String res = PositionToPitch[positionY];
            String[] split = res.Split('_');
            return (int)Double.Parse(split[2]);
        }
    }
}
