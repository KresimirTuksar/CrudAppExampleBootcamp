using ExampleApp.WebApi.Models;
using ExampleApp.WebApi.Requests;
using ExampleApp.WebApi.Responses;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ExampleApp.WebApi.Controllers
{
    [RoutePrefix("api/keys")]
    public class KeysController : ApiController
    {
        public static List<Key> Keys { get; set; } = new List<Key>
        {
                new Key{ Id = 1, Name = "House", Owner = "John" },
                new Key{ Id = 2, Name = "Shed", Owner = "Tom" },
                new Key{ Id = 3, Name = "Car" , Owner = "Jerry"}
        };

        public HttpResponseMessage Get()
        {
            if (Keys == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, Keys);

            }
            return Request.CreateResponse(HttpStatusCode.OK, Keys);

        }

        //[HttpPost]
        //[Route("paginated")]
        //public HttpResponseMessage Post()
        //{
        //    if (Keys == null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NotFound, "No Result");
        //    }

        //    List<Key> query = Keys.OrderBy(x => x.Id).Skip(2 * (2-1)).Take(2).ToList();
        //    PaginatedResponseModel<List<Key>> result = new PaginatedResponseModel<List<Key>> { Results = query };

        //    result.TotalResultCount = Keys.Count();
        //    return Request.CreateResponse(HttpStatusCode.OK, result);

        //}
        // GET /api/keys/5
        public HttpResponseMessage Get(int id)
        {

            Key query = Keys.Find(k => k.Id == id);
            if (query == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);

            }
            //maping
            KeyResponseModel response = new KeyResponseModel() {  Name = query.Name, Owner = query.Owner };

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        // POST /api/keys
        [HttpPost]
        [Route("create")]
        public HttpResponseMessage Post([FromBody] KeyRequestModel request)
        {

            //mapping
            Key key = new Key() { Id = request.Id, Name = request.Name, Owner = request.Owner };
            if (Keys == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            Keys.Add(key);

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        // PUT /api/keys/5
        public HttpResponseMessage Put(int id, [FromBody] KeyRequestModel request)
        {
            Key query = Keys.FirstOrDefault(k => k.Id == id);

            if (query == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                if (request.Name != query.Name)
                {
                    if (!string.IsNullOrEmpty(request.Name))
                    {
                        query.Name = request.Name;
                    }
                }

                if (request.Owner != query.Owner)
                {
                    if (!string.IsNullOrEmpty(request.Owner))
                    {
                        query.Owner = request.Owner;
                    }
                }

                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
        }

        // DELETE /api/keys
        public HttpResponseMessage Delete(int id)
        {
            Key query = Keys.Find(k => k.Id == id);
            if (query == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            Keys.Remove(query);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

    }
}