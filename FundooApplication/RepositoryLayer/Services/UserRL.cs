using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
                        user.Password = (string)row["Password"];
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
        public async Task<string> UserForgotPassword(ForgotPassword forgotPassword)
        {
            try
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                DataSet dataSet = new DataSet();
                User user = new User();
                string sp = "spUserForgotPassword";
                using (connection)
                {
                    using (SqlCommand cmd = new SqlCommand(sp, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FirstName", forgotPassword.Firstname);
                        cmd.Parameters.AddWithValue("@Email", forgotPassword.Email);
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataSet, "UserInfo");
                        }
                    }
                    connection.Close();
                }
                if (forgotPassword.Email != null)
                {
                    ////here we create object of MsmqTokenSender which is present in Common-Layer
                    MsmqTokenSender msmq = new MsmqTokenSender();
                    string key = "This is my SecretKey which is used for security purpose";

                    ////Here generate encrypted key and result store in security key
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

                    //// here using securitykey and algorithm(security) the creadintails is generate(SigningCredentials present in Token)
                    var creadintials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var claims = new[]
                    {
                    new Claim("Email", forgotPassword.Email),
                };

                    var token = new JwtSecurityToken("Security token", "https://Test.com",
                        claims,
                        DateTime.UtcNow,
                        expires: DateTime.Now.AddDays(1),
                        signingCredentials: creadintials);

                    var NewToken = new JwtSecurityTokenHandler().WriteToken(token);

                    //// Send the email and password to Method in MsmqTokenSender
                    msmq.SendMsmqToken(forgotPassword.Email, NewToken.ToString());
                    return NewToken;
                }
                else
                {
                    return "Invalid user";
                }
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
                //var token = new JwtSecurityToken(reset.token);

                ////// Claims the email from token
                //var Email = (token.Claims.First(c => c.Type == "Email").Value);
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
