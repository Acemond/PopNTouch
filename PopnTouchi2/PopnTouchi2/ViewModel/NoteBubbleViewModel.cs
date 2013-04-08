using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;
using PopnTouchi2.Infrastructure;

namespace PopnTouchi2
{
    /// <summary>
    /// Binds NoteBubble's properties to the View.
    /// </summary>
    public class NoteBubbleViewModel : PopnTouchi2.Infrastructure.ViewModelBase
    {
        /// <summary>
        /// Parameter.
        /// NoteBubble element from the Model.
        /// </summary>
        private NoteBubble noteBubble;

        /// <summary>
        /// Parameter.
        /// True if the center of the Bubble is located on the stave.
        /// </summary>
        private bool isOnStave;

        /// <summary>
        /// Parameter.
        /// Event triggered when a NoteBubble is dropped on the stave.
        /// </summary>
        public event EventHandler dropBubble;

        /// <summary>
        /// NoteBubbleViewModel Constructor.
        /// TODO
        /// </summary>
        public NoteBubbleViewModel()
        {
        }

        /// <summary>
        /// Property.
        /// TODO
        /// </summary>
        public NoteBubble Note
        {
            get
            {
                return noteBubble;
            }
            set
            {
                noteBubble = value;
                NotifyPropertyChanged("Note");
            }
        }

        /// <summary>
        /// Property.
        /// TODO
        /// </summary>
        public bool IsOnStave
        {
            get
            {
                return isOnStave;
            }
            set
            {
                isOnStave = value;
                NotifyPropertyChanged("IsOnStave");
            }
        }
    }
}
