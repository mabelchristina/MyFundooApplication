using BusinessManager.Interfaces;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FundooApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserBL userDataAccess;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        public UserController(IMemoryCache memoryCache, IDistributedCache distributedCache, IUserBL userDataAccess)
        {
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this.userDataAccess = userDataAccess;
        }   

     
        [HttpGet]
        async Task<ActionResult<List<User>>> GetAllUsers()
        {
            Response httpResponse = new Response();
            try
            {
                List<User> usersData = await userDataAccess.GetAllUsers();
                return this.Ok(new { Success = true, Message = "Get request is successful", Data = usersData });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message });
            }
        }
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllCustomersUsingRedisCache()
        {
            var cacheKey = "customerList";
            string serializedCustomerList;
            var customerList = new List<User>();
            var redisCustomerList = await distributedCache.GetAsync(cacheKey);
            if (redisCustomerList != null)
            {
                serializedCustomerList = Encoding.UTF8.GetString(redisCustomerList);
                customerList = JsonConvert.DeserializeObject<List<User>>(serializedCustomerList);
            }
            else
            {
                customerList = await userDataAccess.GetAllUsers();
                serializedCustomerList = JsonConvert.SerializeObject(customerList);
                redisCustomerList = Encoding.UTF8.GetBytes(serializedCustomerList);
            }
            return Ok(customerList);
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

        [AllowAnonymous]
        [HttpPost("ForgetPassword")]
        public IActionResult UserForgotPassword(ForgotPassword forgot)
        {
            try
            {
                //Send user data to manager
                bool result = this.userDataAccess.ForgotPassword(forgot.Email);
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
                    
                    User result = this.userDataAccess.ResetPassword(Email,reset);
                    if (result == null)
                    {
                        return this.Ok(new { success = true, message = "Password Reset UnSuccessful", User = result });
                    }
                }
                return this.Ok(new { success = true, message = "Password Reset Successful" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
    }
}
