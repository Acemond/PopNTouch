using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PopnTouchi2.Infrastructure
{
    /// <summary>
    /// Base Template for a ViewModel Class
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Paramater.
        /// Event triggered when a Property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies when a Property has changed using the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The Name of the property</param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
