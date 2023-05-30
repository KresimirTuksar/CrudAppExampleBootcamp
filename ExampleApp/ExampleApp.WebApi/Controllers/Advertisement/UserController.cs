using ExampleApp.WebApi.Requests.Advertisement;
using ExampleApp.WebApi.Responses;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExampleApp.WebApi.Controllers
{
    public class UserController : ApiController
    {

        public static string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=FapMash1na;Database=Advertisement;";



        public HttpResponseMessage Get()
        {
            List<UserResponseModel> result = new List<UserResponseModel>();

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();

                    cmd.Connection = conn;
                    cmd.CommandText = $"SELECT * FROM \"Users\"";
                    NpgsqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result.Add(new UserResponseModel()
                        {
                            Id = reader["Id"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            City = reader["City"].ToString(),
                            Address = reader["Address"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Email = reader["Email"].ToString(),
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
            List<UserResponseModel> result = new List<UserResponseModel>();
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();

                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM \"Users\" WHERE \"Id\" = @userGuid";

                    cmd.Parameters.AddWithValue("userGuid", id);

                    NpgsqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result.Add(new UserResponseModel()
                        {
                            Id = reader["Id"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            City = reader["City"].ToString(),
                            Address = reader["Address"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Email = reader["Email"].ToString(),
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

        public HttpResponseMessage Post([FromBody] UserRequestModel request)
        {
            Guid userGuid = Guid.NewGuid();

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();

                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO \"Users\" ( \"Id\", \"FirstName\", \"LastName\", \"City\", \"Address\", \"Phone\", \"Email\")" +
                                                    "VALUES( @userGuid, @FirstName, @Lastname, @City, @Address, @Phone, @Email);";

                    cmd.Parameters.AddWithValue("userGuid", userGuid);
                    cmd.Parameters.AddWithValue("FirstName", request.Firstname);
                    cmd.Parameters.AddWithValue("Lastname", request.Lastname);
                    cmd.Parameters.AddWithValue("City", request.City);
                    cmd.Parameters.AddWithValue("Address", request.Address);
                    cmd.Parameters.AddWithValue("Email", request.Email);
                    cmd.Parameters.AddWithValue("Phone", request.Phone);

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

        public HttpResponseMessage Put(Guid id, [FromBody] UserRequestModel request)
        {

            UserResponseModel user;

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    using (NpgsqlCommand selectCmd = new NpgsqlCommand())
                    {
                        selectCmd.Connection = conn;
                        selectCmd.CommandText = "SELECT * FROM \"Users\" WHERE \"Id\" = @userGuid";

                        selectCmd.Parameters.AddWithValue("userGuid", id);


                        using (NpgsqlDataReader reader = selectCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new UserResponseModel()
                                {
                                    Id = reader["Id"].ToString(),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    City = reader["City"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    Phone = reader["Phone"].ToString(),
                                    Email = reader["Email"].ToString(),
                                };


                                if (user.FirstName != request.Firstname ||
                                    user.LastName != request.Lastname ||
                                    user.City != request.City ||
                                    user.Address != request.Address ||
                                    user.Email != request.Email ||
                                    user.Phone != request.Phone.ToString())
                                {
                                    reader.Close();

                                    using (NpgsqlCommand updateCmd = new NpgsqlCommand())
                                    {
                                        updateCmd.Connection = conn;

                                        updateCmd.CommandText = "UPDATE \"Users\" SET \"FirstName\" = @firstName, \"LastName\" = @lastname, \"City\" = @city, \"Address\" = @address, \"Phone\" = @phone, \"Email\" = @email WHERE \"Id\" = @userGuid";

                                        updateCmd.Parameters.AddWithValue("userGuid", id);
                                        updateCmd.Parameters.AddWithValue("firstName", request.Firstname);
                                        updateCmd.Parameters.AddWithValue("lastname", request.Lastname);
                                        updateCmd.Parameters.AddWithValue("city", request.City);
                                        updateCmd.Parameters.AddWithValue("address", request.Address);
                                        updateCmd.Parameters.AddWithValue("email", request.Email);
                                        updateCmd.Parameters.AddWithValue("phone", request.Phone);

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
                            return Request.CreateResponse(HttpStatusCode.NotFound, "User Not Found");

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

                    command.CommandText = "DELETE FROM \"Users\" WHERE \"Id\" = @entryId";

                    command.Parameters.AddWithValue("entryId", id);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, "Deleted");
                    }

                    return Request.CreateResponse(HttpStatusCode.NotFound, "User Not Found");
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}