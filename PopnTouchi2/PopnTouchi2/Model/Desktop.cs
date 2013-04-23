using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Model.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Surface.Presentation.Controls;

namespace PopnTouchi2
{
    public class Desktop : Grid
    {
        public ScatterView Photos { get; set; }
        public SurfaceButton CreateSession { get; set; }
        public Builder sessionBuilder { get; set; }


        public Desktop()
        {

            CreateSession = new SurfaceButton();
            CreateSession.Width = 200;
            CreateSession.Height = 75;
            CreateSession.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CreateSession.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            CreateSession.Content = "Generate new Session !!!";
            CreateSession.Click += new RoutedEventHandler(CreateSession_Click);
            Children.Add(CreateSession);

            Photos = new ScatterView();
            Children.Add(Photos);

            sessionBuilder = new Builder();
            Children.Add(sessionBuilder.GenerateSession());
        }

        void CreateSession_Click(object sender, RoutedEventArgs e)
        {
            Children.Add(sessionBuilder.GenerateSession());
        }
    }
}
