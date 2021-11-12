using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    public class ResetPassword
    {
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string token { get; set; }

    }
}
