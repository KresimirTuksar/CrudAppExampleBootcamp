using ExampleApp.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExampleApp.WebApi.Responses
{
    public class PaginatedResponseModel<T>
    {
        public int Page { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int TotalResultCount { get; set; } = 0;
        public int Pagesize { get; set; } = 10;
        public List<T> Results { get; set; }

    }
}