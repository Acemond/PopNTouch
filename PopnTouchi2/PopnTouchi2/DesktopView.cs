using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Surface.Presentation.Controls;
using PopnTouchi2.ViewModel;

namespace PopnTouchi2
{
    /// <summary>
    /// Grid contained by the SurfaceWindow.
    /// </summary>
    public class DesktopView : Grid
    {
        /// <summary>
        /// Property.
        /// The current SessionVM.
        /// </summary>
        public SessionViewModel SessionVM { get; set; }

        /// <summary>
        /// Property.
        /// TODO
        /// </summary>
        public ScatterView Photos { get; set; }

        public List<int> IDs { get; set; }

        /// <summary>
        /// temporary
        /// </summary>
        public SurfaceButton CreateSession_Button { get; set; }

        /// <summary>
        /// Initializes few components including the session.
        /// </summary>
        public DesktopView()
        {
            IDs = new List<int>();

            CreateSession_Button = new SurfaceButton();
            CreateSession_Button.Width = 200;
            CreateSession_Button.Height = 75;
            CreateSession_Button.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            CreateSession_Button.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            CreateSession_Button.Content = "Generate new Session !!!";
            CreateSession_Button.Click += new RoutedEventHandler(CreateSession_Button_Click);
            Children.Add(CreateSession_Button);

            Photos = new ScatterView();
            Children.Add(Photos);

            SessionVM = new SessionViewModel(new Session());
            Children.Add(SessionVM.Grid);
        }

        /// <summary>
        /// Event handling the Create Session Button Click Action.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CreateSession_Button_Click(object sender, RoutedEventArgs e)
        {
            SessionVM = new SessionViewModel(new Session(), IDs);
            Children.Add(SessionVM.Grid);
        }

    }
}
