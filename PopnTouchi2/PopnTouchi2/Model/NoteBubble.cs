using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Model.Enums;
using Microsoft.Surface.Presentation.Controls;
using System.Windows;
using System.Windows.Media;

namespace PopnTouchi2
{
    public class NoteBubble : ScatterViewItem
    {

        public NoteBubble(NoteValue noteValue)
        {
            Note = new Note(0, noteValue, Pitch.A, -1);
            Id = GlobalVariables.idNoteBubble++;

            CanScale = false;
            HorizontalAlignment = HorizontalAlignment.Center;
            CanRotate = false;
            HorizontalAlignment = HorizontalAlignment.Center;

        }

        public NoteBubble(NoteValue noteValue, Theme theme): this(noteValue)
        {
            this.Content = theme.getNoteBubbleImage(noteValue);
        }

        public Note Note { get; set; }
        public int Id { get; set; }
    }
}
