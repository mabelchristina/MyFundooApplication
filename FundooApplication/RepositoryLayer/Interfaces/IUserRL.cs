using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRL
    {
        List<User> GetUsers();
         User UserLogin(Login login);
        public User UserRegister(User user);
        public User UserForgotPassword(string FirstName, string Email);
        public User UserResetPassword(string Email, string CurrentPassword, string NewPassword);
    }
}
