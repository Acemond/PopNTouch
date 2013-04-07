using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Infrastructure;

namespace PopnTouchi2
{
    /// <summary>
    /// Binds Stave's properties to the View.
    /// </summary>
    public class StaveViewModel : ViewModelBase
    {
        /// <summary>
        /// Parameter.
        /// Instrument identifier.
        /// </summary>
        private int instrument;

        /// <summary>
        /// Parameter.
        /// Stave element from the Model.
        /// </summary>
        private Stave stave;

        /// <summary>
        /// StaveViewModel Constructor.
        /// TODO
        /// </summary>
        public StaveViewModel()
        {
        }

        /// <summary>
        /// Property.
        /// TODO
        /// </summary>
        public int CurrentInstrument
        {
            get
            {
                return instrument; 
            }
            set
            {
               instrument = value;
                NotifyPropertyChanged("CurrentInstrument");
            }
        }

        /// <summary>
        /// Property.
        /// TODO
        /// </summary>
        public Stave Stave
        {
            get
            {
                return stave; 
            }
            set
            {
                stave = value;
                NotifyPropertyChanged("Stave");
            }
        }
    }
}
