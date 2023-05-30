using ExampleApp.WebApi.Requests.Advertisement;
using ExampleApp.WebApi.Responses;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Xml.Linq;

namespace ExampleApp.WebApi.Controllers
{
    public class AdvertisementController : ApiController
    {

        public static string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=FapMash1na;Database=Advertisement;";



        public IEnumerable<UserResponseModel> Get()
        {
            List<UserResponseModel> result = new List<UserResponseModel>();
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
                return result;
            }
        }

        public UserResponseModel Get(Guid id)
        {
            List<UserResponseModel> result = new List<UserResponseModel>();
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
                return result.FirstOrDefault();
            }
        }

        public void Post([FromBody] UserRequestModel request)
        {
            Guid userGuid = Guid.NewGuid();
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

                cmd.ExecuteNonQuery();
            }
        }

        public void Put(Guid id, [FromBody] UserRequestModel request)
        {

            UserResponseModel user;

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

                            if (user.FirstName == request.Firstname ||
                                user.LastName == request.Lastname ||
                                user.City == request.City ||
                                user.Address == request.Address ||
                                user.Email == request.Email ||
                                user.Phone == request.Phone.ToString())
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

                                    updateCmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                Console.WriteLine("New values are identical to the existing values. No update necessary.");
                            }
                        }
                    }
                }
            }
        }

        public void Delete(Guid id)
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
                    Console.WriteLine("Entry deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to delete entry. Entry may not exist or the provided ID is invalid.");
                }
            }
        }
    }
}