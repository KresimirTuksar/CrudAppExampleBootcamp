using ExampleApp.Model;
using ExampleApp.Service;
using ExampleApp.WebApi.Helpers;
using ExampleApp.WebApi.Requests.Advertisement;
using ExampleApp.WebApi.Responses.Advertisement;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

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

        public async Task<HttpResponseMessage> Get()
        {
            List<AdModel> response = await Service.GetAllAdsAsync();
            List<AdResponseModel> result;

            if (response.Count() == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Not Found");
            }

            result = HelpersGeneral.MapToResult(response);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("getpaginated")]
        public HttpResponseMessage Post()
        {
            if (Keys == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No Result");
            }

            List<Key> query = Keys.OrderBy(x => x.Id).Skip(2 * (2 - 1)).Take(2).ToList();
            PaginatedResponseModel<List<Key>> result = new PaginatedResponseModel<List<Key>> { Results = query };

            result.TotalResultCount = Keys.Count();
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }


        //[HttpGet]
        //[Route("getjoined")]

        //public HttpResponseMessage GetJoined()
        //{
        //    List<AdModel> response = Service.GetAllAdsCategories();

        //    List<AdJoinedResponseModel> result = new List<AdJoinedResponseModel>();

        //    try
        //    {
        //        using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
        //        {
        //            conn.Open();
        //            NpgsqlCommand cmd = new NpgsqlCommand();

        //            cmd.Connection = conn;
        //            //cmd.CommandText = $"SELECT * FROM \"Ads\"";
        //            cmd.CommandText = "SELECT \"Ads.Id\", \"Ads.Content\", \"Category.Name\" " +
        //                            "FROM \"Ads\" " +
        //                            "INNER JOIN \"AdCategory\" ON \"AdCategory.AdId\" = \"Ads.Id\" " +
        //                            "INNER JOIN \"Category\" ON \"AdCategory.CategoryId\" = \"Category.Id\" ";
        //            NpgsqlDataReader reader = cmd.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                result.Add(new AdJoinedResponseModel()
        //                {
        //                    Id = reader["Id"].ToString(),
        //                    Content = reader["Content"].ToString(),
        //                    Category = reader["Category"].ToString(),
        //                });
        //            }
        //            if (result.Count() == 0)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.NotFound, "Not Found");

        //            }
        //            return Request.CreateResponse(HttpStatusCode.OK, result);
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //        throw e;
        //    }
        //}



        [HttpGet]
        [Route("getbyid")]

        public async Task<HttpResponseMessage> Get(Guid id)
        {
            //AdModel response = Service.GetAdById(id);
            AdResponseModel result;
            List<AdModel> response = new List<AdModel>() { await Service.GetAdByIdAsync(id) };

            if (response.Count() == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Not Found");
            }

            result = HelpersGeneral.MapToResult(response).FirstOrDefault();

            return Request.CreateResponse(HttpStatusCode.OK, result);

        }


        [HttpPost]
        [Route("create")]
        public  async Task<HttpResponseMessage> Post(AdRequestModel request)
        {
            //map from rest model
            AdModel model = new AdModel()
            {
                UserId = request.UserId,
                Content = request.Content,
            };


            bool response = await Service.CreateAdAsync(model);

            if (response)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "OK");

            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed");
        }

        [HttpPut]
        [Route("edit")]
        public async Task<HttpResponseMessage> Put(Guid id, AdRequestModel request)
        {

            AdResponseModel ad;
            DateTime now = DateTime.Now;

            AdModel model = new AdModel()
            {
                Content = request.Content,
            };


            bool response =  await Service.EditAdAsync(model, id);

            if (response)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed");
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            bool response = await Service.DeleteAdAsync(id);

            if (response)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed");
        }
    }
}
