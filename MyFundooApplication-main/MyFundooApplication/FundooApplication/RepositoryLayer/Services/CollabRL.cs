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
    public class CollabRL:ICollabRL
    {
     
            private readonly string _connectionString;
            public CollabRL(IConfiguration configuration)
            {
                _connectionString = configuration.GetConnectionString("defaultConnection");
            }
            public GetCollab AddCollaboration(int userId, int noteId, GetCollab collaborationModel)
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                try
                {
                    using (connection)
                    {
                        //Creating a stored Procedure for adding collaboration into collaboration table
                        DateTime now = DateTime.Now;
                        connection.Open();
                        SqlCommand com = new SqlCommand("spAddCollab", connection);
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@collabEmail", collaborationModel.EmailID);
                        com.Parameters.AddWithValue("@userId", userId);
                        com.Parameters.AddWithValue("@noteId", noteId);
                        //var result = com.ExecuteNonQuery();
                        GetCollab getCollaboration = new GetCollab();
                        SqlDataReader rd = com.ExecuteReader();
                        if (rd.Read())
                        {
                            getCollaboration.CollabId = rd["collabId"] == DBNull.Value ? default : rd.GetInt32("collabId");
                            getCollaboration.EmailID = rd["collabEmail"] == DBNull.Value ? default : rd.GetString("collabEmail");
                            getCollaboration.UserId = rd["userId"] == DBNull.Value ? default : rd.GetInt32("userId");
                            getCollaboration.NoteId = rd["noteId"] == DBNull.Value ? default : rd.GetInt32("noteId");
                            getCollaboration.registeredDate = rd["registeredDate"] == DBNull.Value ? default : rd.GetDateTime("registeredDate");
                            getCollaboration.modifiedDate = rd["modifiedDate"] == DBNull.Value ? default : rd.GetDateTime("modifiedDate");
                        }
                        return getCollaboration;
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

            public List<GetCollab> GetCollab(int userId, int noteId)
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                try
                {
                    using (connection)
                    {
                        List<GetCollab> getCollaborations = new List<GetCollab>();
                        //Creating a stored Procedure for fetching collab
                        connection.Open();
                        SqlCommand com = new SqlCommand("spGetCollabNote", connection);
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@userId", userId);
                        com.Parameters.AddWithValue("@noteId", noteId);
                        //var result = com.ExecuteNonQuery();
                        SqlDataReader rd = com.ExecuteReader();
                        while (rd.Read())
                        {
                            GetCollab getCollab = new GetCollab();
                            getCollab.CollabId = rd["CollabId"] == DBNull.Value ? default : rd.GetInt32("CollabId");
                            getCollab.EmailID = rd["EmailID"] == DBNull.Value ? default : rd.GetString("EmailID");
                            getCollab.UserId = rd["UserId"] == DBNull.Value ? default : rd.GetInt32("UserId");
                            getCollab.NoteId = rd["NoteId"] == DBNull.Value ? default : rd.GetInt32("NoteId");
                            getCollab.registeredDate = rd["registeredDate"] == DBNull.Value ? default : rd.GetDateTime("registeredDate");
                            getCollab.modifiedDate = rd["modifiedDate"] == DBNull.Value ? default : rd.GetDateTime("modifiedDate");
                            getCollaborations.Add(getCollab);
                        }
                        return getCollaborations;
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

        public bool RemoveCollab(int userId, int noteId, Collab collaborationModel)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    //Creating a stored Procedure for remove collabemail 
                    DateTime now = DateTime.Now;
                    connection.Open();
                    SqlCommand com = new SqlCommand("spRemoveCollab", connection);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@collabEmail", collaborationModel.CollabEmailID);
                    com.Parameters.AddWithValue("@userId", userId);
                    com.Parameters.AddWithValue("@noteId", noteId);
                    var result = com.ExecuteNonQuery();
                    if (result > 0)
                    {
                        Console.WriteLine("Record Successfully Removed from collaboration Table");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("No records effected!");
                        return false;
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
        }
}
