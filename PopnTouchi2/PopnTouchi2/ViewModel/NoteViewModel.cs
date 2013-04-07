using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Infrastructure;

namespace PopnTouchi2
{
    /// <summary>
    /// Binds Note's properties to the View.
    /// </summary>
    public class NoteViewModel : ViewModelBase
    {
        /// <summary>
        /// Parameter.
        /// Note element from the Model.
        /// </summary>
        private Note note;

        /// <summary>
        /// Parameter.
        /// Event triggered when the Note is tapped.
        /// </summary>
        public event EventHandler tap;

        /// <summary>
        /// Parameter.
        /// Event triggered when the Note is dropped on the stave.
        /// </summary>
        public event EventHandler dropNote;

        /// <summary>
        /// NoteViewModel Constructor.
        /// TODO
        /// </summary>
        public NoteViewModel()
        {
        }

        /// <summary>
        /// Property.
        /// TODO
        /// </summary>
        public Note Note
        {
            get
            {
                return note;
            }
            set
            {
                note = value;
                NotifyPropertyChanged("Notes");
            }
        }
    }
}
