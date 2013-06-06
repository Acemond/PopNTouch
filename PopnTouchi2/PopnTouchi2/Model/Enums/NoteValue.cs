using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopnTouchi2
{
    /// <summary>
    /// Sets Notes value expressed in half time.
    /// </summary>
    public enum NoteValue
    {
        //quaver = croche
        quaver = 1,
        //crotchet = noire
        crotchet = 2,
        //minim = blanche
        minim = 4,
        //Used for the creation of a (#) or (b) bubble
        alteration = 8
    }
}
