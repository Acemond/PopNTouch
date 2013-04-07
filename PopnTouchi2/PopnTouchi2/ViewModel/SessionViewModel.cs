using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Infrastructure;

namespace PopnTouchi2
{
    /// <summary>
    /// Binds Session's properties to the View.
    /// </summary>
    public class SessionViewModel : ViewModelBase
    {
        /// <summary>
        /// Parameter.
        /// Session's element from the Model.
        /// </summary>
        private Session session;

        /// <summary>
        /// SessionViewModel Construtor.
        /// TODO
        /// </summary>
        /// <param name="session"></param>
        public SessionViewModel(Session session)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Property.
        /// TODO
        /// </summary>
        public Session Session
        {
            get
            {
                return session; 
            }
            set
            {
                session = value;
                NotifyPropertyChanged("Session");
            }
        }
    }
}
