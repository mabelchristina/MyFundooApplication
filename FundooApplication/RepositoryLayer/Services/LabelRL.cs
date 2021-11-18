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
    public class LabelRL:ILabelRL
    {
        private readonly string _connectionString;
        public LabelRL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("defaultConnection");
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
