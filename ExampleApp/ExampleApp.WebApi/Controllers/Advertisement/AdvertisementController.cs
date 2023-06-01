using ExampleApp.Model;
using ExampleApp.Service;
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
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Xml.Linq;

namespace ExampleApp.WebApi.Controllers.Advertisement
{
    [RoutePrefix("api/advertisement")]

    public class AdvertisementController : ApiController
    {

        public static string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=FapMash1na;Database=Advertisement;";
        public AdvertisementService Service { get; set; } = new AdvertisementService();

        // GET: api/Advertisement
        [HttpGet]
        [Route("getall")]

        public HttpResponseMessage Get()
        {
            List<AdModel> response = Service.GetAllAds();
            List<AdResponseModel> result = new List<AdResponseModel>();

            if (response.Count() == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Not Found");
            }

            //mapping
            foreach (var item in response)
            {
                result.Add(
                    new AdResponseModel()
                    {
                        Id = item.Id.ToString(),
                        UserId = item.UserId.ToString(),
                        Content = item.Content,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt,
                    });
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("getjoined")]

        public HttpResponseMessage GetJoined()
        {
            List<AdModel> response = Service.GetAllAds();

            List<AdJoinedResponseModel> result = new List<AdJoinedResponseModel>();

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();

                    cmd.Connection = conn;
                    //cmd.CommandText = $"SELECT * FROM \"Ads\"";
                    cmd.CommandText = "SELECT \"Ads.Id\", \"Ads.Content\", \"Category.Name\" " +
                                    "FROM \"Ads\" " +
                                    "INNER JOIN \"AdCategory\" ON \"AdCategory.AdId\" = \"Ads.Id\" " +
                                    "INNER JOIN \"Category\" ON \"AdCategory.CategoryId\" = \"Category.Id\" ";
                    NpgsqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result.Add(new AdJoinedResponseModel()
                        {
                            Id = reader["Id"].ToString(),
                            Content = reader["Content"].ToString(),
                            Category = reader["Category"].ToString(),
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



        [HttpGet]
        [Route("getbyid")]

        public HttpResponseMessage Get(Guid id)
        {
            AdModel response = Service.GetAdById(id);
            AdResponseModel result;
            if (response == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Not Found");
            }
            result = new AdResponseModel()
            {
                Id = response.Id.ToString(),
                UserId = response.UserId.ToString(),
                Content = response.Content,
                CreatedAt = response.CreatedAt,
                UpdatedAt = response.UpdatedAt,
            };
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }


        [HttpPost]
        [Route("create")]
        public HttpResponseMessage Post(AdRequestModel request)
        {
            //map from rest model
            AdModel model = new AdModel()
            {
                UserId = request.UserId,
                Content = request.Content,
            };

            bool response = Service.CreateAd(model);

            if (response)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "OK");

            }
            return Request.CreateResponse(HttpStatusCode.OK, "OK");

            //Guid adId = Guid.NewGuid();
            //List<int> categoriesIds = new List<int>();
            //try
            //{
            //    using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            //    {
            //        conn.Open();
            //        using (NpgsqlCommand selectCatCmd = new NpgsqlCommand())
            //        {
            //            foreach (var cat in request.Categories)
            //            {
            //                selectCatCmd.Connection = conn;
            //                selectCatCmd.CommandText = "SELECT \"Id\" FROM \"Category\" WHERE \"Name\" = @category";
            //                selectCatCmd.Parameters.AddWithValue("category", cat);

            //                using (NpgsqlDataReader reader = selectCatCmd.ExecuteReader())
            //                {
            //                    if (reader.Read())
            //                    {
            //                        categoriesIds.Add(reader.GetInt32(0));
            //                    }
            //                    reader.Close();
            //                }
            //            }
            //            using (NpgsqlCommand createAdCmd = new NpgsqlCommand())
            //            {
            //                createAdCmd.Connection = conn;
            //                createAdCmd.CommandText = "INSERT INTO \"Ads\" ( \"Id\", \"UserId\", \"Content\")" +
            //                                                "VALUES( @adGuid, @UserId, @Content);";

            //                createAdCmd.Parameters.AddWithValue("adGuid", adId);
            //                createAdCmd.Parameters.AddWithValue("userId", request.UserId);
            //                createAdCmd.Parameters.AddWithValue("content", request.Content);



            //                int adsRowsAffected = createAdCmd.ExecuteNonQuery();

            //                using (NpgsqlCommand createAdCategoryCmd = new NpgsqlCommand())
            //                {
            //                    int categoryRowsAffected = 0;
            //                    createAdCategoryCmd.Connection = conn;
            //                    foreach (var catId in categoriesIds)
            //                    {
            //                        Guid adCategoryGuid = Guid.NewGuid();
            //                        createAdCategoryCmd.CommandText = "INSERT INTO \"AdCategory\" (\"Id\", \"AdId\", \"CategoryId\" )" +
            //                                                        "VALUES( @id, @adGuid, @categoryId );";

            //                        createAdCategoryCmd.Parameters.AddWithValue("id", adCategoryGuid);
            //                        createAdCategoryCmd.Parameters.AddWithValue("adGuid", adId);
            //                        createAdCategoryCmd.Parameters.AddWithValue("categoryId", catId);

            //                        categoryRowsAffected += createAdCategoryCmd.ExecuteNonQuery();
            //                    }

            //                    if (adsRowsAffected > 0)
            //                    {
            //                        return Request.CreateResponse(HttpStatusCode.OK, "OK");
            //                    }
            //                    else if (adsRowsAffected <= 0)
            //                    {
            //                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed to insert Ad");
            //                    }
            //                    else if (categoryRowsAffected > 0)
            //                    {
            //                        return Request.CreateResponse(HttpStatusCode.OK, "OK");
            //                    }
            //                    else
            //                    {
            //                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed to insert AdCategory");
            //                    }
            //                };

            //            };
            //        }
            //    }
            //}
            //catch (Exception e)
            //{

            //    throw e;
            //}

        }

        [HttpPut]
        [Route("edit")]
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

        [HttpDelete]
        [Route("delete")]
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
