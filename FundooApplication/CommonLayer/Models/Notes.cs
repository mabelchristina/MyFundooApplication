using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    public class Notes
    {
        public int NotesId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Reminder { get; set; }
        public int UserId { get; set; }
    }
}
