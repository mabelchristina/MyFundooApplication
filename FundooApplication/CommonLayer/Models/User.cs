using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CommonLayer.Models
{
    public class User
    {
        public static ClaimsIdentity Identity;

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

}
