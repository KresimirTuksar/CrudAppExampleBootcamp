using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExampleApp.WebApi.Requests.Advertisement
{
    public class UserRequestModel
    {
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int Phone { get; set; } 
    }
}