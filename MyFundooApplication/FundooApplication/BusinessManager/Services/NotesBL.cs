using BusinessManager.Interfaces;
using CommonLayer.Models;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

       bool INotesBL.Archive(int Notesid)
        {
            try
            {
                var result = this.dataOperations.Archive(Notesid);
                if (Notesid != 0)
                {
                    return result;
                }
                else
                {
                    throw new Exception("Note is not found select the correct note");
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        bool INotesBL.ChangeColor(int NotesId, string color)
        {
            try
            {
                var result = this.dataOperations.ChangeColor(NotesId, color);
                if (NotesId != 0)
                {
                    return result;
                }
                else
                {
                    throw new Exception("Note color cant be changed");
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        bool INotesBL.Pin(int NotesId)
        {
            try
            {
                var result = this.dataOperations.Pin(NotesId);
                if (NotesId != 0)
                {
                    return result;
                }
                else
                {
                    throw new Exception("Note cant be pinned");
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        void INotesBL.Trash(Notes note)
        {
            try
            {
                dataOperations.Trash(note);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
