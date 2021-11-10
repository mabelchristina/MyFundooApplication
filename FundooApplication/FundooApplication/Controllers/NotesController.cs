using BusinessManager.Interfaces;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
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
    public class NotesController : ControllerBase
    {
        private INotesBL userDataAccess;
        public NotesController(INotesBL userDataAccess)
        {
            this.userDataAccess = userDataAccess;
        }
        [Authorize]
        [HttpGet]
        public ActionResult<List<User>> GetAllUserNotes()
        {
            Response httpResponse = new Response();
            try
            {
                List<Notes> usersData = this.userDataAccess.GetAllUsersNotes();
                return this.Ok(new { Success = true, Message = "Get request is successful", Data = usersData });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message });
            }
        }
        [Authorize]
        [HttpPost("AddNotes")]
        public ActionResult<Notes> AddNotes(Notes note)
        {
            try
            {
                Notes addnote = this.userDataAccess.AddNotes(note);
                return this.Ok(new { Success = true, Message = "User Note added successfully", Data = addnote });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message });
            }
        }
        [Authorize]
        [HttpPost("UpdateNotes")]
        public ActionResult<Notes> UpdateNotes(Notes note)
        {
            try
            {
                Notes addnote = this.userDataAccess.UpdateNote(note);
                return this.Ok(new { Success = true, Message = "User Note Updated successfully", Data = addnote });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message });
            }
        }
        [Authorize]
        [HttpPost("DeleteNotes")]
        public ActionResult<Notes> DeleteNotes(Notes note)
        {
            try
            {
                this.userDataAccess.DeleteNote(note);
                return this.Ok(new { Success = true, Message = "User Note deleted successfully" });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message });
            }
        }
    }

}
