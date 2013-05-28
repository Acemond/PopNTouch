using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Surface.Presentation.Controls;
using PopnTouchi2.ViewModel;

namespace PopnTouchi2.Model
{
    /// <summary>
    /// Save the data of the session
    /// </summary>
    [Serializable]
    public class SessionData
    {
        #region Properties
        /// <summary>
        /// Property.
        /// Session's upper Stave instance.
        /// </summary>
        public List<Note> StaveTopNotes { get; set; }

        /// <summary>
        /// Property.
        /// Session's lower Stave instance.
        /// </summary>
        public List<Note> StaveBottomNotes { get; set; }

        public Instrument TopInstrument { get; set; }

        public Instrument BottomInstrument { get; set; }

        /// <summary>
        /// Property.
        /// Session's ScatterView.
        /// </summary>
        //public ScatterView NotesSV { get; set; }

        /// <summary>
        /// Property.
        /// Session's Theme instance.
        /// </summary>
        public int ThemeID { get; set; }
        #endregion

        public SessionData(SessionViewModel sessionVM)
        {
            StaveTopNotes = new List<Note>();
            StaveBottomNotes = new List<Note>();

            foreach (Note note in sessionVM.Session.StaveTop.Notes)
                StaveTopNotes.Add(note);
            foreach (Note note in sessionVM.Session.StaveBottom.Notes)
                StaveBottomNotes.Add(note);

            TopInstrument = sessionVM.Session.StaveTop.CurrentInstrument;
            BottomInstrument = sessionVM.Session.StaveBottom.CurrentInstrument;

            ThemeID = sessionVM.Session.ThemeID;
        }
    }
}
