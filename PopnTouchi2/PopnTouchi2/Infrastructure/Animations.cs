using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Model.Enums;
using Microsoft.Surface.Presentation.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Timers;
using System.Windows.Threading;

namespace PopnTouchi2.Infrastructure
{
    public class Animations
    {
        /// <summary>
        /// Animation 
        /// Move the NoteBubble randomly on the screen
        /// </summary>
        public static void RandomMoveBubble()
        {
        
        }

        /// <summary>
        /// Animation
        /// Move the bubble on the right place in the stave
        /// </summary>
        public static void BubblePlacementOnStave(ScatterViewItem bubble, Point bubbleCenter, NoteBubble note)
        {
            Storyboard stb = new Storyboard();
            PointAnimation moveCenter = new PointAnimation();

            moveCenter.From = bubble.ActualCenter;
            moveCenter.To = bubbleCenter;
            moveCenter.Duration = new Duration(TimeSpan.FromSeconds(0.15));
            bubble.Center = bubbleCenter;
            moveCenter.FillBehavior = FillBehavior.Stop;

            stb.Children.Add(moveCenter);

            Storyboard.SetTarget(moveCenter, bubble);
            Storyboard.SetTargetProperty(moveCenter, new PropertyPath(ScatterViewItem.CenterProperty));

            stb.Begin(note);
        }
    }
}
