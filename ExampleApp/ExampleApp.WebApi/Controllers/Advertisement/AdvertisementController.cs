﻿using ExampleApp.WebApi.Requests.Advertisement;
using ExampleApp.WebApi.Responses;
using ExampleApp.WebApi.Responses.Advertisement;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExampleApp.WebApi.Controllers.Advertisement
{
    public class AdvertisementController : ApiController
    {

        public static string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=FapMash1na;Database=Advertisement;";

        // GET: api/Advertisement
        public HttpResponseMessage Get()
        {
            List<AdResponseModel> result = new List<AdResponseModel>();

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();

                    cmd.Connection = conn;
                    cmd.CommandText = $"SELECT * FROM \"Ads\"";
                    NpgsqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result.Add(new AdResponseModel()
                        {
                            Id = reader["Id"].ToString(),
                            UserId = reader["UserId"].ToString(),
                            Content = reader["Content"].ToString(),
                            CreatedAt = reader.GetFieldValue<DateTime>(reader.GetOrdinal("CreatedAt")),
                            UpdatedAt = reader.GetFieldValue<DateTime>(reader.GetOrdinal("UpdatedAt"))
                        }); 
                    }
                    if (result.Count() == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Not Found");

                    }
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public HttpResponseMessage Get(Guid id)
        {
            List<AdResponseModel> result = new List<AdResponseModel>();
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();

                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM \"Ads\" WHERE \"Id\" = @adGuid";

                    cmd.Parameters.AddWithValue("adGuid", id);

                    NpgsqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result.Add(new AdResponseModel()
                        {
                            Id = reader["Id"].ToString(),
                            UserId = reader["UserId"].ToString(),
                            Content = reader["Content"].ToString(),
                            CreatedAt =reader.GetFieldValue<DateTime>(reader.GetOrdinal("CreatedAt"))
                        });
                    }
                    if (result.Count() == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Not Found");

                    }
                    return Request.CreateResponse(HttpStatusCode.OK, result.FirstOrDefault());
                }

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public HttpResponseMessage Post([FromBody] AdRequestModel request)
        {
            Guid adGuid = Guid.NewGuid();

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();

                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO \"Ads\" ( \"Id\", \"UserId\", \"Content\")" +
                                                    "VALUES( @adGuid, @UserId, @Content);";

                    cmd.Parameters.AddWithValue("adGuid", adGuid);
                    cmd.Parameters.AddWithValue("userId", request.UserId);
                    cmd.Parameters.AddWithValue("content", request.Content);
                    

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "OK");
                    }
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Failed to insert User");

                }
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public HttpResponseMessage Put(Guid id, [FromBody] AdRequestModel request)
        {

            AdResponseModel ad;
            DateTime now = DateTime.Now;

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    using (NpgsqlCommand selectCmd = new NpgsqlCommand())
                    {
                        selectCmd.Connection = conn;
                        selectCmd.CommandText = "SELECT * FROM \"Ads\" WHERE \"Id\" = @adGuid";

                        selectCmd.Parameters.AddWithValue("adGuid", id);


                        using (NpgsqlDataReader reader = selectCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ad = new AdResponseModel()
                                {
                                    Id = reader["Id"].ToString(),
                                    UserId = reader["UserId"].ToString(),
                                    Content = reader["Content"].ToString(),
                                };


                                if (ad.Content != request.Content)
                                {
                                    reader.Close();

                                    using (NpgsqlCommand updateCmd = new NpgsqlCommand())
                                    {
                                        updateCmd.Connection = conn;

                                        updateCmd.CommandText = "UPDATE \"Ads\" SET \"Content\" = @content, \"UpdatedAt\" = @updatedAt WHERE \"Id\" = @adGuid";

                                        updateCmd.Parameters.AddWithValue("adGuid", id);
                                        updateCmd.Parameters.AddWithValue("content", request.Content);
                                        updateCmd.Parameters.AddWithValue("updatedAt", now);


                                        int rowsAffected = updateCmd.ExecuteNonQuery();

                                        if (rowsAffected > 0)
                                        {
                                            return Request.CreateResponse(HttpStatusCode.NoContent, "Updated");

                                        }
                                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed to update");

                                    }
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.NoContent, "Nothing to do");
                                }
                            }
                            return Request.CreateResponse(HttpStatusCode.NotFound, "Ad Not Found");

                        }
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public HttpResponseMessage Delete(Guid id)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    NpgsqlCommand command = new NpgsqlCommand();

                    command.Connection = conn;

                    command.CommandText = "DELETE FROM \"Ads\" WHERE \"Id\" = @adId";

                    command.Parameters.AddWithValue("adId", id);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, "Deleted");
                    }

                    return Request.CreateResponse(HttpStatusCode.NotFound, "Ad Not Found");
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
