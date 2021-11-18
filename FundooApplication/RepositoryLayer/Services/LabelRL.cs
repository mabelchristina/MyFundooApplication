using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Emit;
using System.Text;

namespace RepositoryLayer.Services
{
    public class LabelRL : ILabelRL
    {
        private readonly string _connectionString;
        public LabelRL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("defaultConnection");
        }
        public List<NoteLabel> GetAllLabel()
        {
            try
            {
                DataSet dataSet = new DataSet();
                List<NoteLabel> usersList = new List<NoteLabel>();
                string storedProcedure = "spGetAllLabel";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(storedProcedure, connection);
                    adapter.Fill(dataSet, "NoteLabel");

                    foreach (DataRow row in dataSet.Tables["NoteLabel"].Rows)
                    {
                        NoteLabel notes = new NoteLabel();
                        notes.labelId = (int)row["labelId"];
                        notes.labelName = (string)row["labelName"];
                        notes.UserId = (int)row["UserId"];
                        notes.noteId = (int)row["noteId"];


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
        public NoteLabel AddLabel(NoteLabel note)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("spAddLabel", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@labelName", note.labelName);
                    command.Parameters.AddWithValue("@UserId", note.UserId);
                    command.Parameters.AddWithValue("@noteId", note.noteId);
                    var result = command.ExecuteNonQuery();
                    connection.Close();
                    if (result != 0)
                    {

                        Console.WriteLine("successfully registered" + note.labelId + note.labelName + note.UserId
                             + note.noteId);

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
    }
}
