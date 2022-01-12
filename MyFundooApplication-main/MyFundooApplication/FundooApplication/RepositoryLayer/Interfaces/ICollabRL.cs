using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface ICollabRL
    {

        public GetCollab AddCollaboration(int userId, int noteId, GetCollab collaborationModel);
        public List<GetCollab> GetCollab(int userId, int noteId);
        public bool RemoveCollab(int userId, int noteId, Collab collaborationModel);
    }
}
