using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Infrastructure;

using PopnTouchi2.ViewModel.Animation;

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
        /// Parameter.
        /// StaveAnimation item handling all animations related to the stave.
        /// </summary>
        private StaveAnimation animation;

        /// <summary>
        /// StaveViewModel Constructor.
        /// TODO
        /// </summary>
        public StaveViewModel(Stave s)
        {
            stave = s;
            if (s.isUp)
            {
                //TODO Image =
            }
            else
            {
                //TODO Image =
            }

            animation = new StaveAnimation(s);
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
    }
}
