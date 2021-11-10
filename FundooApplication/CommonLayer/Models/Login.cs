using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public bool Any(Func<object, bool> p)
        {
            throw new NotImplementedException();
        }
    }
}
