using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
 
        public string color { get; set; }

        public bool Trash { get; set; }

        public bool Archive { get; set; }


        public bool Pin { get; set; }

    }
}
