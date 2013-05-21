using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace PopnTouchi2.ViewModel.Animation
{
    public class StaveAnimation : Animation
    {
        /// <summary>
        /// Property.
        /// Stave's Image.
        /// </summary>
        private ImageBrush Image { get; set; }

        /// <summary>
        /// StaveAnimation Constructor
        /// </summary>
        /// <param name="s"></param>
        public StaveAnimation(Stave s) 
            : base()
        {
            if (s.isUp)
            {
                //Image =
            }
            else
            {
                //Image =
            }
        }

        /// <summary>
        /// Performs the glowing animation when playing the music
        /// </summary>
        public void Animate()
        {
        }
    }
}
