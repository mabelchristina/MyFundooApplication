using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessManager.Interfaces
{
    public interface IUserBL
    {
        List<User> GetAllUsers();
        public User UserLogin(Login login);
        public User UserRegister(User user);
        public Task<string> UserForgotPassword(ForgotPassword forgotPassword);
        public void UserResetPassword(ResetPassword reset);
    }
}
