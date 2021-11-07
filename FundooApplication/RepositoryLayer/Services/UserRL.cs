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
                    // result = JsonConvert.SerializeObject(dataSet.Tables["UserInfo"]);

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

        public User UserLogin(string Email, string Password)
        {
            try
            {
                DataSet dataSet = new DataSet();
                User user = new User();
                string storedProcedure = "spLogin";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(storedProcedure, connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@Password", Password);

                    SqlParameter outputPara = new SqlParameter();
                    SqlDataAdapter adapter = new SqlDataAdapter(storedProcedure, connection);
                    adapter.Fill(dataSet, "UserInfo");
                    connection.Open();

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
                    //var result = JsonConvert.SerializeObject(dataSet.Tables["UserInfo"]);

                    connection.Close();
                }
                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ForgotPassword UserForgotPassword(ForgotPassword forgotPassword)
        {
            try
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                DataSet dataSet = new DataSet();
                using (connection)
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("spUserForgotPassword", connection);
                    adapter.Fill(dataSet, "UserInfo");
                    foreach (DataRow dataRow in dataSet.Tables["UserInfo"].Rows)
                    {
                        Console.WriteLine("\t" + dataRow["Firstname"] + "  " + dataRow["Email"] + " ");
                    }
                    connection.Close();
                }
                return forgotPassword;
                
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public void UserResetPassword(string Email, string CurrentPassword, string NewPassword)
        {
            try
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                DataSet dataSet = new DataSet();
                using (connection)
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("spUserResetPassword", connection);
                    adapter.Fill(dataSet, "UserInfo");
                    foreach (DataRow dataRow in dataSet.Tables["UserInfo"].Rows)
                    {
                        Console.WriteLine("\t" + dataRow["Email"] + "  " + dataRow["CurrentPassword"] + " " + dataRow["NewPassword"] + " ");
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
