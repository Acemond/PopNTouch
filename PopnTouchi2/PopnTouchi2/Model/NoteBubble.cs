using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopnTouchi2.Model.Enums;

namespace PopnTouchi2
{
    /// <summary>
    /// Represents a bubble item containing an instance of Note.
    /// </summary>
    public class NoteBubble
    {
        /// <summary>
        /// Property.
        /// NoteBubble's contained Note
        /// </summary>
        public Note Note { get; set; }

        /// <summary>
        /// Property.
        /// NoteBubble unique ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// NoteBubble Constructor.
        /// Creates a new NoteBubble object and its Note.
        /// </summary>
        /// <param name="noteValue">The NoteValue needed to create a Note</param>
        public NoteBubble(NoteValue noteValue)
        {
            Note = new Note(1, noteValue, "do", -1);
            Id = GlobalVariables.idNoteBubble++;
        }

        /// <summary>
        /// NoteBubble Constructor.
        /// Creates a new NoteBubble object and its Note.
        /// </summary>
        /// <param name="note">The note</param>
        public NoteBubble(Note note)
        {
            Note = note;
            Id = GlobalVariables.idNoteBubble++;
        }

        
        /// <summary>
        /// NoteBubble Constructor.
        /// Creates a new NoteBubble object and its Note with its alteration
        /// </summary>
        /// <param name="sharp">sharp (#)</param>
        /// <param name="flat">flat (b)</param>
        public NoteBubble(bool sharp, bool flat)
        {
            Note = new Note(NoteValue.alteration);
            Note.Flat = flat;
            Note.Sharp = sharp;
            Id = GlobalVariables.idNoteBubble++;
        }
    }
}
