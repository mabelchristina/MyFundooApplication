using BusinessManager.Interfaces;
using CommonLayer.Models;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessManager.Services
{
   public class UserBL:IUserBL
    {
        private IUserRL dataOperations;
        public UserBL(IUserRL dataOperations)
        {
            this.dataOperations = dataOperations;
        }
        public List<User> GetAllUsers()
        {
            try
            {
                return this.dataOperations.GetUsers();
            }
            catch(Exception )
            {
                throw;
            }
        }

        public User UserLogin(string Email, string Password)
        {
            return dataOperations.UserLogin(Email, Password);
        }

        ForgotPassword IUserBL.UserForgotPassword(ForgotPassword forgotPassword)
        {
           return dataOperations.UserForgotPassword(forgotPassword);
        }

        User IUserBL.UserRegister(User user)
        {
            return dataOperations.UserRegister(user);
        }

        void IUserBL.UserResetPassword(string Email, string CurrentPassword, string NewPassword)
        {
            dataOperations.UserResetPassword(Email, CurrentPassword, NewPassword);
        }
    }
}
