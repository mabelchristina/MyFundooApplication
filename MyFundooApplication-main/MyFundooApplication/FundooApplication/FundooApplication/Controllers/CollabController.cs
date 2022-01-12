using BusinessManager.Interfaces;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundooApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollabController : ControllerBase
    {
        private readonly ICollabBL collaborationBL;
        public CollabController(ICollabBL collaborationBL)
        {
            this.collaborationBL = collaborationBL;
        }
        [HttpPost("noteId")]
        public IActionResult AddCollaboration(int noteId, GetCollab collaborationModel)
        {
            try
            {
                var Userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(Userid.Value);
                GetCollab getCollaborations = new GetCollab();
                getCollaborations = collaborationBL.AddCollaboration(UserId, noteId, collaborationModel);
                if (getCollaborations != null)
                {
                    return Ok(new { success = true, Message = "Collaboration Added Successful", Collaboration = getCollaborations });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "Collaboration Insertion Failed" });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        [HttpDelete("noteId")]
        public IActionResult RemoveCollab(int noteId, Collab collaborationModel)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userid.Value);
                bool result = collaborationBL.RemoveCollab(UserId, noteId, collaborationModel);
                if (result == true)
                {
                    return Ok(new { success = true, Message = "Collaboration Deletion Successful" });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "Collaboration Deletion Failed" });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        [HttpGet("noteId")]
        public IActionResult GetCollab(int noteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userid.Value);
                List<GetCollab> getCollaborations = new List<GetCollab>();
                getCollaborations = collaborationBL.GetCollab(UserId, noteId);
                if (getCollaborations != null)
                {
                    return Ok(new { success = true, Message = "Collaboration fetched Successfully", Collaboration = getCollaborations });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "Collaboration fetched Failed" });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }


    }
}
