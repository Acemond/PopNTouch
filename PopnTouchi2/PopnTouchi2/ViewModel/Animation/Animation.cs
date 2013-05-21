using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Microsoft.Surface.Presentation.Controls;

namespace PopnTouchi2.ViewModel.Animation
{
    public abstract class Animation
    {
        /// <summary>
        /// Property.
        /// Storyboard animation.
        /// </summary>
        public Storyboard Storyboard { get; set; }
        /// <summary>
        /// Property.
        /// DispatcherTimer.
        /// </summary>
        public DispatcherTimer DispatcherTimer { get; set; }

        /// <summary>
        /// Property.
        /// The ScatterViewItem animated.
        /// </summary>
        public ScatterViewItem SVItem { get; set; }

        /// <summary>
        /// Property.
        /// The ScatterView containing the ScatterViewItem currently animated.
        /// </summary>
        public ScatterView ParentSV { get; set; }

        /// <summary>
        /// Parameter.
        /// Boolean to stop or continue current animation.
        /// </summary>
        public bool canAnimate;

        /// <summary>
        /// Global Animation Constructor.
        /// </summary>
        public Animation()
        {
            Storyboard = new Storyboard();
            DispatcherTimer = new DispatcherTimer();
        }
    }
}
