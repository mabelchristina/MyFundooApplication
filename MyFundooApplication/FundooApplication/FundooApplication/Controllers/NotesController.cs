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
    [Route("Notes")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private INotesBL userDataAccess;
        public NotesController(INotesBL userDataAccess)
        {
            this.userDataAccess = userDataAccess;
        }
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
        [HttpPost]
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
        
        [HttpPut]
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
        [HttpDelete]
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

        [HttpPut("Color")]
        public IActionResult ChangeColor(int NotesId, string color)
        {
            try
            {
                var result = userDataAccess.ChangeColor(NotesId, color);
                if (result == true)
                {
                    return Ok(new { success = true, Message = "NoteColor Updated Successful", NoteDetails = result });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "NoteColor Updation Failed" });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        [HttpPut("Archive")]
        public IActionResult Archive(int Notesid)
        {
            //var UserValidilty = HttpContext.User;
            //int UserId = Convert.ToInt32(UserValidilty.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            var status = userDataAccess.Archive(Notesid);
            if (status == true)
            {
                return Ok(new { success = true, Message = "Note successfully archived", Notes = status });
            }
            else
            {
                return BadRequest(new { success = false, Message = "Unable to archive note" });
            }


        }
        [HttpPut("Pin")]
        public IActionResult Pin(int Notesid)
        {
            //var UserValidilty = HttpContext.User;
            //int UserId = Convert.ToInt32(UserValidilty.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            var status = userDataAccess.Pin(Notesid);
            if (status == true)
            {
                return Ok(new { success = true, Message = "Note successfully pinned", Notes = status });
            }
            else
            {
                return BadRequest(new { success = false, Message = "Unable to pin note" });
            }
        }
        [HttpDelete("Trash")]
        public ActionResult<Notes> Trash(Notes note)
        {
            try
            {
                this.userDataAccess.Trash(note);
                return this.Ok(new { Success = true, Message = "User Note deleted successfully" });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message });
            }
        }
    }

}
