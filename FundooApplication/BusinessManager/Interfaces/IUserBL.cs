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
        public string UserLogin(Login login);
        public User UserRegister(User user);

        // public Task<string> UserForgotPassword(ForgotPassword forgotPassword);
        public User ResetPassword(ResetPassword resetPassword);
        public bool CheckUser(string email);
    }

}
