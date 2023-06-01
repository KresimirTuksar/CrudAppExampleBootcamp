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
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed");
        }

        [HttpPut]
        [Route("edit")]
        public HttpResponseMessage Put(Guid id, AdRequestModel request)
        {

            AdResponseModel ad;
            DateTime now = DateTime.Now;

            AdModel model = new AdModel()
            {
                Content = request.Content,
            };


            bool response = Service.EditAd(model, id);

            if (response)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed");
        }

        [HttpDelete]
        [Route("delete")]
        public HttpResponseMessage Delete(Guid id)
        {
            bool response = Service.DeleteAd(id);

            if (response)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed");

        }
    }
}
