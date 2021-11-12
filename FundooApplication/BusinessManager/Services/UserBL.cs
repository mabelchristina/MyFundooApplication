using BusinessManager.Interfaces;
using CommonLayer.Models;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        
         async Task<string> IUserBL.UserForgotPassword(ForgotPassword forgotPassword)
        {
            try
            {
                //// If checks forgotpassword model details is empty or not 
                if (forgotPassword != null)
                {
                    var result = await this.dataOperations.UserForgotPassword(forgotPassword);
                    return result;
                }
                else
                {
                    throw new Exception("User Email is not valid");
                }
            }
            catch (Exception exception)
            {
                throw exception;
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

        void IUserBL.UserResetPassword(ResetPassword reset)
        {
            try
            {
                this.dataOperations.UserResetPassword(reset);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
