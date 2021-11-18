using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessManager.Interfaces
{
    public interface ILabelBL
    {
        public List<NoteLabel> GetAllLabel();
        public NoteLabel AddLabel(NoteLabel note);
    }
}
