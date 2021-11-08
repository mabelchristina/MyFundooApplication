using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessManager.Interfaces
{
    public interface IUserBL
    {
        List<User> GetAllUsers();
        User UserLogin(Login login);
        public User UserRegister(User user);
        public User UserForgotPassword(string FirstName, string Email);
        public User UserResetPassword(string Email, string CurrentPassword, string NewPassword);
    }
}
