﻿using BusinessManager.Interfaces;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FundooApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserBL userDataAccess;
        //private readonly IAuthenticationManager jWTAuthenticationManager;

        public UserController(IUserBL userDataAccess)
        {
            this.userDataAccess = userDataAccess;
           // this.jWTAuthenticationManager = jWTAuthenticationManager;
            
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
        
        [HttpPost("Login")]
        public ActionResult<string> UserLogin(Login login)
        {
            try
            {
                var token = this.userDataAccess.UserLogin(login);
               //var token = jWTAuthenticationManager.Authenticate(userData);
              if (token == null)
                    return Unauthorized();
                return this.Ok(new { Success = true, Message = "User logged in successful", token });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message });
            }
        }
        //[Authorize]
        //[Route("ForgotPassword")]
        //[HttpPost]
        // public IActionResult UserForgotPassword(ForgotPassword forgotPassword)
        //{
        //    try
        //    {

        //        var result = userDataAccess.UserForgotPassword(forgotPassword);
        //        return this.Ok(new { success = true, Message = "password reset link has been sent to your email id", email = forgotPassword.Email });

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(new { success = false, Message = "email id don't exist" });
        //    }
        //}

        [AllowAnonymous]
        [HttpPost("ForgetPassword")]
        public IActionResult UserForgotPassword(ForgotPassword forgot)
        {
            try
            {
                //Send user data to manager
                bool result = this.userDataAccess.CheckUser(forgot.Email);
                if (result == true)
                {
                    return this.Ok(new { Status = true, Message = "Please check your email", Email = forgot.Email });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Email not Sent" });
                }
            }
            catch (Exception ex)
            {
                return this.NotFound(new  { Status = false, Message = ex.Message });
            }
        }
        [AllowAnonymous]
        [Route("ResetPassword")]
        [HttpPost]
        public ActionResult<User> UserResetPassword(ResetPassword reset)
        {

            try
                {
                    var identity = User.Identity as ClaimsIdentity;
                    if (identity != null)
                        {
                    IEnumerable<Claim> claims = identity.Claims;
                    var Email = claims.Where(p => p.Type == @"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").FirstOrDefault()?.Value;
                    reset.Email = Email;
                    User result = this.userDataAccess.ResetPassword(reset);
                    if (result == null)
                    {
                        return this.Ok(new { success = true, message = "Password Reset Successful", User = result });
                    }
                }
                return this.Ok(new { success = false, message = "Password Reset UnSuccessful" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
    }
}
