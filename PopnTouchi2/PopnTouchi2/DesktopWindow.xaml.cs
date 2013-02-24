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

namespace PopnTouchi2
{
    /// <summary>
    /// Interaction logic for DesktopWindow.xaml
    /// </summary>
    public partial class DesktopWindow : Window
    {
        //Notes pour faire les testes de sons
        Note n = new Note(6, NoteValue.quaver, Pitch.A, 0);
        Note n1 = new Note(6, NoteValue.minim, Pitch.B, 0);
        Note n2 = new Note(6, NoteValue.crotchet, Pitch.C, 0);
        Note n3 = new Note(6, NoteValue.minim, Pitch.D, 2);
        Note n4 = new Note(6, NoteValue.quaver, Pitch.E, 3);
        Instrument i = new Instrument(InstrumentType.piano);
        Instrument i2 = new Instrument(InstrumentType.piano);
        List<Instrument> listinstr = new List<Instrument>();
        Stave stave;
        //A virer

        public DesktopWindow()
        {
            InitializeComponent();

            //Création d'une portée pour tester le son
            listinstr.Add(i2);
            listinstr.Add(i);
            stave = new Stave(listinstr);
            stave.addNote(n);
            stave.addNote(n1);
            stave.addNote(n2);
            stave.addNote(n3);
            stave.addNote(n4);
            //Fin création -> A virer
        }

        //*********** Méthodes pour tester le son ***********
        private void playSon(object sender, RoutedEventArgs e)
        {
            i.playNote(n3);
        }

        private void playSon2(object sender, RoutedEventArgs e)
        {
            stave.playAllNotes();
        }

        private void stopSon(object sender, RoutedEventArgs e)
        {
            stave.stopMusic();
        }
        //A virer !****************
    }
}
