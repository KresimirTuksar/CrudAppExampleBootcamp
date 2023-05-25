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
        public HttpResponseMessage Post([FromBody] KeyRequestModel request)
        {

            //mapping
            Key key = new Key() { Id = request.Id, Name = request.Name };
            if (Keys == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            Keys.Add(key);

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        // PUT /api/keys/5
        public HttpResponseMessage Put(int id, [FromBody] Key request)
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
                    query.Name = request.Name;
                }

                if (request.Owner != query.Owner)
                {
                    query.Owner = request.Owner;
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