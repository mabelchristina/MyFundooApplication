using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRL
    {
        public List<User> GetUsers();
         User UserLogin(Login login);
        public User UserRegister(User user);
        public User UserForgotPassword(string FirstName, string Email);
        public void UserResetPassword(ResetPassword reset);
    }
}
