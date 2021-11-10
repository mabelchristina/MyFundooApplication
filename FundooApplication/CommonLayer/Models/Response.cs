using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    public class Response
    {

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Status { set; get; }
        public string Message { set; get; }
    }
}
