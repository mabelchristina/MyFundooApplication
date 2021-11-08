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
            try
            {
                return dataOperations.UserLogin(login);
            }
            catch (Exception)
            {

                throw;
            }
        }

        User IUserBL.UserForgotPassword(string FirstName, string Email)
        {
            try
            {
                return dataOperations.UserForgotPassword(FirstName, Email);
            }
            catch (Exception)
            {

                throw;
            }
        }

        User IUserBL.UserRegister(User user)
        {
            try
            {
                return dataOperations.UserRegister(user);
            }
            catch (Exception)
            {

                throw;
            }
        }

        User IUserBL.UserResetPassword(string Email, string CurrentPassword, string NewPassword)
        {
            try
            {
                return dataOperations.UserResetPassword(Email, CurrentPassword, NewPassword);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
