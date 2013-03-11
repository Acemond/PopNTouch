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

namespace PopnTouchi2
{
    /// <summary>
    /// Interaction logic for DesktopWindow.xaml
    /// </summary>
    public partial class DesktopWindow : Window
    {
        //Notes pour faire les testes de sons
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
            Note n = new Note(6, NoteValue.crotchet, Pitch.A, 0);
            Note n2 = new Note(6, NoteValue.crotchet, Pitch.E, 2);
            Note n3 = new Note(6, NoteValue.crotchet, Pitch.E, 1);
                
                stave.addNote(n,0);
                stave.addNote(n2,2);
                stave.addNote(n3, 1);
            
            String s = "";
            for (int j = 0; j < stave.Notes.Count; j++)
            {
                s += stave.Notes[j].Position;
            }
            MessageBox.Show(s);
            //Fin création -> A virer
        }

        //*********** Méthodes pour tester le son ***********
        private void trier(object sender, RoutedEventArgs e)
        {
            stave.trier();
            String s = "";
            for (int j = 0; j < stave.Notes.Count; j++)
            {
                s += stave.Notes[j].Position;
            }
            MessageBox.Show(s);
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
