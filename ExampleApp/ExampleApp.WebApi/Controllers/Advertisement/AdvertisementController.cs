using ExampleApp.WebApi.Requests.Advertisement;
using ExampleApp.WebApi.Responses;
using ExampleApp.WebApi.Responses.Advertisement;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
                            CreatedAt = reader.GetFieldValue<DateTime>(reader.GetOrdinal("CreatedAt"))
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

        public HttpResponseMessage Post(AdRequestModel request)
        {
            Guid adGuid = Guid.NewGuid();
            List<int> categoriesIds = new List<int>();
            StringBuilder queryBuilder = new StringBuilder("");
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (NpgsqlCommand selectCatCmd = new NpgsqlCommand())
                    {
                        foreach (var cat in request.Categories)
                        {
                            selectCatCmd.Connection = conn;
                            selectCatCmd.CommandText = "SELECT \"Id\" FROM \"Category\" WHERE \"Name\" = @category";
                            selectCatCmd.Parameters.AddWithValue("category", cat);

                            using (NpgsqlDataReader reader = selectCatCmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    categoriesIds.Add(reader.GetInt32(0));
                                }
                                reader.Close();
                            }
                        }
                        using (NpgsqlCommand createAdCmd = new NpgsqlCommand())
                        {
                            //queryBuilder.Append("with new_ad as ( insert into \"Ads\"(\"Id\", \"UserId\", \"Content\") values (@adGuid, @UserId, @Content) returning \"Id\")");
                            //for (int i = 0; i < categoriesIds.Count()-1; i++)
                            //{
                            //    queryBuilder.Append($",new_adCategory{i} as ( INSERT INTO \"AdCategory\" (\"Id\", \"AdId\", \"CategoryId\" ) values  ({Guid.NewGuid()} (select \"Id\" from new_ad), {categoriesIds[i]}))");
                            //}

                            createAdCmd.Connection = conn;
                            createAdCmd.CommandText = queryBuilder.ToString();
                            createAdCmd.CommandText = "INSERT INTO \"Ads\" ( \"Id\", \"UserId\", \"Content\")" +
                                                            "VALUES( @adGuid, @UserId, @Content);";

                            createAdCmd.Parameters.AddWithValue("adGuid", adGuid);
                            createAdCmd.Parameters.AddWithValue("userId", request.UserId);
                            createAdCmd.Parameters.AddWithValue("content", request.Content);



                            int adsRowsAffected = createAdCmd.ExecuteNonQuery();

                            using (NpgsqlCommand createAdCategoryCmd = new NpgsqlCommand())
                            {
                                int categoryRowsAffected = 0;
                                createAdCategoryCmd.Connection = conn;
                                foreach (var catId in categoriesIds)
                                {
                                    Guid adCategoryGuid = Guid.NewGuid();
                                    createAdCategoryCmd.CommandText = "INSERT INTO \"AdCategory\" (\"Id\", \"AdId\", \"CategoryId\" )" +
                                                                    "VALUES( @id, @adGuid, @categoryId );";

                                    createAdCategoryCmd.Parameters.AddWithValue("id", adCategoryGuid);
                                    createAdCategoryCmd.Parameters.AddWithValue("adGuid", adGuid);
                                    createAdCategoryCmd.Parameters.AddWithValue("categoryId", catId);

                                    categoryRowsAffected += createAdCategoryCmd.ExecuteNonQuery();
                                }

                                if (adsRowsAffected > 0)
                                {
                                    return Request.CreateResponse(HttpStatusCode.OK, "OK");
                                }
                                else if (adsRowsAffected <= 0)
                                {
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed to insert Ad");
                                }
                                else if (categoryRowsAffected > 0)
                                {
                                    return Request.CreateResponse(HttpStatusCode.OK, "OK");
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed to insert AdCategory");
                                }
                            };

                        };
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public HttpResponseMessage Put(Guid id, AdRequestModel request)
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
