using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface ILabelRL
    {
        public List<NoteLabel> GetAllLabel();
        public NoteLabel AddLabel(NoteLabel note);
    }
}
