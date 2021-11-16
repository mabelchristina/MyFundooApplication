using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class NotesRL : INotesRL
    {
        private readonly string _connectionString;

        public NotesRL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("defaultConnection");
        }
        public List<Notes> GetAllUsersNotes()
        {
            try
            {
                DataSet dataSet = new DataSet();
                List<Notes> usersList = new List<Notes>();
                string storedProcedure = "spGetAllNote";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(storedProcedure, connection);
                    adapter.Fill(dataSet, "Note");

                    foreach (DataRow row in dataSet.Tables["Note"].Rows)
                    {
                        Notes notes = new Notes();
                        notes.NotesId = (int)row["NotesId"];
                        notes.Title = (string)row["Title"];
                        notes.Description = (string)row["Description"];
                        notes.Reminder = (string)row["Reminder"];
                        notes.UserId = (int)row["UserId"];


                        usersList.Add(notes);
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
        public Notes AddNotes(Notes note)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("spAddUserNotes", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Title", note.Title);
                    command.Parameters.AddWithValue("@Description", note.Description);
                    command.Parameters.AddWithValue("@Reminder", note.Reminder);
                    command.Parameters.AddWithValue("@UserId", note.UserId);
                    var result = command.ExecuteNonQuery();
                    connection.Close();
                    if (result != 0)
                    {

                        Console.WriteLine("successfully registered" + note.Title + note.Description + note.Reminder
                             + note.UserId);

                    }
                    return note;
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

        public Notes UpdateNote(Notes note)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    SqlCommand command = new SqlCommand("spUpdateNotes", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@NotesId", note.NotesId);
                    command.Parameters.AddWithValue("@Title", note.Title);
                    command.Parameters.AddWithValue("@Description", note.Description);
                    command.Parameters.AddWithValue("@Reminder", note.Reminder);

                    connection.Open();
                    var result = command.ExecuteNonQuery();
                    if (result != 0)
                    {
                       Console.WriteLine("successfully updated" + note.Title + note.Description + note.Reminder
                             + note.UserId);
                    }
                    return note;
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
        public void DeleteNote(Notes note)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    SqlCommand command = new SqlCommand("spDeleteNote", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Title", note.Title);
                    connection.Open();
                    var result = command.ExecuteNonQuery();
                    if (result != 0)
                    {
                        Console.WriteLine("successfully updated");
                    }
                
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
        public bool Archive(int NotesId)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    //Creating a stored Procedure for change color in Notes
                    DateTime now = DateTime.Now;
                    connection.Open();
                    SqlCommand com = new SqlCommand("spArchieve", connection);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@NotesId", NotesId);
                    var result = com.ExecuteNonQuery();
                    Notes note = new Notes();
                    SqlDataReader rd = com.ExecuteReader();
                    if (rd.Read())
                    {
                        note.NotesId = rd["NotesId"] == DBNull.Value ? default : rd.GetInt32("NotesId");
                        note.Title = rd["Title"] == DBNull.Value ? default : rd.GetString("Title");
                        note.Description = rd["Description"] == DBNull.Value ? default : rd.GetString("Description");
                        note.UserId = rd["UserId"] == DBNull.Value ? default : rd.GetInt32("UserId");
                        note.color = rd["color"] == DBNull.Value ? default : rd.GetString("color");
                        note.CreatedDate = rd["CreatedDate"] == DBNull.Value ? default : rd.GetDateTime("CreatedDate");
                        note.ModifiedDate = rd["ModifiedDate"] == DBNull.Value ? default : rd.GetDateTime("ModifiedDate");
                    }
                    if (result > 0)
                    {
                        Console.WriteLine("NOte archived Successfully ");
                    }
                    else
                    {
                        Console.WriteLine("No records effected!");
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

    
        public bool ChangeColor(int NotesId, string color)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    //Creating a stored Procedure for change color in Notes
                    DateTime now = DateTime.Now;
                    connection.Open();
                    SqlCommand com = new SqlCommand("spColor", connection);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@NotesId", NotesId);
                    com.Parameters.AddWithValue("@color", color);
                    var result = com.ExecuteNonQuery();
                    Notes note = new Notes();
                    SqlDataReader rd = com.ExecuteReader();
                    if (rd.Read())
                    {
                        note.NotesId = rd["NotesId"] == DBNull.Value ? default : rd.GetInt32("NotesId");
                        note.Title = rd["Title"] == DBNull.Value ? default : rd.GetString("Title");
                        note.Description = rd["Description"] == DBNull.Value ? default : rd.GetString("Description");
                        note.UserId = rd["UserId"] == DBNull.Value ? default : rd.GetInt32("UserId");
                        note.color = rd["color"] == DBNull.Value ? default : rd.GetString("color");
                        note.CreatedDate = rd["CreatedDate"] == DBNull.Value ? default : rd.GetDateTime("CreatedDate");
                        note.ModifiedDate = rd["ModifiedDate"] == DBNull.Value ? default : rd.GetDateTime("ModifiedDate");
                    }
                    if (result > 0)
                    {
                        Console.WriteLine("Color Successfully Updated On Note Table");
                    }
                    else
                    {
                        Console.WriteLine("No records effected!");
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        public bool Pin(int NotesId)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    //Creating a stored Procedure for change color in Notes
                    DateTime now = DateTime.Now;
                    connection.Open();
                    SqlCommand com = new SqlCommand("spPin", connection);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@NotesId", NotesId);
                    var result = com.ExecuteNonQuery();
                    Notes note = new Notes();
                    SqlDataReader rd = com.ExecuteReader();
                    if (rd.Read())
                    {
                        note.NotesId = rd["NotesId"] == DBNull.Value ? default : rd.GetInt32("NotesId");
                        note.Title = rd["Title"] == DBNull.Value ? default : rd.GetString("Title");
                        note.Description = rd["Description"] == DBNull.Value ? default : rd.GetString("Description");
                        note.UserId = rd["UserId"] == DBNull.Value ? default : rd.GetInt32("UserId");
                        note.color = rd["color"] == DBNull.Value ? default : rd.GetString("color");
                        note.CreatedDate = rd["CreatedDate"] == DBNull.Value ? default : rd.GetDateTime("CreatedDate");
                        note.ModifiedDate = rd["ModifiedDate"] == DBNull.Value ? default : rd.GetDateTime("ModifiedDate");
                    }
                    if (result > 0)
                    {
                        Console.WriteLine("Noten pinned Successfully");
                    }
                    else
                    {
                        Console.WriteLine("No records effected!");
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
