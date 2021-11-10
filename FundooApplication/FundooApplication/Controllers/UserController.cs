using BusinessManager.Interfaces;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Interfaces;
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
        private readonly IAuthenticationManager jWTAuthenticationManager;

        public UserController(IUserBL userDataAccess, IAuthenticationManager jWTAuthenticationManager)
        {
            this.userDataAccess = userDataAccess;
            this.jWTAuthenticationManager = jWTAuthenticationManager;
            
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

       [Authorize]
        [Route("Register")]
        [HttpPost]
        public ActionResult<User> UserRegister(User user)
        {
            if (user == null)
            {
                return BadRequest("User is null.");
            }
            try
            {
                User userData = this.userDataAccess.UserRegister(user);
                return this.Ok(new { Success = true, Message = "New User registration is successful", Data = userData });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message });
            }
        }
        [Authorize]
        [HttpPost("Login")]
        public ActionResult<User> UserLogin(Login login)
        {
            try
            {
                User userData = this.userDataAccess.UserLogin(login);
                var token = jWTAuthenticationManager.Authenticate(login.Email, login.Password);
                if (token == null)
                    return Unauthorized();
                return this.Ok(new { Success = true, Message = "User logged in successful", Data = userData, token });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message });
            }
        }
        [Authorize]
        [Route("ForgotPassword")]
        [HttpPost]
        public ActionResult<User> UserForgotPassword(string FirstName, string Email)
        {
            try
            {
                User userData = this.userDataAccess.UserForgotPassword(FirstName, Email);
                return this.Ok(new { Success = true, Message = "Get request is successful", Data = userData });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message });
            }
        }
        [Authorize]
        [Route("ResetPassword")]
        [HttpPost]
        public ActionResult<User> UserResetPassword(ResetPassword reset)
        {
            try
            {
                 userDataAccess.UserResetPassword(reset);
                return this.Ok(new { Success = true, Message = "Password reset is successful" });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message });
            }
        }
    }
}
