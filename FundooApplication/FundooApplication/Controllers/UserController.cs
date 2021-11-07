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
    public class UserController : ControllerBase
    {
        private IUserBL userDataAccess;
        public UserController(IUserBL userDataAccess)
        {
            this.userDataAccess = userDataAccess;
        }
        [HttpGet]
        public ActionResult<List<User>> GetAllUsers()
        {
            Response httpResponse = new Response();
            try
            {
                List<User> usersData = this.userDataAccess.GetAllUsers();
                return this.Ok(new { Success = true, Message = "Get request is successful", Data = usersData });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message });
            }
        }


        [Route("login")]
        [HttpPost("{UserId}")]
         public ActionResult<User> UserLogin(string Email, string password)
        {
            try
            {
                User userData = this.userDataAccess.UserLogin(Email, password);
                return this.Ok(new { Success = true, Message = "Get request is successful", Data = userData });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message });
            }
        }
        [Route("Register")]
        [HttpPost("{UserId}")]
        public ActionResult<User> UserRegister(User user)
        {
            try
            {
                User userData = this.userDataAccess.UserRegister(user);
                return this.Ok(new { Success = true, Message = "Get request is successful", Data = userData });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message });
            }
        }
        [Route("ForgotPassword")]
        [HttpPost]
        public ActionResult<User> UserForgotPassword(string Firstname, string Email)
        {
            try
            {
                User userData = this.userDataAccess.UserLogin(Firstname, Email);
                return this.Ok(new { Success = true, Message = "Get request is successful", Data = userData });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message });
            }
        }

        [Route("ResetPassword")]
        [HttpPost]
        public ActionResult<User> UserResetPassword(string Email, string CurrentPassword, string NewPassword)
        {
            try
            {
                User userData = this.userDataAccess.UserLogin(Email,CurrentPassword);
                return this.Ok(new { Success = true, Message = "Get request is successful", Data = userData });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message });
            }
        }
    }
}
