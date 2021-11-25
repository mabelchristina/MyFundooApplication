using CommonLayer.Models;
using Experimental.System.Messaging;
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
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class UserRL : IUserRL
    {
        private readonly string _connectionString;

        public UserRL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("defaultConnection");
        }
        public static string EncryptPassword(string password)
        {
            try
            {
                byte[] encryptData = new byte[password.Length];
                encryptData = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encryptData);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
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
            catch (Exception)
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
                    command.Parameters.AddWithValue("@Password",  (user.Password));
                    var result = command.ExecuteNonQuery();
                    connection.Close();
                    if (result != 0)
                    {

                        Console.WriteLine("successfully registered" + user.FirstName + user.LastName + user.City
                            + user.City + user.MobileNumber + user.Email + user.Password);

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

        public string UserLogin(Login login)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            User user = new User();
            try
            {
                using (connection)
                {
                    ////Creating a stored Procedure for login Users into database
                        connection.Open();
                        SqlCommand cmd = new SqlCommand("spLogin", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Email", login.Email);
                        cmd.Parameters.AddWithValue("@Password", login.Password);
                        var result = cmd.ExecuteNonQuery();;
                        SqlDataReader rd = cmd.ExecuteReader();
                        if (rd.Read())
                        {
                            user.UserId = rd["UserId"] == DBNull.Value ? default : rd.GetInt32("UserId");
                            user.FirstName = rd["FirstName"] == DBNull.Value ? default : rd.GetString("FirstName");
                            user.LastName = rd["LastName"] == DBNull.Value ? default : rd.GetString("LastName");
                            user.Email = rd["Email"] == DBNull.Value ? default : rd.GetString("Email");
                            user.Password = EncryptPassword( rd["Password"] == DBNull.Value ? default : rd.GetString("Password"));
                        }
                        return GenerateJWTToken(user.Email);
                        ////return user;
                    }
                }
            catch (Exception)
            {
                throw;
            }
        }
   
        private static string GenerateJWTToken(string email)
        {
            ////generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("This is my test private key");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("email", email), 
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public bool ForgotPassword(string email)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    //Creating a stored Procedure for forgetpassword Users into database
                    connection.Open();
                    SqlCommand com = new SqlCommand("spUserForgotPassword", connection);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@email", email);
                    var result = com.ExecuteNonQuery();
           
                    MessageQueue queue;
                    //ADD MESSAGE TO QUEUE
                    if (MessageQueue.Exists(@".\Private$\FundooQueue"))
                    {
                        queue = new MessageQueue(@".\Private$\FundooQueue");
                    }
                    else
                    {
                        queue = MessageQueue.Create(@".\Private$\FundooQueue");
                    }

                    Message MyMessage = new Message();
                    MyMessage.Formatter = new BinaryMessageFormatter();
                    MyMessage.Body = GenerateJWTToken(email);
                    MyMessage.Label = "Forget Password Email";
                    queue.Send(MyMessage);
                    Message msg = queue.Receive();
                    msg.Formatter = new BinaryMessageFormatter();
                    MSMQEmail.SendEmail(email, msg.Body.ToString());
                    queue.ReceiveCompleted += new ReceiveCompletedEventHandler(msmqQueueReceive);

                    queue.BeginReceive();
                    queue.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
        private void msmqQueueReceive(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                MessageQueue queue = (MessageQueue)sender;
                Message msg = queue.EndReceive(e.AsyncResult);
                MSMQEmail.SendEmail(e.Message.ToString(), GenerateToken(e.Message.ToString()));
                queue.BeginReceive();
            }
            catch (MessageQueueException ex)
            {
                if (ex.MessageQueueErrorCode ==
                    MessageQueueErrorCode.AccessDenied)
                {
                    Console.WriteLine("Access is denied. " +
                        "Queue might be a system queue.");
                }
                // Handle other sources of MessageQueueException.
            }
        }
        public User ResetPassword(string email, ResetPassword resetPasswordModel)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    //Creating a stored Procedure for change password of User into database
                    connection.Open();
                    SqlCommand com = new SqlCommand("spResetPassword", connection);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@email", email);
                    com.Parameters.AddWithValue("@Newpassword", resetPasswordModel.NewPassword);
                    var result = com.ExecuteNonQuery();
                    User user = new User();
                    SqlDataReader rd = com.ExecuteReader();
                    if (rd.Read())
                    {
                        user.UserId = rd["UserId"] == DBNull.Value ? default : rd.GetInt32("UserId");
                        user.FirstName = rd["FirstName"] == DBNull.Value ? default : rd.GetString("FirstName");
                        user.LastName = rd["LastName"] == DBNull.Value ? default : rd.GetString("LastName");
                        user.Email = rd["Email"] == DBNull.Value ? default : rd.GetString("Email");
                        user.Password = EncryptPassword(rd["Password"] == DBNull.Value ? default : rd.GetString("Password"));

                    }
                    if (result > 0)
                    {
                        Console.WriteLine("Record Successfully Updated On Table");
                        return user;
                    }
                    else
                    {
                        Console.WriteLine("No records effected!");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
 
        public void SendToQueue(string url)
        {
            try
            {
                MessageQueue msgQueue;
                if (MessageQueue.Exists(@".\Private$\MyQueue"))
                {
                    msgQueue = new MessageQueue(@".\Private$\MyQueue");
                    msgQueue.Authenticate = true;
                }
                else
                {
                    msgQueue = MessageQueue.Create(@".\Private$\MyQueue");
                    msgQueue.Authenticate = true;
                }

                Message message = new Message();
                message.Formatter = new BinaryMessageFormatter();
                message.Body = url;
                msgQueue.Label = "Url Link";
                msgQueue.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
  
        public string ReceiveQueue()
        {
            try
            {
                var receiveQueue = new MessageQueue(@".\Private$\MyQueue");
                var receiveMsg = receiveQueue.Receive();
                receiveMsg.Formatter = new BinaryMessageFormatter();
                return receiveMsg.Body.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SendMail(string email)
        {
            try
            {
                string url = this.ReceiveQueue();
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("testcode1176@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Reset your password";
                mail.Body = $"Click this link to reset your password\n";
                smtpServer.Port = 587;
                smtpServer.Credentials = new NetworkCredential("testcode1176@gmail.com", "Test@123");
                smtpServer.EnableSsl = true;
                smtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        //GENERATE TOKEN WITH EMAIL
        public string GenerateToken(string email)
        {
            if (email == null)
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("email",email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
