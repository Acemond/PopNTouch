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
        public ObservableCollection<Note> StaveTopNotes { get; set; }

        /// <summary>
        /// Property.
        /// Session's lower Stave instance.
        /// </summary>
        public ObservableCollection<Note> StaveBottomNotes { get; set; }

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
            StaveTopNotes = sessionVM.Session.StaveTop.Notes;
            StaveBottomNotes = sessionVM.Session.StaveBottom.Notes;
            //NotesSV = sessionVM.Notes;
            ThemeID = sessionVM.Session.ThemeID;
        }
    }
}
