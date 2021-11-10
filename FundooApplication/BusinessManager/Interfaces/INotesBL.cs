using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessManager.Interfaces
{
    public interface INotesBL
    {
        public List<Notes> GetAllUsersNotes();
        public Notes AddNotes(Notes note);
        public Notes UpdateNote(Notes note);
        public void DeleteNote(Notes note);
    }
}
