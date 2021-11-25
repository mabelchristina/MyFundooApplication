using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRL
    {
        public List<User> GetUsers();
        public string UserLogin(Login login);
        public User UserRegister(User user);

        ////public  Task<string> UserForgotPassword(ForgotPassword forgotPassword);
        // public void UserResetPassword(ResetPassword reset);
        // public bool ForgetPassword(string email);

        public User ResetPassword(string email, ResetPassword resetPasswordModel);
        public bool ForgotPassword(string email);
    }
}
