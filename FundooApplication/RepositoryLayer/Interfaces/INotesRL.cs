using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface INotesRL
    {
        public List<Notes> GetAllUsersNotes();
        public Notes AddNotes(Notes note);
        public Notes UpdateNote(Notes note);
        public void DeleteNote(Notes note);
    }
}
