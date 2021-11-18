using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
   public  class NoteLabel
    {
   
        public int labelId { get; set; }
      
        public string labelName { get; set; }
 
        public int UserId { get; set; }
     
        public int noteId { get; set; }
        
        public DateTime registeredDate { get; set; }
        
        public DateTime modifiedDate { get; set; }
    }
}
