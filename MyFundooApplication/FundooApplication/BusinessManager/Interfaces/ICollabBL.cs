using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessManager.Interfaces
{
    public interface ICollabBL
    {
        public GetCollab AddCollaboration(int userId, int noteId, GetCollab collaborationModel);
        public List<GetCollab> GetCollab(int userId, int noteId);
        public bool RemoveCollab(int userId, int noteId, Collab collaborationModel);
    }
}
