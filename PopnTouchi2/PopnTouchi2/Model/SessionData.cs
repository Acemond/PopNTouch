﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Surface.Presentation.Controls;
using PopnTouchi2.ViewModel;
using PopnTouchi2.Model.Enums;

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

        /// <summary>
        /// Property.
        /// The current Instrument of the StaveTop
        /// </summary>
        public Instrument TopInstrument { get; set; }

        /// <summary>
        /// Property.
        /// The current Instrument of the StaveBottom
        /// </summary>
        public Instrument BottomInstrument { get; set; }

        /// <summary>
        /// Property.
        /// Session's ID instance.
        /// </summary>
        public int SessionID { get; set; }

        /// <summary>
        /// Property.
        /// The tempo of the session
        /// </summary>
        public int bpm { get; set; }

        /// <summary>
        /// Property.
        /// Session's Theme instance.
        /// </summary>
        public int ThemeID { get; set; }
        #endregion

        /// <summary>
        /// Load the data from the sessionVM
        /// </summary>
        /// <param name="sessionVM">The loaded SessionViewModel</param>
        public SessionData(SessionViewModel sessionVM)
        {
            StaveTopNotes = new List<Note>();
            StaveBottomNotes = new List<Note>();

            foreach (Note note in sessionVM.Session.StaveTop.Notes)
                StaveTopNotes.Add(note);
            foreach (Note note in sessionVM.Session.StaveBottom.Notes)
                StaveBottomNotes.Add(note);

            bpm = sessionVM.Session.Bpm;

            SessionID = sessionVM.SessionID;
            TopInstrument = sessionVM.Session.StaveTop.CurrentInstrument;
            BottomInstrument = sessionVM.Session.StaveBottom.CurrentInstrument;

            ThemeID = sessionVM.Session.ThemeID;
        }
    }
}
