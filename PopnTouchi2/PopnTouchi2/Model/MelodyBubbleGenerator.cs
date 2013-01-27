using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PopnTouchi2
{
    public class MelodyBubbleGenerator : ObservableCollection<String> , INotifyPropertyChanged
    {
        private ObservableCollection<String> Bubbles;
    }
}
