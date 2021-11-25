using BusinessManager.Interfaces;
using CommonLayer.Models;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessManager.Services
{
    public class LabelBL : ILabelBL
    {
        private readonly ILabelRL noteLabelRL;
        public LabelBL(ILabelRL noteLabelRL)
        {
            this.noteLabelRL = noteLabelRL;
        }

        NoteLabel ILabelBL.AddLabel(NoteLabel note)
        {
            try
            {
                return noteLabelRL.AddLabel(note);
            }
            catch (Exception)
            {
                throw;
            }
        }

        void ILabelBL.DeleteLabel(NoteLabel note)
        {
            try
            {
                this.noteLabelRL.DeleteLabel(note);
            }
            catch (Exception)
            {
                throw;
            }
        }

        List<NoteLabel> ILabelBL.GetAllLabel()
        {
            try
            {
                return noteLabelRL.GetAllLabel();
            }
            catch (Exception)
            {
                throw;
            }
        }

        NoteLabel ILabelBL.UpdateLabel(NoteLabel note)
        {
            try
            {
                return noteLabelRL.UpdateLabel(note);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
