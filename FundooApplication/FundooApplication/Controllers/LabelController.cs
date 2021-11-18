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

        [HttpPost("Add")]
        public ActionResult<Notes> AddLabel(NoteLabel labelModel)
        {
            try
            {
                //var Userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                //int UserId = Int32.Parse(Userid.Value);
                NoteLabel addlabel= noteLabelBL.AddLabel(labelModel);
                return this.Ok(new { Success = true, Message = "Label added successfully", Data = addlabel });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message });
            }
        }
    }
}
