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
        public User UserLogin(Login login);
        public User UserRegister(User user);
        Task<string> UserForgotPassword(ForgotPassword forgotPassword);
        public void UserResetPassword(ResetPassword reset);
    }
}
