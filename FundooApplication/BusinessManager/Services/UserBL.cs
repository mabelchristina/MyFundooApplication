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

        public User UserLogin(Login login)
        {
            return dataOperations.UserLogin(login);
        }

        User IUserBL.UserForgotPassword(string FirstName, string Email)
        {
            return dataOperations.UserForgotPassword(FirstName, Email);
        }

        User IUserBL.UserRegister(User user)
        {
            return dataOperations.UserRegister(user);
        }

        User IUserBL.UserResetPassword(string Email, string CurrentPassword, string NewPassword)
        {
            return dataOperations.UserResetPassword(Email, CurrentPassword, NewPassword);
        }
    }
}
