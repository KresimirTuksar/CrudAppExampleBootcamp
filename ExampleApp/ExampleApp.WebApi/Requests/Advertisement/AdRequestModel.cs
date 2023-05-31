using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExampleApp.WebApi.Requests.Advertisement
{
    public class AdRequestModel
    {
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public List<string> Categories { get; set; }
    }
}