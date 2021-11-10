using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryLayer.Services
{
    public class UserRL:IUserRL
    {
        private readonly string _connectionString;

        public UserRL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("defaultConnection");
        }
        public List<User> GetUsers()
        {
            try
            {
                DataSet dataSet = new DataSet();
                List<User> usersList = new List<User>();
                string storedProcedure = "spGetAllUsersInfo";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(storedProcedure, connection);
                    adapter.Fill(dataSet, "UserInfo");

                    foreach (DataRow row in dataSet.Tables["UserInfo"].Rows)
                    {
                        User user = new User();
                        user.UserId = (int)row["UserId"];
                        user.FirstName = (string)row["FirstName"];
                        user.LastName = (string)row["LastName"];
                        user.City = (string)row["City"];
                        user.MobileNumber = (string)row["MobileNumber"];
                        user.Email = (string)row["Email"];
                        usersList.Add(user);
                    }

                    connection.Close();
                }
                return usersList;
            }
            catch (Exception )
            {
                throw;
            }
        }
        public User UserRegister(User user)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("spRegister", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Firstname", user.FirstName);
                    command.Parameters.AddWithValue("@Lastname", user.LastName);
                    command.Parameters.AddWithValue("@City", user.City);
                    command.Parameters.AddWithValue("@MobileNumber", user.MobileNumber);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    var result = command.ExecuteNonQuery();
                    connection.Close();
                    if (result != 0)
                    {

                        Console.WriteLine("successfully registered" + user.FirstName+user.LastName+user.City
                            +user.City+user.MobileNumber+user.Email+user.Password);

                    }
                    return user;
                }
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public User UserLogin(Login login)
        {
            try
            {
                DataSet dataSet = new DataSet();
                User user = new User();
                string storedProcedure = "spLogin";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedure, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Email", login.Email);
                        cmd.Parameters.AddWithValue("@Password", login.Password);
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataSet, "UserInfo");
                        }
                    }

                    foreach (DataRow row in dataSet.Tables["UserInfo"].Rows)
                    {
                        user.UserId = (int)row["UserId"];
                        user.FirstName = (string)row["FirstName"];
                        user.LastName = (string)row["LastName"];
                        user.City = (string)row["City"];
                        user.MobileNumber = (string)row["MobileNumber"];
                        user.Email = (string)row["Email"];
                        user.Password = (string) row ["Password"];

                    }

                    connection.Close();
                }
                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public User UserForgotPassword(string FirstName,string Email)
        {
            try
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                DataSet dataSet = new DataSet();
                User user = new User();
                string sp = "spUserForgotPassword";
                using (connection)
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sp, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FirstName", FirstName);
                    cmd.Parameters.AddWithValue("@Email", Email);
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataSet, "UserInfo");
                    }
                    foreach (DataRow row in dataSet.Tables["UserInfo"].Rows)
                    {
                        user.UserId = (int)row["UserId"];
                        user.FirstName = (string)row["FirstName"];
                        user.LastName = (string)row["LastName"];
                        user.City = (string)row["City"];
                        user.MobileNumber = (string)row["MobileNumber"];
                        user.Email = (string)row["Email"];
                        user.Password = "************";
                    }
                    connection.Close();
                }
                return user;
                
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public void UserResetPassword(ResetPassword reset)
        {
            try
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                DataSet dataSet = new DataSet();
                User user = new User();
                string sp = "spUserResetPassword";
                using (connection)
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sp, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", reset.Email);
                    cmd.Parameters.AddWithValue("@CurrentPassword", reset.CurrentPassword);
                    cmd.Parameters.AddWithValue("@NewPassword",reset.NewPassword);
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataSet, "UserInfo");
                    }
                    connection.Close();
                }
               
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
