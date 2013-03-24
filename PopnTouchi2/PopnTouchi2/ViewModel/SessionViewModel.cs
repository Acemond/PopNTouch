using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopnTouchi2
{
    public class SessionViewModel : PopnTouchi2.Infrastructure.ViewModelBase
    {
        private Session _session;

        public SessionViewModel(Session session)
        {
            throw new System.NotImplementedException();
        }

        public Session Session
        {
            get
            {
                return _session; 
            }
            set
            {
                _session = value;
                NotifyPropertyChanged("Session");
            }
        }
    }
}
