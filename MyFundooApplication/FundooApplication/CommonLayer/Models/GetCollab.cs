using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.Models
{
    public class GetCollab
    {
        [Required]
        public int CollabId { get; set; }
        [Required]
        public string EmailID { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int NoteId { get; set; }
        [Required]
        public DateTime registeredDate { get; set; }
        [Required]
        public DateTime modifiedDate { get; set; }
    }
}
