using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopnTouchi2.ViewModel
{
    public class Converter
    {
        public List<String> PositionToPitch { get; set; }

        public int StaveTopFirstDo = 280;

        public int StaveBottomFirstDo = 630;

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
    }
}
