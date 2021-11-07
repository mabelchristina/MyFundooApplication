using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessManager.Interfaces
{
    public interface IUserBL
    {
        List<User> GetAllUsers();
        User UserLogin(string Email, string password);
        public User UserRegister(User user);
        public ForgotPassword UserForgotPassword(ForgotPassword forgotPassword);
        public void UserResetPassword(string Email, string CurrentPassword, string NewPassword);
    }
}
