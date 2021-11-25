using BusinessManager.Interfaces;
using CommonLayer.Models;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessManager.Services
{
    public class CollabBL : ICollabBL
    {
        private readonly ICollabRL collabRL;
        public CollabBL(ICollabRL collabRL)
        {
            this.collabRL = collabRL;
        }
        GetCollab ICollabBL.AddCollaboration(int userId, int noteId, GetCollab collaborationModel)
        {
            try
            {
                return collabRL.AddCollaboration(userId, noteId, collaborationModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        List<GetCollab> ICollabBL.GetCollab(int userId, int noteId)
        {
            try
            {
                return collabRL.GetCollab(userId, noteId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        bool ICollabBL.RemoveCollab(int userId, int noteId, Collab collaborationModel)
        {
            try
            {
                return collabRL.RemoveCollab(userId, noteId,collaborationModel);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
