using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PopnTouchi2.ViewModel;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("SessionViewModel")]
[assembly: InternalsVisibleTo("DesktopViewModel")]

namespace PopnTouchi2.Infrastructure
{
    /// <summary>
    /// Base Template for a ViewModel Class
    /// </summary>
    public class ViewModelBase
    {
        /// <summary>
        /// Property.
        /// The current sessionVM shared by all objects.
        /// </summary>
        protected SessionViewModel SessionVM { get; set; }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        internal ViewModelBase() { }

        /// <summary>
        /// Constructor specific to a sessionviewmodel.
        /// </summary>
        /// <param name="s"></param>
        public ViewModelBase(SessionViewModel s)
        {
            SessionVM = s;
        }
    }
}
