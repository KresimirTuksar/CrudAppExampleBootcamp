using ExampleApp.WebApi.Models;
using ExampleApp.WebApi.Requests;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ExampleApp.WebApi.Controllers
{
    public class KeysController : ApiController
    {
        public List<Key> Keys { get; set; }

        public KeysController()
        {
            Keys = new List<Key>()
            {
                new Key{ Id = 1, Name = "House" },
                new Key{ Id = 2, Name = "Shed" },
                new Key{ Id = 3, Name = "Car" }
            };
        }
        // GET /api/keys
        public IEnumerable<Key> Get()
        {
            return Keys;
        }

        // GET /api/keys/5
        public Key Get(int id)
        {
            return Keys.Find(k => k.Id == id);

        }

        // POST /api/keys
        public List<Key> Post([FromBody] KeyRequestModel request)
        {
            //mapping
            Key key = new Key() { Id = request.Id, Name = request.Name };

            Keys.Add(key);

            return Keys;
        }

        // PUT /api/keys/5
        public string Put(int id, [FromBody] Key request)
        {
            Key query = Keys.FirstOrDefault(k => k.Id == id);

            if (query == null)
            {
                return "Key not found";
            }
            else
            {
                if (request.Name == query.Name)
                {
                    return "No changes, nothing to edit";
                }
                else
                {
                    query.Name = request.Name;
                    return "Ok";
                }
            }
        }

        // DELETE /api/keys
        public void Delete(int id)
        {
            Key query = Keys.Find(k => k.Id == id);
            Keys.Remove(query);
        }

    }
}