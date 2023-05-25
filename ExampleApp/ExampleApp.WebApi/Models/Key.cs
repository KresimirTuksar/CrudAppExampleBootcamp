using ExampleApp.WebApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExampleApp.WebApi.Models
{
    public class Key: Ikey
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}