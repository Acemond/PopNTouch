using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using Microsoft.Xna.Framework.Audio;

namespace PopnTouchi2
{
    /// <summary>
    /// Interaction logic for DesktopWindow.xaml
    /// </summary>
    public partial class DesktopWindow : SurfaceWindow
    {
        public DesktopWindow()
        {
            InitializeComponent();

            Builder sessionBuilder = new Builder();
            Session newSession = sessionBuilder.generateSession();
      //      desktop.Children.Add(newSession);
        }

        private void SinglePlayer(object sender, TouchEventArgs e)
        {
            SinglePlayerWindow singleWindows = new SinglePlayerWindow();
            Application.Current.Windows[0].Hide();
            singleWindows.Show();
        }

        private void MultiplePlayer(object sender, TouchEventArgs e)
        {

        }
    }
}
