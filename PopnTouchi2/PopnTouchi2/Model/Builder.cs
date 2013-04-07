using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;

namespace PopnTouchi2
{
    /// <summary>
    /// Creates game environnement including sessions.
    /// </summary>
    public class Builder
    {
        /// <summary>
        /// Builder Constructor.
        /// </summary>
        public Builder()
        {

        }

        /// <summary>
        /// Creates a new game session.
        /// </summary>
        /// <returns>The session newly created</returns>
        public Session GenerateSession()
        {
            return new Session();
        }
    }
}
