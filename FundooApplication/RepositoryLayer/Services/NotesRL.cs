using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

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
    }
}
