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
    [Route("Label")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelBL noteLabelBL;
        public LabelController(ILabelBL noteLabelBL)
        {
            this.noteLabelBL = noteLabelBL;
        }
        [HttpGet]
        public ActionResult<List<User>> GetAllUserNotes()
        {
            Response httpResponse = new Response();
            try
            {
                List<NoteLabel> usersData = this.noteLabelBL.GetAllLabel();
                return this.Ok(new { Success = true, Message = "Get request is successful", Data = usersData });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message });
            }
        }

        [HttpPost]
        public ActionResult<NoteLabel> AddLabel(NoteLabel labelModel)
        {
            try
            {
                //var Userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                //int UserId = Int32.Parse(Userid.Value);
                NoteLabel addlabel = noteLabelBL.AddLabel(labelModel);
                return this.Ok(new { Success = true, Message = "Label added successfully", Data = addlabel });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message });
            }
        }
        [HttpPut]
        public ActionResult<NoteLabel> UpdateLabel(NoteLabel labelModel)
        {
            try
            {
                //var Userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                //int UserId = Int32.Parse(Userid.Value);
                NoteLabel addlabel = noteLabelBL.UpdateLabel(labelModel);
                return this.Ok(new { Success = true, Message = "Label added successfully", Data = addlabel });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message });
            }
        }
        [HttpDelete]
        public IActionResult DeleteLabel(NoteLabel labelModel)
        {
            try
            {
                //var Userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                //int UserId = Int32.Parse(Userid.Value);
                noteLabelBL.DeleteLabel(labelModel);
                return this.Ok(new { Success = true, Message = "Label added successfully"});
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message });
            }
        }
    }
}
