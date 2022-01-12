using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessManager.Interfaces
{
    public interface IUserBL
    {
       Task<List<User>> GetAllUsers();
        public string UserLogin(Login login);
        public User UserRegister(User user);

        // public Task<string> UserForgotPassword(ForgotPassword forgotPassword);
        public User ResetPassword(string email, ResetPassword resetPasswordModel);
        public bool ForgotPassword(string email);
    }

}
