using BusinessManager.Interfaces;
using CommonLayer.Models;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessManager.Services
{
    public class NotesBL : INotesBL
    {
        private INotesRL dataOperations;
        public NotesBL(INotesRL dataOperations)
        {
            this.dataOperations = dataOperations;
        }
        public Notes AddNotes(Notes note)
        {
            try
            {
                return dataOperations.AddNotes(note);
            }
            catch (Exception)
            {

                throw;
            }
        }


        List<Notes> INotesBL.GetAllUsersNotes()
        {
            try
            {
                return dataOperations.GetAllUsersNotes();
            }
            catch (Exception)
            {

                throw;
            }
        }

        Notes INotesBL.UpdateNote(Notes note)
        {
            try
            {
                return dataOperations.UpdateNote(note);
            }
            catch (Exception)
            {

                throw;
            }
        }

        void INotesBL.DeleteNote(Notes note)
        {
            try
            {
                dataOperations.DeleteNote(note);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
