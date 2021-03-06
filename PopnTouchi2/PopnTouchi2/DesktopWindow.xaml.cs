﻿using System;
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
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.IO;
using PopnTouchi2.ViewModel;

namespace PopnTouchi2
{
    /// <summary>
    /// Interaction logic for DesktopWindow.xaml
    /// </summary>
    public partial class DesktopWindow : SurfaceWindow
    {
        /// <summary>
        /// DesktopWindow Constructor.
        /// Initializes a new DesktopView.
        /// </summary>
        public DesktopWindow()
        {
            InitializeComponent();

            DesktopView Desktop = new DesktopView();
            this.AddChild(Desktop);
        }
    }
}
