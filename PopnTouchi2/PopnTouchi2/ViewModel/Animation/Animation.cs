using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Microsoft.Surface.Presentation.Controls;

namespace PopnTouchi2.ViewModel.Animation
{
    abstract class Animation
    {
        /// <summary>
        /// Property.
        /// Storyboard animation.
        /// </summary>
        protected Storyboard Storyboard { get; set; }
        /// <summary>
        /// Property.
        /// DispatcherTimer.
        /// </summary>
        protected DispatcherTimer DispatcherTimer { get; set; }

        /// <summary>
        /// Property.
        /// The ScatterViewItem animated.
        /// </summary>
        protected ScatterViewItem SVItem { get; set; }

        /// <summary>
        /// Property.
        /// The ScatterView containing the ScatterViewItem currently animated.
        /// </summary>
        protected ScatterView ParentSV { get; set; }

        /// <summary>
        /// Parameter.
        /// Boolean to stop or continue current animation.
        /// </summary>
        protected bool canAnimate;

        /// <summary>
        /// Global Animation Constructor.
        /// </summary>
        protected Animation()
        {
            Storyboard = new Storyboard();
            DispatcherTimer = new DispatcherTimer();
        }

        /// <summary>
        /// Stops a current animation performing.
        /// </summary>
        protected void stopAnimation()
        {
            canAnimate = false;
            DispatcherTimer.Stop();
            Storyboard.Pause();
            SVItem.Center = SVItem.ActualCenter;
            SVItem.Orientation = SVItem.Orientation;
            Storyboard.Remove();
        }
    }
}
