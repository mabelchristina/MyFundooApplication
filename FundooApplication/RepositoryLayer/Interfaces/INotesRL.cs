using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface INotesRL
    {
        public List<Notes> GetAllUsersNotes();
        public Notes AddNotes(Notes note);
        public Notes UpdateNote(Notes note);
        public void DeleteNote(Notes note);
        public bool Archive(int NotesId);
        public bool ChangeColor(int NotesId, string color);
        public bool Pin(int NotesId);
    }
}
