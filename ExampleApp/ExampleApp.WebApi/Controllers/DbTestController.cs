using ExampleApp.WebApi.Responses;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;

namespace ExampleApp.WebApi.Controllers
{
    public class DbTestController : ApiController
    {

        public static string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=FapMash1na;Database=Advertisement;";
        NpgsqlConnection conn = new NpgsqlConnection(connectionString);



        public IEnumerable<UserResponseModel> Get()
        {
            List<UserResponseModel> result = new List<UserResponseModel>();

            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = $"SELECT * FROM users";
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new UserResponseModel()
                {
                    Id = (int)reader["id"],
                    FirstName = reader["firstname"] as string,
                    LastName = reader["lastname"] as string,
                    City = reader["city"] as string,
                    Address = reader["address"] as string,
                    Phone = reader["phone"].ToString(),
                    Email = reader["email"].ToString(),
                });

            }
            return result;
        }

        public string Get(int id)
        {
            return "value";
        }

        public void Post([FromBody] string value)
        {
            Guid userGuid = Guid.NewGuid();
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO Users(FirstName, LastName, City, Address, Phone, Email)" +
                                            "VALUES('Pero', 'Perifddfgdfdć', 'Osijek', 'Ulica 3', 031234321, 'pero@mail.com');";
            cmd.ExecuteNonQuery();
            conn.Close();


        }

        public void Put(int id, [FromBody] string value)
        {
        }

        public void Delete(int id)
        {
        }

    }
}